using Crypto.Digest;

namespace CryptoBC.Signer
{
    internal class CryptoSignerGost94 : CryptoSignerBase
    {
        public CryptoSignerGost94(ICryptoDigest cryptoDigest) : base("GOST3410", cryptoDigest)
        {
        }
    }
}
