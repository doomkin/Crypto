using CryptoSSL.Configurator;

namespace CryptoSSL.Digest
{
    internal class CryptoDigestGost3411_2012_256 : CryptoDigestBase
    {
        public CryptoDigestGost3411_2012_256(OpenSslConfigurator configurator) : base("md_gost12_256", configurator)
        {
        }
    }
}
