using System;
using System.IO;
using System.Reflection;
using Crypto.Signer;
using CryptoSSL.Configurator;
using CryptoSSL.CryptoProvider;
using CryptoSSL.Tests.Helpers;
using NUnit.Framework;

namespace CryptoSSL.Tests.Signer
{
    [TestFixture]
    public class CryptoSignerGost2012_256Tests
    {
        private static readonly ResourceLoader ResourceLoader = new ResourceLoader(Assembly.GetExecutingAssembly());
        private static readonly CryptoFactory CryptoFactory = new CryptoFactory
            {
                Configurator = new OpenSslConfigurator
                    {
                        OpenSslCommand = TestHelper.GetTestFullDataPath(@"..\..\..\..\Additional\OpenSsl\Output\x64\bin\openssl.cmd"),
                        TempPath = Path.GetTempPath()
                    }
            };
        private static readonly string PrivateKeyPath = @"TestData.Certs.Cert2012_256.key.pem";
        private static readonly string PublicKeyPath = @"TestData.Certs.Cert2012_256.keypub.pem";
        private static readonly byte[] PrivateKey;
        private static readonly byte[] PublicKey;

        private ICryptoSigner _cryptoSigner;

        static CryptoSignerGost2012_256Tests()
        {
            PrivateKey = ResourceLoader.ReadAllBytes(PrivateKeyPath);
            PublicKey = ResourceLoader.ReadAllBytes(PublicKeyPath);
        }

        [SetUp]
        public void Init()
        {
            _cryptoSigner = CryptoFactory.CreateSigner(SignAlgorithm.Gost2012_256);
        }

        [Test]
        public void SignAndVerifyNullValTest()
        {
            Assert.That(() => _cryptoSigner.Sign(null, PrivateKey), Throws.Exception.TypeOf<ArgumentNullException>());
            Assert.That(() => _cryptoSigner.Sign(new byte[] { }, (byte[])null), Throws.Exception.TypeOf<ArgumentNullException>());
        }

        [TestCase(new byte[] { }, TestName = "Empty")]
        [TestCase(new byte[] { 0x74, 0x65, 0x73, 0x74 }, TestName = "test")]
        public void SignAndVerifyTest(byte[] data)
        {
            var sign = _cryptoSigner.Sign(data, PrivateKey);

            Assert.IsNotNull(sign);
            Assert.IsTrue(_cryptoSigner.Verify(data, sign, PublicKey));
        }
    }
}
