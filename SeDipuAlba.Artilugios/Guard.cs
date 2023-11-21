using System;
using System.Collections.Generic;
using System.Text;

namespace SeDipuAlba.Artilugios
{
    /// <summary>
    /// Validation class
    /// </summary>
    public static class Guard
    {
        /// <summary>
        /// Ensures that the value of a parameter is between exclusive bounds.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="min">The exclusive minimum bound.</param>
        /// <param name="max">The exclusive maximum bound.</param>
        /// <param name="paramName">The name of the parameter being checked.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the value is outside the exclusive bounds.</exception>
        public static void IsBetweenExclusive(int value, int min, int max, string paramName)
        {
            if (value <= min || value >= max)
            {
                throw new ArgumentOutOfRangeException(paramName, $"The value of {paramName} ({value}) must be between {min} and {max} exclusive.");
            }
        }
    }

}
