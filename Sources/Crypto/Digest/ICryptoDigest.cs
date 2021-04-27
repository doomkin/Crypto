namespace Crypto.Digest
{
    public interface ICryptoDigest
    {
        byte[] Digest(byte[] data);
    }
}
