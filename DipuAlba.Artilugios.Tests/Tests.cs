using System;
using DipuAlba.Artilugios.Extensions;
using NUnit.Framework;

namespace DipuAlba.Artilugios.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void InnerExceptions()
        {
            var ex1 = new Exception("Excepci�n 1");
            var ex2 = new Exception("Excepci�n 2", ex1);
            var ex3 = new Exception("Excepci�n 3", ex2);

            Assert.AreEqual("Excepci�n 3 => Excepci�n 2 => Excepci�n 1", ex3.GetInnerExceptionsConcatMessage());
        }

        [Test]
        public void HashedPassword()
        {
            Assert.AreEqual("20300101000000f9XoDw0SE1Wcag37Pvph0FdSNX1zL4g7M+M9unurRQY=", new DipuAlba.Artilugios.HashedPassword("1234567890").Generate(new DateTime(2030, 1, 1)));
        }

        [Test]
        public void RemoveDiacritics()
        {
            Assert.AreEqual("En Espana esta un pinguino", "En Espa�a est� un ping�ino".RemoveDiacritics());
        }
    }
}