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
        public void TestExcepciones()
        {
            var ex1 = new Exception("Excepci�n 1");
            var ex2 = new Exception("Excepci�n 2", ex1);
            var ex3 = new Exception("Excepci�n 3", ex2);

            Assert.Pass(ex3.GetInnerExceptionsConcatMessage());
        }
    }
}