using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace PatientManagementApi.Utils
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    public static class EncryptionHelper
    {
        private static readonly string Key = "PatientSecureKey"; // Replace with a secure 16/24/32-character key
        private static readonly string IV = "PatientSecureIVV";  // Replace with a secure 16-character IV

        public static string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return null;

            using (var aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(Key);
                aes.IV = Encoding.UTF8.GetBytes(IV);

                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        using (var writer = new StreamWriter(cryptoStream))
                        {
                            writer.Write(plainText);
                        }
                    }
                    return Convert.ToBase64String(memoryStream.ToArray());
                }
            }
        }

        public static string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
                return null;

            try
            {
                using (var aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(Key);
                    aes.IV = Encoding.UTF8.GetBytes(IV);

                    using (var memoryStream = new MemoryStream(Convert.FromBase64String(cipherText)))
                    {
                        using (var cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
                        {
                            using (var reader = new StreamReader(cryptoStream))
                            {
                                return reader.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch (CryptographicException)
            {
                // Log or handle invalid ciphertext scenario
                return null;
            }
        }

    }
}
