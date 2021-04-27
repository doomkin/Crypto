using CryptoSSL.Configurator;

namespace CryptoSSL.Signer
{
    internal class CryptoSignerGost2012_512 : CryptoSignerBase
    {
        public CryptoSignerGost2012_512(OpenSslConfigurator configurator) : base("md_gost12_512", configurator)
        {
        }
    }
}
