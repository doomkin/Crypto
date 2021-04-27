using CryptoSSL.Configurator;

namespace CryptoSSL.Digest
{
    internal class CryptoDigestGost3411 : CryptoDigestBase
    {
        public CryptoDigestGost3411(OpenSslConfigurator configurator) : base("md_gost94", configurator)
        {
        }
    }
}
