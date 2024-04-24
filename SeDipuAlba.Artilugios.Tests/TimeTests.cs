using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeDipuAlba.Artilugios.Tests
{
    public class TimeTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Java()
        {
            var result = TimeConversion.JavaTimeStampToDateTime(1713782077851);//2024-04-22 12:34:37.851
            Assert.AreEqual(new DateTime(2024, 4, 22, 12, 34, 37, 851), result);
        }

        [Test]
        public void Unix()
        {
            var result = TimeConversion.UnixTimeStampToDateTime(1713953234);//2024-04-24 12:07:14
            Assert.AreEqual(new DateTime(2024, 4, 24, 12, 7, 14), result);
        }
    }
}
