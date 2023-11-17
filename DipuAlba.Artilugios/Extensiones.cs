using System;
using System.Collections.Generic;
using System.Linq;

namespace DipuAlba.Artilugios
{
    public static class Extensiones
    {
        /// <summary>
        /// Devuelve todas las excepciones internas de una excepción en una lista
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
        /// Devuelve los mensajes de las excepciones en una lista
        /// </summary>
        public static IEnumerable<string> GetInnerExceptionsMessages(this Exception ex)
        {
            return ex.GetInnerExceptions().Select(m =>  m.Message);
        }

        /// <summary>
        /// Devuelve los mensajes del as excepciones concatenados con un símbolo
        /// </summary>
        /// <param name="joinSymbol">Por defecto el símbolo => </param>
        public static string GetInnerExceptionsConcatMessage(this Exception ex, string joinSymbol = " => ")
        {
            return ex.GetInnerExceptionsMessages().Aggregate((c, n) => $"{c} {joinSymbol} {n}");
        }
    }
}
