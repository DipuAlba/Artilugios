using System;
using System.Collections.Generic;
using System.Linq;

namespace DipuAlba.Artilugios.Extensions
{
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Returns all internal exceptions of an exception in a list
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public static IEnumerable<Exception> GetInnerExceptions(this Exception ex)
        {
            if (ex == null)
            {
                throw new ArgumentNullException(nameof(ex));
            }

            var innerException = ex;
            do
            {
                yield return innerException;
                innerException = innerException.InnerException;
            }
            while (innerException != null);
        }

        /// <summary>
        /// Returns exception messages in a list
        /// </summary>
        public static IEnumerable<string> GetInnerExceptionsMessages(this Exception ex)
        {
            return ex.GetInnerExceptions().Select(m => m.Message);
        }

        /// <summary>
        /// Returns exception messages concatenated with a symbol
        /// </summary>
        public static string GetInnerExceptionsConcatMessage(this Exception ex, string joinSymbol = " => ")
        {
            return ex.GetInnerExceptionsMessages().Aggregate((c, n) => $"{c} {joinSymbol} {n}");
        }


    }
}
