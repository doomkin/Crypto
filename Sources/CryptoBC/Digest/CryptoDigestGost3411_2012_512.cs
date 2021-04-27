using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;

namespace CryptoBC.Digest
{
    internal class CryptoDigestGost3411_2012_512 : CryptoDigestBase
    {
        public CryptoDigestGost3411_2012_512() : base("GOST3411-2012-512 ")
        {
        }

        #region Overrides of CryptoDigestBase

        protected override IDigest CreateDigest()
        {
            return new GOST3411_2012_512Digest();
        }

        #endregion
    }
}
