using System.Security.Cryptography.X509Certificates;
using Crypto.Digest;
using Crypto.Signer;
using Crypto.Stores;

namespace Crypto.CryptoProvider
{
    public interface ICryptoFactory
    {
        ICryptoDigest CreateDigest(DigestAlgorithm algorithm);
        ICryptoSigner CreateSigner(SignAlgorithm algorithm);
        ICertificatesStore CreateCertificatesStore(StoreName storeName, StoreLocation storeLocation);
    }
}
