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
    public class CryptoDigestGost3411_2012_256Tests
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
            _cryptoDigest = CryptoFactory.CreateDigest(DigestAlgorithm.Gost2012_256);
        }

        [Test]
        public void DigestNullValTest()
        {
            Assert.That(() => _cryptoDigest.Digest(null), Throws.Exception.TypeOf<ArgumentNullException>());
        }

        [TestCase(new byte[0], new byte[] { 0x3f, 0x53, 0x9a, 0x21, 0x3e, 0x97, 0xc8, 0x02, 0xcc, 0x22, 0x9d, 0x47, 0x4c, 0x6a, 0xa3, 0x2a, 0x82, 0x5a, 0x36, 0x0b, 0x2a, 0x93, 0x3a, 0x94, 0x9f, 0xd9, 0x25, 0x20, 0x8d, 0x9c, 0xe1, 0xbb }, TestName = "Empty")]
        [TestCase(new byte[] { 0x74, 0x65, 0x73, 0x74 }, new byte[] { 0x12, 0xa5, 0x08, 0x38, 0x19, 0x1b, 0x55, 0x04, 0xf1, 0xe5, 0xf2, 0xfd, 0x07, 0x87, 0x14, 0xcf, 0x6b, 0x59, 0x2b, 0x9d, 0x29, 0xaf, 0x99, 0xd0, 0xb1, 0x0d, 0x8d, 0x02, 0x88, 0x1c, 0x38, 0x57 }, TestName = "test")]
        public void DigestTest(byte[] data, byte[] expected)
        {
            var digest = _cryptoDigest.Digest(data);

            Assert.IsNotNull(digest);
            Assert.AreEqual(32, digest.Length);
            Assert.AreEqual(expected, digest);
        }
    }
}
