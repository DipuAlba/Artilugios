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
            var ex1 = new Exception("Excepción 1");
            var ex2 = new Exception("Excepción 2", ex1);
            var ex3 = new Exception("Excepción 3", ex2);

            Assert.Pass(ex3.GetInnerExceptionsConcatMessage());
        }
    }
}