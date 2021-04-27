using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Crypto.Digest;
using Crypto.Signer;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Signers;

namespace CryptoBC.Signer
{
    internal abstract class CryptoSignerBase : ICryptoSigner
    {
        private readonly ICryptoDigest _digest;
        private readonly IDsa _dsa;
        private readonly SecureRandom _random;

        protected CryptoSignerBase(string signerAlgorithm, ICryptoDigest cryptoDigest)
        {
            if (string.IsNullOrEmpty(signerAlgorithm))
            {
                throw new ArgumentNullException("signerAlgorithm");
            }

            if (cryptoDigest == null)
            {
                throw new ArgumentNullException("cryptoDigest");
            }

            _digest = cryptoDigest;
            _dsa = GetDsa(signerAlgorithm);
            _random = new SecureRandom();
        }

        #region Implementation of ICryptoSigner

        public byte[] Sign(byte[] data, X509Certificate2 certificate)
        {
            throw new NotImplementedException();
        }

        public byte[] Sign(byte[] data, byte[] key)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            return Sign(data, ExtractKeyFromPem(key));
        }


        public byte[] SignHash(byte[] hash, X509Certificate2 certificate)
        {
            throw new NotImplementedException();
        }

        public byte[] SignHash(byte[] hash, byte[] key)
        {
            return SignHash(hash, ExtractKeyFromPem(key));
        }

        public bool Verify(byte[] data, byte[] signature, X509Certificate2 certificate)
        {
            var cert = DotNetUtilities.FromX509Certificate(certificate);
            return Verify(data, signature, cert.GetPublicKey());
        }

        public bool Verify(byte[] data, byte[] signature, byte[] key)
        {
            return Verify(data, signature, ExtractKeyFromPem(key));
        }

        public bool VerifyHash(byte[] hash, byte[] signature, X509Certificate2 certificate)
        {
            throw new NotImplementedException();
        }

        public bool VerifyHash(byte[] hash, byte[] signature, byte[] key)
        {
            return VerifyHash(hash, signature, ExtractKeyFromPem(key));
        }

        #endregion

        private static IDsa GetDsa(string algorithm)
        {
            switch (algorithm)
            {
                case "GOST3410":
                    return new Gost3410Signer();
                case "ECGOST3410":
                    return new ECGost3410Signer();
                default:
                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Unsupported algorithm '{0}'.", algorithm));
            }
        }

        private static AsymmetricKeyParameter ExtractKeyFromPem(byte[] pemContent)
        {
            return ExtractKeyFromPem(Encoding.ASCII.GetString(pemContent));
        }

        private static AsymmetricKeyParameter ExtractKeyFromPem(string pemContent)
        {
            using (var keyPemTextReader = new StringReader(pemContent))
            {
                var keyPemReader = new PemReader(keyPemTextReader);
                var pemObject = keyPemReader.ReadObject();
                return pemObject as AsymmetricKeyParameter;
            }
        }

        private byte[] Sign(byte[] data, AsymmetricKeyParameter key)
        {
            var hash = _digest.Digest(data);
            return SignHash(hash, key);
        }

        private bool Verify(byte[] data, byte[] signature, AsymmetricKeyParameter key)
        {
            var hash = _digest.Digest(data);
            return VerifyHash(hash, signature, key);
        }

        private byte[] SignHash(byte[] hash, AsymmetricKeyParameter key)
        {
            _dsa.Init(true, new ParametersWithRandom(key, _random));
            var signature = _dsa.GenerateSignature(hash);
            var r = signature[0].ToByteArrayUnsigned();
            var s = signature[1].ToByteArrayUnsigned();
            int sigSize = s.Length + r.Length;
            var signatureBuffer = new byte[sigSize];
            s.CopyTo(signatureBuffer, 0);
            r.CopyTo(signatureBuffer, sigSize - r.Length);
            return signatureBuffer;
        }

        private bool VerifyHash(byte[] hash, byte[] signature, AsymmetricKeyParameter key)
        {
            _dsa.Init(false, key);
            int len = (signature.Length >> 1);
            var r = new BigInteger(1, signature, len, len);
            var s = new BigInteger(1, signature, 0, len);
            return _dsa.VerifySignature(hash, r, s);
        }
    }
}
