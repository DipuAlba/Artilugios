using System;
using SeDipuAlba.Artilugios.Extensions;
using NUnit.Framework;

namespace SeDipuAlba.Artilugios.Tests
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
            Assert.AreEqual("20300101000000f9XoDw0SE1Wcag37Pvph0FdSNX1zL4g7M+M9unurRQY=", new SeDipuAlba.Artilugios.HashedPassword("1234567890").Generate(new DateTime(2030, 1, 1)));
        }

        [Test]
        public void RemoveDiacritics()
        {
            Assert.AreEqual("En Espana esta un pinguino", "En Espa�a est� un ping�ino".RemoveDiacritics());
        }

        [Test]
        public void AddParameter()
        {
            var uri = new Uri("http://www.sedipualba.es");
            uri = uri.AddParameter("id", "1");
            Assert.AreEqual("http://www.sedipualba.es/?id=1", uri.ToString());
            uri = uri.AddParameter("hello", "world");
            Assert.AreEqual("http://www.sedipualba.es/?id=1&hello=world", uri.ToString());
            uri = uri.AddParameter("urlReturn", "http://espa�a.a�na.io/?anotherParam=murci�lago");
            Assert.AreEqual("http://www.sedipualba.es/?id=1&hello=world&urlReturn=http%253a%252f%252fespa%25c3%25b1a.a%25c3%25bdna.io%252f%253fanotherParam%253dmurci%25c3%25a9lago", uri.ToString());
        }

        [Test]
        public void EuroCurrencyText()
        {
            decimal number = 0.015M;
            Assert.AreEqual("0,02", number.EuroCurrencyString());
            Assert.AreEqual("0,01", number.EuroCurrencyString(roundDecimals: false));
            Assert.AreEqual("0,01 �", number.EuroCurrencyString(roundDecimals: false, addEuroSymbol:true));
            number = 123456789;
            Assert.AreEqual("123.456.789,00", number.EuroCurrencyString());
            Assert.AreEqual("123.456.789", number.EuroCurrencyString(roundDecimals: false, decimalNumbers:0));
            Assert.AreEqual("123.456.789 �", number.EuroCurrencyString(roundDecimals: false, decimalNumbers:0, addEuroSymbol:true));
            Assert.AreEqual("123.456.789,00 �", number.EuroCurrencyString(roundDecimals: false, addEuroSymbol:true));
            number = 123456789.987654321M;
            Assert.AreEqual("123.456.789,9877", number.EuroCurrencyString(decimalNumbers:4));
            Assert.AreEqual("123.456.789,9876", number.EuroCurrencyString(decimalNumbers:4, roundDecimals: false));
            Assert.AreEqual("123.456.789,9876 �", number.EuroCurrencyString(decimalNumbers:4, roundDecimals: false, addEuroSymbol:true));

        }
    }
}