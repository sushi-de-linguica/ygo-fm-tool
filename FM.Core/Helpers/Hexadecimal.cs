using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FM.Core.Helpers
{
    public class Hexadecimal
    {
        public static byte[] ConvertHexStringToByteArray(string hexadecimalString)
        {
            if ((hexadecimalString.Length % 2) > 0)
            {
                return new byte[] { };
            }

            var buffer = new byte[hexadecimalString.Length / 2];

            for (int i = 0; i < hexadecimalString.Length; i += 2)
            {
                buffer[i / 2] = Convert.ToByte(hexadecimalString.Substring(i, 2), 0x10);
            }

            return buffer;
        }

        public static void PutHex(byte[] bytes, int offset, string hexString)
        {
            var sourceArray = ConvertHexStringToByteArray(hexString);

            Array.Copy(sourceArray, 0, bytes, offset, sourceArray.Length);
        }
    }
}
