using System;
using System.Collections.Generic;
using System.Text;

namespace SeDipuAlba.Artilugios.Extensions
{
    /// <summary>
    /// Provides extension methods for decimal type, particularly for currency formatting.
    /// </summary>
    public static class DecimalExtensions
    {
        private static readonly System.Globalization.CultureInfo SpanishCulture = new System.Globalization.CultureInfo("es-ES");

        /// <summary>
        /// Converts a decimal number to a string formatted as Euro currency in Spanish culture.
        /// </summary>
        /// <param name="num">The decimal number to format.</param>
        /// <param name="decimalNumbers">The number of decimal places to include in the formatted string. Default is 2.</param>
        /// <param name="roundDecimals">Determines if the number should be rounded to the nearest decimal. Default is true.</param>
        /// <param name="addEuroSymbol">Determines if the Euro symbol (€) should be appended to the formatted string. Default is false.</param>
        /// <returns>A string representation of the decimal number formatted as Euro currency.</returns>
        public static string EuroCurrencyString(this decimal num, int decimalNumbers = 2, bool roundDecimals = true, bool addEuroSymbol = false)
        {
            string formattedNumber;
            if (roundDecimals)
            {
                formattedNumber = num.ToString($"N{decimalNumbers}", SpanishCulture);
            }
            else
            {
                var multiplier = (decimal)Math.Pow(10, decimalNumbers);
                var truncated = decimal.Truncate(num * multiplier) / multiplier;
                formattedNumber = truncated.ToString($"N{decimalNumbers}", SpanishCulture);
            }

            return addEuroSymbol ? $"{formattedNumber} €" : formattedNumber;
        }

    }
}