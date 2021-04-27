using CryptoSSL.Configurator;

namespace CryptoSSL.Signer
{
    internal class CryptoSignerGost94 : CryptoSignerBase
    {
        public CryptoSignerGost94(OpenSslConfigurator configurator) : base("md_gost94", configurator)
        {
        }
    }
}
