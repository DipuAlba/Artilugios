using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SeDipuAlba.Artilugios
{
    /// <summary>
    /// Hides parts of a string with a specified character.
    /// Based on Assil code: https://stackoverflow.com/a/45056499/2126607
    /// </summary>
    public class StringMask
    {
        /// <summary>
        /// The Mask character
        /// </summary>
        private readonly char _maskCharacter;

        /// <summary>
        /// The instance
        /// </summary>
        private readonly string _instance;

        /// <summary>
        /// The Mask
        /// </summary>
        private readonly BitArray _mask;

        // Indicates if the mask has been updated
        private bool _isUpdated;

        // Caches the result of ToString
        private string _cachedToString;

        /// <summary>
        /// Initializes a new instance of the <see cref="StringMask"/> class.
        /// </summary>
        /// <param name="instance">The string you would like to mask.</param>
        /// <param name="maskCharacter">The Mask character.</param>
        public StringMask(string instance, char maskCharacter)
        {
            _maskCharacter = maskCharacter;
            _instance = instance ?? throw new ArgumentNullException(nameof(instance));
            _mask = new BitArray(instance.Length, false);
            _isUpdated = true; // Initially updated as there is no mask yet
        }

        /// <summary>
        /// Shows the first [number] of characters and masks the rest.
        /// </summary>
        /// <param name="number">The number of the characters to show.</param>
        /// <returns>IStringMask.</returns>
        public StringMask ShowFirst(int number)
        {
            Validate(number);
            SetMask(0, number, true);
            return this;
        }

        /// <summary>
        /// Shows the last [number] of characters and masks the rest.
        /// </summary>
        /// <param name="number">The number of the characters to show.</param>
        /// <returns>IStringMask.</returns>
        public StringMask ShowLast(int number)
        {
            Validate(number);
            SetMask(_instance.Length - number, _instance.Length, true);
            return this;
        }

        /// <summary>
        /// Masks a Spanish NIF/NIE value following data minimization criteria.
        /// </summary>
        /// <param name="nif">NIF/NIE to mask.</param>
        /// <param name="maskCharacter">Mask character.</param>
        /// <returns>Masked NIF/NIE.</returns>
        public static string MaskSpanishNif(string nif, char maskCharacter = '*')
        {
            if (string.IsNullOrWhiteSpace(nif))
            {
                throw new ArgumentException("NIF cannot be null or empty.", nameof(nif));
            }

            string normalizedNif = nif.Trim().ToUpperInvariant();
            var nifType = SpanishId.ValidateNif(normalizedNif);

            if (nifType == SpanishId.NifType.Invalid)
            {
                return CountDigits(normalizedNif) >= 7
                    ? MaskKeepingDigitsFourToSeven(normalizedNif, maskCharacter)
                    : MaskKeepingLastFourCharacters(normalizedNif, maskCharacter);
            }

            switch (nifType)
            {
                case SpanishId.NifType.PersonalNif:
                case SpanishId.NifType.Nie:
                case SpanishId.NifType.LegalEntityNif:
                    return MaskKeepingDigitsFourToSeven(normalizedNif, maskCharacter);
                default:
                    return new string(maskCharacter, normalizedNif.Length);
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            if (!_isUpdated) return _cachedToString;

            var sb = new StringBuilder();
            for (int i = 0; i < _instance.Length; i++)
            {
                sb.Append(_mask[i] ? _instance[i] : _maskCharacter);
            }

            _cachedToString = sb.ToString();
            _isUpdated = false; // Reset the update indicator
            return _cachedToString;
        }

        private void Validate(int number)
        {
            Guard.IsBetweenExclusive(number, 0, _instance.Length, nameof(number));
        }

        private static string MaskKeepingDigitsFourToSeven(string value, char maskCharacter)
        {
            var maskedChars = new char[value.Length];
            int digitPosition = 0;
            for (int i = 0; i < value.Length; i++)
            {
                char current = value[i];
                if (char.IsDigit(current))
                {
                    digitPosition++;
                    maskedChars[i] = (digitPosition >= 4 && digitPosition <= 7) ? current : maskCharacter;
                }
                else
                {
                    maskedChars[i] = maskCharacter;
                }
            }

            return new string(maskedChars);
        }

        private static string MaskKeepingLastFourCharacters(string value, char maskCharacter)
        {
            int unmaskedStart = Math.Max(0, value.Length - 4);
            var maskedChars = new char[value.Length];

            for (int i = 0; i < value.Length; i++)
            {
                maskedChars[i] = i >= unmaskedStart ? value[i] : maskCharacter;
            }

            return new string(maskedChars);
        }

        private static int CountDigits(string value)
        {
            int count = 0;
            for (int i = 0; i < value.Length; i++)
            {
                if (char.IsDigit(value[i]))
                {
                    count++;
                }
            }

            return count;
        }

        // Sets or unsets the mask for a range of characters
        private void SetMask(int start, int end, bool value)
        {
            for (int i = start; i < end; i++)
            {
                if (_mask[i] != value)
                {
                    _mask[i] = value;
                    _isUpdated = true; // Mark that the mask has been updated
                }
            }
        }
    }

}
