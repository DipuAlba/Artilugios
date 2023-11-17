using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace DipuAlba.Artilugios
{
    /// <summary>
    /// Password generator for WsSegPass SEDIPUALB@
    /// https://pre.sedipualba.es/hashedpassword/
    /// </summary>
    public class HashedPassword
    {
        private readonly string _clearPassword;

        public HashedPassword(string clearPassword)
        {
            if (string.IsNullOrEmpty(clearPassword)) throw new ArgumentNullException(nameof(clearPassword));
            _clearPassword = clearPassword;
        }

        /// <summary>
        /// Generate password
        /// </summary>
        /// <param name="time">If time is null UtcNow will be used</param>
        public string Generate(DateTime? time = null)
        {
            // Obtener la hora actual UTC
            time = time ?? DateTime.UtcNow;
            var t = time.Value;

            // Formatear en una cadena de texto la hora UTC con el siguiente formato yyyyMMddHHmmss
            //  - yyyy: representa los cuatro dígitos del año.
            //  - MM: representa el mes usando dos dígitos(rellenando con 0 a la izquierda si es necesario).
            //  - dd: representa el día del mes usando dos dígitos(rellenando con 0 a la izquierda si es necesario).
            //  - HH: representa la hora en formato 24 horas usando dos dígitos(rellenando con 0 a la izquierda si es necesario).
            //  - mm: representa el minuto usando dos dígitos(rellenando con 0 a la izquierda si es necesario).
            //  - ss: representa el segundo usando dos dígitos(rellenando con 0 a la izquierda si es necesario).
            // Ojo: este timestamp lo usaremos dos veces: para calcular el hash y para obtener la cadena final.

            string timestampString = t.ToString("yyyyMMddHHmmss");

            // Concatenamos la cadena anterior con el timestamp y la contraseña en texto claro
            string hashInputString = timestampString + _clearPassword;

            // Llamamos al método de obtener el hash de 256
            string hashBase64 = GetSha256Hash(hashInputString);

            // Por último volvemos a concatenar el timestamp y el hash en Base64
            return timestampString + hashBase64;
        }

        private static string GetSha256Hash(string input)
        {
            using (var hashAlgorithm = SHA256.Create())
            {
                // Codificamos el resultado en UTF8
                var byteValue = Encoding.UTF8.GetBytes(input);
                // Calculamos el hash SHA256 de lo anterior
                var byteHash = hashAlgorithm.ComputeHash(byteValue);
                // Codificamos el hash en Base64 para obtener una cadena de texto
                return Convert.ToBase64String(byteHash);
            }
        }
    }
}
