﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace DipuAlba.Artilugios.Extensions
{
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
    }
}