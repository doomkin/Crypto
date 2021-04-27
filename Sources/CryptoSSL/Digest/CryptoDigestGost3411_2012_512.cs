using CryptoSSL.Configurator;

namespace CryptoSSL.Digest
{
    internal class CryptoDigestGost3411_2012_512 : CryptoDigestBase
    {
        public CryptoDigestGost3411_2012_512(OpenSslConfigurator configurator) : base("md_gost12_512", configurator)
        {
        }
    }
}
