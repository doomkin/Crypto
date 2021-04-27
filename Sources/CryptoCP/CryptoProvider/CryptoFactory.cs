using System;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using Crypto.CryptoProvider;
using Crypto.Digest;
using Crypto.Signer;
using Crypto.Stores;
using CryptoCP.Digest;

namespace CryptoCP.CryptoProvider
{
    public class CryptoFactory: ICryptoFactory
    {
        #region Implementation of ICryptoFactory

        public ICryptoDigest CreateDigest(DigestAlgorithm algorithm)
        {
            switch (algorithm)
            {
                case DigestAlgorithm.Gost94:
                    return new CryptoDigestGost3411();
                case DigestAlgorithm.Gost2012_256:
                    return new CryptoDigestGost3411_2012_256();
                case DigestAlgorithm.Gost2012_512:
                    return new CryptoDigestGost3411_2012_512();
            }

            throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Unsupported algorithm '{0}'.", algorithm.ToString()));
        }

        public ICryptoSigner CreateSigner(SignAlgorithm algorithm)
        {
            throw new System.NotImplementedException();
        }

        public ICertificatesStore CreateCertificatesStore(StoreName storeName, StoreLocation storeLocation)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
