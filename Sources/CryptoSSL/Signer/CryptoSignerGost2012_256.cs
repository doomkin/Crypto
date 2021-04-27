using CryptoSSL.Configurator;

namespace CryptoSSL.Signer
{
    internal class CryptoSignerGost2012_256 : CryptoSignerBase
    {
        public CryptoSignerGost2012_256(OpenSslConfigurator configurator) : base("md_gost12_256", configurator)
        {
        }
    }
}
