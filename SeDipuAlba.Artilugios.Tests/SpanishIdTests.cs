using NUnit.Framework;
using System;

namespace SeDipuAlba.Artilugios.Tests
{
    public class SpanishIdTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ValidateNif_ValidPersonalNif_ReturnsPersonalNif()
        {
            string validPersonalNif = "12345678Z";
            var result = SpanishId.ValidateNif(validPersonalNif);
            Assert.AreEqual(SpanishId.NifType.PersonalNif, result);
        }

        [Test]
        public void ValidateNif_ValidNie_ReturnsNie()
        {
            string validNie = "X1234567L";
            var result = SpanishId.ValidateNif(validNie);
            Assert.AreEqual(SpanishId.NifType.Nie, result);
        }

        [Test]
        public void ValidateNif_ValidLegalEntityNif_ReturnsLegalEntityNif()
        {
            string validLegalEntityNif = "P0200000H";
            var result = SpanishId.ValidateNif(validLegalEntityNif);
            Assert.AreEqual(SpanishId.NifType.LegalEntityNif, result);
        }

        [Test]
        public void ValidateNif_ValidK_ReturnsPersonalNif()
        {
            string validK = "K1234567L";
            var result = SpanishId.ValidateNif(validK);
            Assert.AreEqual(SpanishId.NifType.PersonalNif, result);
        }

        [Test]
        public void ValidateNif_ValidL_ReturnsPersonalNif()
        {
            string validL = "L1234567L";
            var result = SpanishId.ValidateNif(validL);
            Assert.AreEqual(SpanishId.NifType.PersonalNif, result);
        }

        [Test]
        public void ValidateNif_ValidM_ReturnsPersonalNif()
        {
            string validM = "M1234567L";
            var result = SpanishId.ValidateNif(validM);
            Assert.AreEqual(SpanishId.NifType.Nie, result);
        }

        [Test]
        public void ValidateNif_InvalidLength_ReturnsInvalid()
        {
            string invalidNif = "1234567";
            var result = SpanishId.ValidateNif(invalidNif);
            Assert.AreEqual(SpanishId.NifType.Invalid, result);
        }

        [Test]
        public void ValidateNif_InvalidControlDigit_ReturnsInvalid()
        {
            string invalidNif = "12345678A";
            var result = SpanishId.ValidateNif(invalidNif);
            Assert.AreEqual(SpanishId.NifType.Invalid, result);
        }

        [Test]
        public void ValidateNif_LowerCaseControlDigit_ReturnsInvalid()
        {
            string invalidNif = "12345678z";
            var result = SpanishId.ValidateNif(invalidNif);
            Assert.AreEqual(SpanishId.NifType.Invalid, result);
        }

        [Test]
        public void ValidateNif_NullInput_ReturnsInvalid()
        {
            string nullNif = null;
            var result = SpanishId.ValidateNif(nullNif);
            Assert.AreEqual(SpanishId.NifType.Invalid, result);
        }

        [Test]
        public void ValidateNif_EmptyInput_ReturnsInvalid()
        {
            string emptyNif = "";
            var result = SpanishId.ValidateNif(emptyNif);
            Assert.AreEqual(SpanishId.NifType.Invalid, result);
        }

    }
}
