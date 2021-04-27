using System;
using System.IO;
using Crypto.Digest;
using CryptoSSL.Configurator;
using CryptoSSL.CryptoProvider;
using CryptoSSL.Tests.Helpers;
using NUnit.Framework;

namespace CryptoSSL.Tests.Digest
{
    [TestFixture]
    public class CryptoDigestGost3411Tests
    {
        private static readonly CryptoFactory CryptoFactory = new CryptoFactory
            {
                Configurator = new OpenSslConfigurator
                    {
                        OpenSslCommand = TestHelper.GetTestFullDataPath(@"..\..\..\..\Additional\OpenSsl\Output\x64\bin\openssl.cmd"),
                        TempPath = Path.GetTempPath()
                    }
            };
        private ICryptoDigest _cryptoDigest;

        [SetUp]
        public void Init()
        {
            _cryptoDigest = CryptoFactory.CreateDigest(DigestAlgorithm.Gost94);
        }

        [Test]
        public void DigestNullValTest()
        {
            Assert.That(() => _cryptoDigest.Digest(null), Throws.Exception.TypeOf<ArgumentNullException>());
        }

        [TestCase(new byte[0], new byte[] { 0x3f, 0x25, 0xbc, 0x1f, 0xbb, 0xce, 0x27, 0xca, 0x10, 0xfb, 0x19, 0x58, 0xf3, 0x19, 0x47, 0x3a, 0xe7, 0xe1, 0x74, 0x82, 0xc3, 0xb5, 0x3e, 0xcf, 0x47, 0xa7, 0xe2, 0xde, 0x8a, 0xab, 0xe4, 0xc8 }, TestName = "Empty")]
        [TestCase(new byte[] { 0x74, 0x65, 0x73, 0x74 }, new byte[] { 0xee, 0x67, 0x30, 0x36, 0x96, 0xd2, 0x05, 0xdd, 0xd2, 0xb2, 0x36, 0x3e, 0x8e, 0x01, 0xb4, 0xb7, 0x19, 0x9a, 0x80, 0x95, 0x7d, 0x94, 0xd7, 0x67, 0x8e, 0xaa, 0xd3, 0xfc, 0x83, 0x4c, 0x5a, 0x27 }, TestName = "test")]
        public void DigestTest(byte[] data, byte[] expected)
        {
            var hash = _cryptoDigest.Digest(data);

            Assert.IsNotNull(hash);
            Assert.AreEqual(32, hash.Length);
            Assert.AreEqual(expected, hash);
        }
    }
}
