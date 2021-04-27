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
    public class CryptoSignerGost94Tests
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
        private static readonly string PrivateKeyPath = @"TestData.Certs.Cert94.key.pem";
        private static readonly string PublicKeyPath = @"TestData.Certs.Cert94.keypub.pem";
        private static readonly byte[] PrivateKey;
        private static readonly byte[] PublicKey;

        private ICryptoSigner _cryptoSigner;

        static CryptoSignerGost94Tests()
        {
            PrivateKey = ResourceLoader.ReadAllBytes(PrivateKeyPath);
            PublicKey = ResourceLoader.ReadAllBytes(PublicKeyPath);
        }

        [SetUp]
        public void Init()
        {
            _cryptoSigner = CryptoFactory.CreateSigner(SignAlgorithm.Gost94);
        }

        [Test]
        public void SignAndVerifyNullValTest()
        {
            Assert.That(() => _cryptoSigner.Sign(null, PrivateKey), Throws.Exception.TypeOf<ArgumentNullException>());
            Assert.That(() => _cryptoSigner.Sign(new byte[] { }, (byte[])null), Throws.Exception.TypeOf<ArgumentNullException>());
        }

        [Ignore("Gost94 signature was removed from gost engine. See https://github.com/gost-engine/engine/commit/02f99b2e3b46f4ff44fd5420487551d5a447c2ad")]
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
