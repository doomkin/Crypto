using System.Security.Cryptography.X509Certificates;

namespace Crypto.Signer
{
    public interface ICryptoSigner
    {
        byte[] Sign(byte[] data, X509Certificate2 certificate);
        byte[] Sign(byte[] data, byte[] key);
        byte[] SignHash(byte[] hash, X509Certificate2 certificate);
        byte[] SignHash(byte[] hash, byte[] key);
        bool Verify(byte[] data, byte[] signature, X509Certificate2 certificate);
        bool Verify(byte[] data, byte[] signature, byte[] key);
        bool VerifyHash(byte[] hash, byte[] signature, X509Certificate2 certificate);
        bool VerifyHash(byte[] hash, byte[] signature, byte[] key);
    }
}
