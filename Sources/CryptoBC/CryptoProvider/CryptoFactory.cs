using System;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using Crypto.CryptoProvider;
using Crypto.Digest;
using Crypto.Signer;
using Crypto.Stores;
using CryptoBC.Digest;
using CryptoBC.Signer;

namespace CryptoBC.CryptoProvider
{
    public class CryptoFactory : ICryptoFactory
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
            switch (algorithm)
            {
                case SignAlgorithm.Gost94:
                    return new CryptoSignerGost94(CreateDigest(DigestAlgorithm.Gost94));
                case SignAlgorithm.Gost2001:
                    return new CryptoSignerGost2001(CreateDigest(DigestAlgorithm.Gost94));
                case SignAlgorithm.Gost2012_256:
                    return new CryptoSignerGost2012_256(CreateDigest(DigestAlgorithm.Gost2012_256));
                case SignAlgorithm.Gost2012_512:
                    return new CryptoSignerGost2012_512(CreateDigest(DigestAlgorithm.Gost2012_512));
            }

            throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Unsupported algorithm '{0}'.", algorithm.ToString()));
        }

        public ICertificatesStore CreateCertificatesStore(StoreName storeName, StoreLocation storeLocation)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
