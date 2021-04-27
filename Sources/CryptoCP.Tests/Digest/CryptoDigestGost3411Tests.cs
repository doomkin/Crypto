using System;
using Crypto.Digest;
using CryptoCP.CryptoProvider;
using NUnit.Framework;

namespace CryptoCP.Tests.Digest
{
    [TestFixture]
    public class CryptoDigestGost3411Tests
    {
        private static readonly CryptoFactory CryptoFactory = new CryptoFactory();
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

        [TestCase(new byte[0], new byte[] { 0x98, 0x1e, 0x5f, 0x3c, 0xa3, 0x0c, 0x84, 0x14, 0x87, 0x83, 0x0f, 0x84, 0xfb, 0x43, 0x3e, 0x13, 0xac, 0x11, 0x01, 0x56, 0x9b, 0x9c, 0x13, 0x58, 0x4a, 0xc4, 0x83, 0x23, 0x4c, 0xd6, 0x56, 0xc0 }, TestName = "Empty")]
        [TestCase(new byte[] { 0x74, 0x65, 0x73, 0x74 }, new byte[] { 0xee, 0x67, 0x30, 0x36, 0x96, 0xd2, 0x05, 0xdd, 0xd2, 0xb2, 0x36, 0x3e, 0x8e, 0x01, 0xb4, 0xb7, 0x19, 0x9a, 0x80, 0x95, 0x7d, 0x94, 0xd7, 0x67, 0x8e, 0xaa, 0xd3, 0xfc, 0x83, 0x4c, 0x5a, 0x27 }, TestName = "test")]
        public void DigestTest(byte[] data, byte[] expected)
        {
            var digest = _cryptoDigest.Digest(data);

            Assert.IsNotNull(digest);
            Assert.AreEqual(32, digest.Length);
            Assert.AreEqual(expected, digest);
        }
    }
}
