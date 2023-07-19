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
    public class CustomDeckUseCase
    {
        public static void EnableFirstDeckOnly(string waPath)
        {
            var bytesArray = File.ReadAllBytes(waPath);

            string hexStringToFirstDeckOnly = "000040A01D80023C000256241780023CDCD3548C000000002C008012DCD357241000BE27280013240200942600000000000000000000000021900000218040022188800221A02002000000000000229600000000110040102120D00300008390000000002B1062000500401401006224010010260100732687AA050802009426000082A0010002262101042639B3000C0000C2A687AA05080200D62601001026D202022AE7FF401402003126FFFF7326E3FF6016000000000400F7260000F48E00000000D7FF8016000000000C03BF8F0803BE8F0403B78F0003B68FFC02B58FF802B48FF402B38FF002B28FEC02B18FE802B08F0800E0031003BD27E0FFBD271000B0AF1D80103C000010262120000221280000003006241800BFAFD2D5000C1400B1AF1780043C90808424212800021C3A020C00300624FBA0050C00000000534B000C000000006439020C0000000002A7050C00000000F9FF4010000000004CAA050C218000001D80023C000251240C0424260B0003240000829001008424FFFF6324FCFF6104268002020A80023C9CB0428CC6AA0508001202006439020C000000000012020025105000FBFF4010340322AE1800BF8F1400B18F1000B08F0800E0032000BD270000000000000000";
            int offset = 0xF92970;
            Hexadecimal.PutHex(bytesArray, offset, hexStringToFirstDeckOnly);

            offset = 0xFBD970;
            Hexadecimal.PutHex(bytesArray, offset, hexStringToFirstDeckOnly);

            File.WriteAllBytes(waPath, bytesArray);
        }

        public static void EnableSixDecks(string waPath)
        {
            // Dont change at Deck 7 (seven deck)
            var bytesArray = File.ReadAllBytes(waPath);

            string hexStringToSixStaticDecks = "000040A01D80023C000256241780023CDCD3548C000000002C008012DCD357241000BE278AB305082800132402009426000000000000000021900000218040022188800221A02002000000000000229600000000110040102120D00300008390000000002B1062000500401401006224010010260100732687AA050802009426000082A0010002262101042639B3000C0000C2A687AA05080200D62601001026D202022AE7FF401402003126FFFF7326E3FF6016000000000400F7260000F48E0000000000000000000000000C03BF8F0803BE8F0403B78F0003B68FFC02B58FF802B48FF402B38FF002B28FEC02B18FE802B08F0800E0031003BD27E0FFBD271000B0AF1D80103C000010262120000221280000003006241800BFAFD2D5000C1400B1AF1780043C90808424212800021C3A020C00300624FBA0050C00000000534B000C000000006439020C0000000002A7050C00000000F9FF4010000000004CAA050C218000001D80023C000251240C0424260B0003240000829001008424FFFF6324FCFF6104268002020A80023C9CB0428CC6AA0508001202006439020C000000000012020025105000FBFF4010340322AE1800BF8F1400B18F1000B08F0800E0032000BD270000000000000000";
            int offset = 0xF92970;
            Hexadecimal.PutHex(bytesArray, offset, hexStringToSixStaticDecks);
            offset = 0xFBD970;
            Hexadecimal.PutHex(bytesArray, offset, hexStringToSixStaticDecks);

            hexStringToSixStaticDecks = "6439020C219000000F00423042100200070012240200421600000000FFFF42240200401400000000010002240400122418005200129000001780023C21105200D8D3548C67AA050821900000";
            offset = 0xF94E28;
            Hexadecimal.PutHex(bytesArray, offset, hexStringToSixStaticDecks);

            File.WriteAllBytes(waPath, bytesArray);
        }

        public static void SetInitialDeck(StarterDeck[] starterDeck)
        {
            using (var starterDeckStream = new FileStream(Configurations.WaPath, FileMode.Open))
            {
                var position = 0;
                foreach (var deck in starterDeck)
                {
                    WriteDeckByPosition(starterDeckStream, deck, position);
                    position++;
                }
            }
        }

        private static void WriteDeckByPosition(FileStream stream, StarterDeck deck, int position) {
            stream.Position = 0xF92BD4 + (0x5B8 * position);

            using (var memoryStream = new MemoryStream(0x5A6))
            {
                var droppedUInt16 = (ushort)deck.Dropped;

                memoryStream.Write(droppedUInt16.Int16ToByteArray(), 0, 2);

                foreach (var Card in deck.Cards)
                {
                    var cardUShort = (ushort)Card;
                    memoryStream.Write(cardUShort.Int16ToByteArray(), 0, 2);
                }

                stream.Write(memoryStream.ToArray(), 0, 0x5A6);
            }
        }
    }
}
