using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SeDipuAlba.Artilugios
{
    public class SpanishId
    {
        // Regex constants for legal entity NIF validation
        private static readonly Regex InvalidCharsRegex = new Regex(@"([^A-W0-9]|[OT]|[^\w])", RegexOptions.Compiled);
        private static readonly Regex LegalEntityFormatRegex = new Regex(@"^[A-HJ-W]\d{7}$", RegexOptions.Compiled);

        // Regex constants for personal NIF/NIE validation
        private static readonly Regex PersonalNifNieFormatRegex = new Regex(@"(^\d{8}$)|(^[K-MX-Z]\d{7}$)", RegexOptions.Compiled);
        private static readonly Regex ExtractNumbersRegex = new Regex(@"(\d{7,8}$)", RegexOptions.Compiled);

        /// <summary>
        /// Checks if the NIF/NIE is valid and returns the type: personal NIF, legal entity NIF, or NIE.
        /// </summary>
        /// <param name="nif">The NIF should be provided in uppercase, otherwise it will not be considered valid.</param>
        /// <returns></returns>
        public static NifType ValidateNif(string nif)
        {
            if (string.IsNullOrWhiteSpace(nif)) return NifType.Invalid;
            if (nif.Length != 9) return NifType.Invalid;

            char controlDigit = nif[8];
            if (controlDigit == 0) return NifType.Invalid;

            string nifWithoutControlDigit = nif.Substring(0, 8);

            if (controlDigit == GetControlDigitForPersonalNifNie(nifWithoutControlDigit, out var isNie))
            {
                return isNie ? NifType.Nie : NifType.PersonalNif;
            }
            else if (controlDigit == GetControlDigitForLegalEntityNif(nifWithoutControlDigit, controlDigit))
            {
                return NifType.LegalEntityNif;
            }
            else
            {
                return NifType.Invalid;
            }
        }

        /// <summary>
        /// Gets the control digit for personal NIF or NIE.
        /// </summary>
        /// <param name="nifWithoutControlDigit">The NIF without the control digit.</param>
        /// <param name="isNie">Indicates if the value corresponds to a NIE.</param>
        /// <returns>The control digit as a character.</returns>
        private static char GetControlDigitForPersonalNifNie(string nifWithoutControlDigit, out bool isNie)
        {
            isNie = false;

            // The NIF is composed of eight numbers followed by a letter.
            // 
            // The NIF of other categories of individuals is composed of
            // a letter (K, L, M), followed by 7 numbers and a final letter.
            // 
            // The NIE is composed of an initial letter (X, Y, Z),
            // followed by 7 numbers and a final letter.
            try
            {
                if (!PersonalNifNieFormatRegex.IsMatch(nifWithoutControlDigit))
                    return '\0';

                // Extract only the numbers from the NIF.
                string numbers = ExtractNumbersRegex.Match(nifWithoutControlDigit).Value;

                // First character of the NIF.
                char firstChar = nifWithoutControlDigit[0];

                // If necessary, replace the NIE letter with the corresponding value.
                if (firstChar == 'X')
                {
                    numbers = "0" + numbers;
                    isNie = true;
                }
                else if (firstChar == 'Y')
                {
                    numbers = "1" + numbers;
                    isNie = true;
                }
                else if (firstChar == 'Z')
                {
                    numbers = "2" + numbers;
                    isNie = true;
                }
                else if (firstChar == 'M')
                {
                    isNie = true;
                }

                // NIF table
                // 
                // 0T  1R  2W  3A  4G  5M  6Y  7F  8P  9D
                // 10X 11B 12N 13J 14Z 15S 16Q 17V 18H 19L
                // 20C 21K 22E 23T
                // 
                // Proceed to calculate the NIF/NIE.
                int dni = Convert.ToInt32(numbers);

                // The operation consists of calculating the remainder when dividing the DNI
                // by 23 (without decimals). This remainder (which will be between 0 and 22)
                // is used to find the letter of the NIF from the table.
                int remainder = dni % 23;

                // Get the control digit of the NIF.
                char controlDigit = "TRWAGMYFPDXBNJZSQVHLCKE"[remainder];

                return controlDigit;
            }
            catch
            {
                return default;
            }
        }

        /// <summary>
        /// Gets the control digit for a legal entity NIF.
        /// </summary>
        /// <param name="nifWithoutControlDigit">The NIF without the control digit.</param>
        /// <param name="lastChar">The last character of the NIF.</param>
        /// <returns>The control digit as a character.</returns>
        private static char GetControlDigitForLegalEntityNif(string nifWithoutControlDigit, char lastChar)
        {
            nifWithoutControlDigit = InvalidCharsRegex.Replace(nifWithoutControlDigit, string.Empty).ToUpper();

            if (!LegalEntityFormatRegex.IsMatch(nifWithoutControlDigit))
                return '\0';

            try
            {
                int sumEven = 0;
                int sumOdd = 0;

                // The string must contain 7 digits.
                string digits = nifWithoutControlDigit.Substring(1, 7);

                for (int n = 0; n <= digits.Length - 1; n += 2)
                {
                    if (n < 6)
                    {
                        // Sum the even digits (positions 1, 3, 5 in the variable "digits").
                        sumOdd += Convert.ToInt32(digits[n + 1].ToString());
                    }
                    // Multiply each odd digit (positions 0, 2, 4, 6) by two.
                    int doubleOdd = 2 * Convert.ToInt32(digits[n].ToString());

                    // Accumulate the sum of the doubled odd numbers.
                    sumEven += (doubleOdd % 10) + (doubleOdd / 10);
                }

                // Sum the even and odd digits.
                int totalSum = sumEven + sumOdd;

                // Get the units digit and subtract it from 10, if it is not zero.
                totalSum = (10 - (totalSum % 10)) % 10;

                char[] characters = new[] { 'J', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I' };
                char controlDigit = characters[totalSum];

                // Return the control digit based on the first character of the NIF.
                char firstChar = nifWithoutControlDigit[0];

                switch (firstChar)
                {
                    case 'N':
                    case 'P':
                    case 'Q':
                    case 'R':
                    case 'S':
                    case 'W':
                    case 'K':
                    case 'L':
                    case 'M':
                        // Corresponds to a letter.
                        return controlDigit;
                    case 'C':
                        if (lastChar == totalSum.ToString()[0] || lastChar == controlDigit)
                        {
                            // Return the given control digit, which can be a number or a letter.
                            return lastChar;
                        }
                        else
                        {
                            // Return the calculated letter for the control digit of the NIF.
                            return controlDigit;
                        }
                    default:
                        // Corresponds to a number.
                        return Convert.ToChar(totalSum.ToString());
                }
            }
            catch
            {
                return '\0';
            }
        }

        public enum NifType
        {
            PersonalNif,
            Nie,
            LegalEntityNif,
            Invalid
        }
    }
}
