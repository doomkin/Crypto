using System;
using System.IO;
using System.Reflection;
using Crypto.Signer;
using CryptoSSL.Configurator;
using CryptoSSL.Tests.Helpers;
using NUnit.Framework;

namespace CryptoSSL.Tests.Signer
{
    [TestFixture]
    public class CryptoSignerGost2012_512Tests
    {
        private static readonly ResourceLoader ResourceLoader = new ResourceLoader(Assembly.GetExecutingAssembly());
        private static readonly CryptoBC.CryptoProvider.CryptoFactory CryptoFactoryBC = new CryptoBC.CryptoProvider.CryptoFactory();
        private static readonly CryptoSSL.CryptoProvider.CryptoFactory CryptoFactorySSL = new CryptoSSL.CryptoProvider.CryptoFactory()
        {
            Configurator = new OpenSslConfigurator
                    {
                        OpenSslCommand = TestHelper.GetTestFullDataPath(@"..\..\..\..\Additional\OpenSsl\Output\x64\bin\openssl.cmd"),
                        TempPath = Path.GetTempPath()
                    }
            };
        private static readonly string PrivateKeyPath = @"TestData.Certs.Cert2012_512.key.pem";
        private static readonly string PublicKeyPath = @"TestData.Certs.Cert2012_512.keypub.pem";
        private static readonly byte[] PrivateKey;
        private static readonly byte[] PublicKey;

        private ICryptoSigner _cryptoSignerBC;
        private ICryptoSigner _cryptoSignerSSL;

        static CryptoSignerGost2012_512Tests()
        {
            PrivateKey = ResourceLoader.ReadAllBytes(PrivateKeyPath);
            PublicKey = ResourceLoader.ReadAllBytes(PublicKeyPath);
        }

        [SetUp]
        public void Init()
        {
            _cryptoSignerBC = CryptoFactoryBC.CreateSigner(SignAlgorithm.Gost2012_512);
            _cryptoSignerSSL = CryptoFactorySSL.CreateSigner(SignAlgorithm.Gost2012_512);
        }

        [Test]
        public void SignAndVerifyNullValTest()
        {
            Assert.That(() => _cryptoSignerSSL.Sign(null, PrivateKey), Throws.Exception.TypeOf<ArgumentNullException>());
            Assert.That(() => _cryptoSignerSSL.Sign(new byte[] { }, (byte[])null), Throws.Exception.TypeOf<ArgumentNullException>());
        }

        [TestCase(new byte[] { }, TestName = "Empty")]
        [TestCase(new byte[] { 0x74, 0x65, 0x73, 0x74 }, TestName = "test")]
        public void SignAndVerifyTest(byte[] data)
        {
            var sign = _cryptoSignerSSL.Sign(data, PrivateKey);

            Assert.IsNotNull(sign);
            Assert.IsTrue(_cryptoSignerSSL.Verify(data, sign, PublicKey));
        }

        [TestCase(new byte[] { }, TestName = "Empty")]
        [TestCase(new byte[] { 0x74, 0x65, 0x73, 0x74 }, TestName = "test")]
        public void SignSSLAndVerifyBCTest(byte[] data)
        {
            var sign = _cryptoSignerSSL.Sign(data, PrivateKey);

            Assert.IsNotNull(sign);
            Assert.IsTrue(_cryptoSignerBC.Verify(data, sign, PublicKey));
        }

        [TestCase(new byte[] { }, TestName = "Empty")]
        [TestCase(new byte[] { 0x74, 0x65, 0x73, 0x74 }, TestName = "test")]
        public void SignBCAndVerifySSLTest(byte[] data)
        {
            var sign = _cryptoSignerBC.Sign(data, PrivateKey);

            Assert.IsNotNull(sign);
            Assert.IsTrue(_cryptoSignerSSL.Verify(data, sign, PublicKey));
        }
    }
}
