using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SeDipuAlba.Artilugios
{
    /// <summary>
    /// Crypto library to encrypt/decrypt string texts
    /// Based on Brett code: https://stackoverflow.com/a/2791259/2126607
    /// </summary>
    public class Crypto
    {
        /// <summary>
        /// Encrypts a string using AES with a shared secret. The encrypted string can be decrypted using DecryptStringAES().
        /// </summary>
        /// <param name="plainText">Text to encrypt.</param>
        /// <param name="sharedSecret">Password for key generation.</param>
        /// <param name="salt">Salt for key generation.</param>
        /// <returns>Encrypted string in Base64 format.</returns>
        public static string EncryptStringAES(string plainText, string sharedSecret, string salt)
        {
            return EncryptStringAES(plainText, sharedSecret, GenerateSaltFromString(salt));
        }

        /// <summary>
        /// Encrypts a string using AES with a shared secret. The encrypted string can be decrypted using DecryptStringAES().
        /// </summary>
        /// <param name="plainText">Text to encrypt.</param>
        /// <param name="sharedSecret">Password for key generation.</param>
        /// <param name="salt">Salt for key generation.</param>
        /// <returns>Encrypted string in Base64 format.</returns>
        public static string EncryptStringAES(string plainText, string sharedSecret, byte[] salt)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentNullException(nameof(plainText));
            if (string.IsNullOrEmpty(sharedSecret))
                throw new ArgumentNullException(nameof(sharedSecret));

            RijndaelManaged aesAlg = null; // AES algorithm instance

            try
            {
                // Key generation from shared secret and salt
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, salt);

                aesAlg = new RijndaelManaged();
                aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);

                // Encryptor creation
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Encryption stream setup
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    // Prepend IV for decryption
                    msEncrypt.Write(BitConverter.GetBytes(aesAlg.IV.Length), 0, sizeof(int));
                    msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);

                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }

                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
            finally
            {
                aesAlg?.Clear(); // Ensure the AES algorithm is cleared from memory
            }
        }

        /// <summary>
        /// Decrypts a string that was encrypted using EncryptStringAES() and the same shared secret.
        /// </summary>
        /// <param name="cipherText">Encrypted text in Base64 format.</param>
        /// <param name="sharedSecret">Password for key generation.</param>
        /// <param name="salt">Salt for key generation.</param>
        /// <returns>Decrypted string.</returns>
        public static string DecryptStringAES(string cipherText, string sharedSecret, string salt)
        {
            return DecryptStringAES(cipherText, sharedSecret, GenerateSaltFromString(salt));
        }

        /// <summary>
        /// Decrypts a string that was encrypted using EncryptStringAES() and the same shared secret.
        /// </summary>
        /// <param name="cipherText">Encrypted text in Base64 format.</param>
        /// <param name="sharedSecret">Password for key generation.</param>
        /// <param name="salt">Salt for key generation.</param>
        /// <returns>Decrypted string.</returns>
        public static string DecryptStringAES(string cipherText, string sharedSecret, byte[] salt)
        {
            if (string.IsNullOrEmpty(cipherText))
                throw new ArgumentNullException(nameof(cipherText));
            if (string.IsNullOrEmpty(sharedSecret))
                throw new ArgumentNullException(nameof(sharedSecret));

            RijndaelManaged aesAlg = null;

            try
            {
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, salt);

                byte[] bytes = Convert.FromBase64String(cipherText);
                using (MemoryStream msDecrypt = new MemoryStream(bytes))
                {
                    aesAlg = new RijndaelManaged();
                    aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);

                    // Extracting IV from the encrypted stream
                    aesAlg.IV = ReadByteArray(msDecrypt);

                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
            finally
            {
                aesAlg?.Clear(); // Clear AES algorithm from memory
            }
        }

        private static byte[] ReadByteArray(Stream s)
        {
            byte[] rawLength = new byte[sizeof(int)];
            if (s.Read(rawLength, 0, rawLength.Length) != rawLength.Length)
                throw new SystemException("Stream did not contain properly formatted byte array");

            byte[] buffer = new byte[BitConverter.ToInt32(rawLength, 0)];
            if (s.Read(buffer, 0, buffer.Length) != buffer.Length)
                throw new SystemException("Did not read byte array properly");

            return buffer;
        }

        private static byte[] GenerateSaltFromString(string inputString)
        {
            if (string.IsNullOrEmpty(inputString))
            {
                throw new ArgumentException("Input string cannot be null or empty.");
            }

            // Convierte el texto en una matriz de bytes
            // Puedes usar Encoding.UTF8, Encoding.ASCII, etc., según tus necesidades
            return Encoding.UTF8.GetBytes(inputString);
        }
    }

}
