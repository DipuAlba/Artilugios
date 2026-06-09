using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeDipuAlba.Artilugios.Tests
{
    public class StringMaskTests
    {
        private string input = "40770058698999513265";
        private char maskChar = 'X';
        private StringMask mask;

        [SetUp]
        public void Initiate()
        {
            mask = new StringMask(input, maskChar);
        }

        [Test]
        public void MaskShowLast()
        {
            var output = mask.ShowLast(10);
            Console.WriteLine(output);
            Assert.AreEqual("XXXXXXXXXX8999513265", output.ToString());
        }



        [Test]
        public void MaskInTheMiddle()
        {
            var output = mask.ShowLast(5).ShowFirst(5);
            Console.WriteLine(output);
            Assert.AreEqual("40770XXXXXXXXXX13265", output.ToString());
        }

        [Test]
        public void MaskInTheMiddleTooShort()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => mask.ShowLast(0).ShowFirst(0));
        }

        [Test]
        public void MaskInTheMiddleTooLong()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => mask.ShowLast(500).ShowFirst(500));
        }

        [Test]
        public void MaskAtTheEnd()
        {
            var output = mask.ShowFirst(10);
            Console.WriteLine(output);
            Assert.AreEqual("4077005869XXXXXXXXXX", output.ToString());

        }

        [Test]
        public void MaskSpanishNif_ValidPersonalNif_MasksOnlyDigitsFourToSeven()
        {
            var output = StringMask.MaskSpanishNif("12345678Z", 'X');
            Assert.AreEqual("XXX4567XX", output);
        }

        [Test]
        public void MaskSpanishNif_ValidNie_MasksOnlyDigitsFourToSeven()
        {
            var output = StringMask.MaskSpanishNif("X1234567L", 'X');
            Assert.AreEqual("XXXX4567X", output);
        }

        [Test]
        public void MaskSpanishNif_ValidLegalEntityNif_MasksOnlyDigitsFourToSeven()
        {
            var output = StringMask.MaskSpanishNif("P0200000H", 'X');
            Assert.AreEqual("XXXX0000X", output);
        }

        [Test]
        public void MaskSpanishNif_InvalidNif_MasksAllChars()
        {
            var output = StringMask.MaskSpanishNif("12345678A", 'X');
            Assert.AreEqual("XXX4567XX", output);
        }

        [Test]
        public void MaskSpanishNif_LowerCaseInput_NormalizesAndMasks()
        {
            var output = StringMask.MaskSpanishNif("x1234567l", 'X');
            Assert.AreEqual("XXXX4567X", output);
        }

        [Test]
        public void MaskSpanishNif_InvalidWithAtLeastSevenDigits_MasksAsOtherIdentification()
        {
            var output = StringMask.MaskSpanishNif("XY12345678AB", 'X');
            Assert.AreEqual("XXXXX4567XXX", output);
        }

        [Test]
        public void MaskSpanishNif_InvalidWithLessThanSevenDigits_ShowsLastFourCharacters()
        {
            var output = StringMask.MaskSpanishNif("ABCD123XY", 'X');
            Assert.AreEqual("XXXXX23XY", output);
        }
    }
}
