using FM.Core.Application;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FM.Core.Usecases
{
    public class LoadCharacterTableUseCase
    {
        readonly string filePath = string.Empty;
        readonly string IS_VALID_CHARACTER_LINE_PATTERN = "^([A-Fa-f0-9]{2})\\=(.*)$";

        public LoadCharacterTableUseCase(string filePath) {
            this.filePath = filePath;
        }
        
        public bool Execute()
        {
            var stringReader = new StringReader(File.ReadAllText(this.filePath));
            string input;

            while ((input = stringReader.ReadLine()) != null)
            {
                var match = Regex.Match(input, IS_VALID_CHARACTER_LINE_PATTERN);

                if (!match.Success)
                {
                    continue;
                }

                var k1 = Convert.ToChar(match.Groups[2].ToString());
                var k2 = (byte)int.Parse(match.Groups[1].ToString(), NumberStyles.HexNumber);

                Configurations.CharacterTableDict.Add(k2, k1);

                if (!Configurations.CharacterTableRDict.ContainsKey(k1))
                {
                    Configurations.CharacterTableRDict.Add(k1, k2);
                }
            }

            if (Configurations.CharacterTableDict.Values.Count != 85)
            {
                return false;
            }

            return true;
        }
    }
}
