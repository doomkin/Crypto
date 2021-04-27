using Crypto.Digest;

namespace CryptoBC.Signer
{
    internal class CryptoSignerGost2012_256 : CryptoSignerBase
    {
        public CryptoSignerGost2012_256(ICryptoDigest cryptoDigest) : base("ECGOST3410", cryptoDigest)
        {
        }
    }
}
