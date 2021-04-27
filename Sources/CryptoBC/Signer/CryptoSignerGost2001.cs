using Crypto.Digest;

namespace CryptoBC.Signer
{
    internal class CryptoSignerGost2001 : CryptoSignerBase
    {
        public CryptoSignerGost2001(ICryptoDigest cryptoDigest) : base("ECGOST3410", cryptoDigest)
        {
        }
    }
}
