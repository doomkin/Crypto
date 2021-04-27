using System.Security.Cryptography.X509Certificates;

namespace Crypto.Stores
{
    public interface ICertificatesStore
    {
        X509Certificate2 FindByTumbprint(string thumbprint);
    }
}
