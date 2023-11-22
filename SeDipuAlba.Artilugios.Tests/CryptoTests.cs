using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeDipuAlba.Artilugios.Tests
{
    internal class CryptoTests
    {
        [SetUp]
        public void Initiate()
        {
        }

        [Test]
        public void Basic()
        {
            const string original = "anything";
            var encrypted = SeDipuAlba.Artilugios.Crypto.EncryptStringAES(original, "secret", "saltatleast8bytes");
            var decrypted = SeDipuAlba.Artilugios.Crypto.DecryptStringAES(encrypted, "secret", "saltatleast8bytes");
            Assert.AreEqual(decrypted, original);
        }

    }
}
