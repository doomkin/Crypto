using CryptoSSL.Configurator;

namespace CryptoSSL.Signer
{
    internal class CryptoSignerGost2001 : CryptoSignerBase
    {
        public CryptoSignerGost2001(OpenSslConfigurator configurator) : base("md_gost94", configurator)
        {
        }
    }
}
