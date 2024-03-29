﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SeDipuAlba.Artilugios.Extensions
{
    /// <summary>
    /// Extensions for handling string texts
    /// </summary>
    public static class TextExtensions
    {
        /// <summary>
        /// Removes diacritics from texts. Example: "En España está un pingüino" becomes "En Espana esta un pinguino"
        /// </summary>
        public static string RemoveDiacritics(this string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder(capacity: normalizedString.Length);

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder
                .ToString()
                .Normalize(NormalizationForm.FormC);
        }

        /// <summary>
        /// Remove all characters from the string that are not specified in alphabet.
        /// </summary>
        public static string RemoveSymbols(this string txt, string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789 ")
        {
            if (txt == null) throw new ArgumentNullException(nameof(txt));
            if (alphabet == null) throw new ArgumentNullException(nameof(alphabet));

            var allowedChars = new HashSet<char>(alphabet);
            var result = new StringBuilder();

            foreach (var c in txt.Where(c => allowedChars.Contains(c)))
            {
                result.Append(c);
            }

            return result.ToString();
        }

        /// <summary>
        /// Truncates a string to a specified maximum length and appends a truncation suffix if necessary.
        /// </summary>
        /// <param name="value">The string to truncate.</param>
        /// <param name="maxLength">The maximum length of the string.</param>
        /// <param name="truncationSuffix">The suffix to append when truncation occurs (it won't make the result longer).</param>
        /// <returns>The truncated string with the truncation suffix if truncation occurred.</returns>
        public static string Truncate(this string value, int maxLength, string truncationSuffix = "…")
        {
            if (value?.Length <= maxLength)
            {
                return value;
            }

            int suffixLength = truncationSuffix?.Length ?? 0;
            maxLength -= suffixLength;
            return value.Substring(0, maxLength) + truncationSuffix;
        }
    }
}
