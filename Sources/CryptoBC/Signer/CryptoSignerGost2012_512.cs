using Crypto.Digest;

namespace CryptoBC.Signer
{
    internal class CryptoSignerGost2012_512 : CryptoSignerBase
    {
        public CryptoSignerGost2012_512(ICryptoDigest cryptoDigest) : base("ECGOST3410", cryptoDigest)
        {
        }
    }
}
