using System;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using Crypto.Digest;

namespace CryptoBC.Digest
{
    internal abstract class CryptoDigestBase : ICryptoDigest
    {
        private readonly string _digestAlgorithm;
        private IDigest _digest;

        protected CryptoDigestBase(string digestAlgorithm)
        {
            if (string.IsNullOrEmpty(digestAlgorithm))
            {
                throw new ArgumentNullException("digestAlgorithm");
            }

            _digestAlgorithm = digestAlgorithm;
        }

        #region Implementation of ICryptoSigner

        public byte[] Digest(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            var digest = InternalDigest;
            var hash = new byte[digest.GetDigestSize()];
            digest.BlockUpdate(data, 0, data.Length);
            digest.DoFinal(hash, 0);
            return hash;
        }

        #endregion

        private IDigest InternalDigest
        {
            get
            {
                return _digest ?? (_digest = CreateDigest());
            }
        }

        protected virtual IDigest CreateDigest()
        {
            return DigestUtilities.GetDigest(_digestAlgorithm);
        }
    }
}
