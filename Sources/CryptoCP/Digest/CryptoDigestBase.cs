using System;
using System.Globalization;
using System.Security.Cryptography;
using Crypto.Digest;

namespace CryptoCP.Digest
{
    internal abstract class CryptoDigestBase : ICryptoDigest
    {
        private readonly string _algorithm;

        protected CryptoDigestBase(string algorithm)
        {
            if (string.IsNullOrEmpty(algorithm))
            {
                throw new ArgumentNullException("algorithm");
            }

            _algorithm = algorithm;
        }

        #region Implementation of ICryptoDigest

        public byte[] Digest(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            var hashAlgorithm = HashAlgorithm.Create(_algorithm);
            if (hashAlgorithm == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Unsupported digest algorithm '{0}'.", _algorithm));
            }

            return hashAlgorithm.ComputeHash(data);
        }

        #endregion
    }
}
