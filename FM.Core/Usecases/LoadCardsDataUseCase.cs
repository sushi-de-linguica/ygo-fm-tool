using FM.Core.Application;
using FM.Core.Domain.Models;
using FM.Core.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FM.Core.Usecases
{
    public class LoadCardsDataUseCase
    {
        public LoadCardsDataUseCase() { }

        private void ExtractBasicData(MemoryStream stream) {
            stream.Position = 0x1C4A44L;

            for (int i = 0; i < Static.MAX_CARDS; i++)
            {
                int int32 = stream.ExtractPiece(0, 4).ExtractInt32();

                Configurations.Cards[i] = new Card
                {
                    Id = i + 1,
                    Attack = (int32 & 0x1FF) * 10,
                    Defense = (int32 >> 9 & 0x1FF) * 10,
                    GuardianStar2 = int32 >> 18 & 0xF,
                    GuardianStar1 = int32 >> 22 & 0xF,
                    Type = int32 >> 26 & 0x1F
                };
            }
        }

        private void ExtractLevelAndAttribute(MemoryStream stream)
        {
            stream.Position = 0x1C5B33L;

            for (int i = 0; i < Static.MAX_CARDS; i++)
            {
                byte num = stream.ExtractPiece(0, 1)[0];

                Configurations.Cards[i].Level = num & 0xF;
                Configurations.Cards[i].Attribute = num >> 4 & 0xF;
            }
        }

        private void ExtractCardName(MemoryStream stream)
        {
            for (int i = 0; i < Static.MAX_CARDS; i++)
            {
                stream.Position = 0x1C6002 + i * 2;

                int num = stream.ExtractPiece(0, 2).ExtractUInt16() & ushort.MaxValue;

                stream.Position = 0x1C6800 + num - 0x6000;
                Configurations.Cards[i].Name = stream.GetText(Configurations.CharacterTableDict);
            }
        }

        private void ExtractCardDescription(MemoryStream stream)
        {
            for (int i = 0; i < Static.MAX_CARDS; i++)
            {
                stream.Position = 0x1B0A02 + i * 2;

                int num3 = stream.ExtractPiece(0, 2).ExtractUInt16();

                stream.Position = 0x1B11F4 + (num3 - 0x9F4);
                Configurations.Cards[i].Description = stream.GetText(Configurations.CharacterTableDict);
            }
        }

        public void Execute()
        {
            var stream = new MemoryStream(File.ReadAllBytes(Configurations.SlusPath));

            ExtractBasicData(stream);
            ExtractLevelAndAttribute(stream);
            ExtractCardName(stream);
            ExtractCardDescription(stream);

            stream.Close();
        }
    }
}
