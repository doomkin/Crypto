using System;
using System.Linq;

namespace CryptoSSL.Helpers
{
    public class Converter
    {
        public byte[] ToByteArray(string hexString)
        {
            if (hexString == null)
            {
                throw new ArgumentNullException("hexString");
            }

            return Enumerable.Range(0, hexString.Length).Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hexString.Substring(x, 2), 16)).ToArray();
        }
    }
}
