using System.Security.Cryptography;
using System.Text;

namespace JazzApi.Helpers
{
    public class EncryptionHelper
    {
        private readonly string encryptionKey;

        public EncryptionHelper(IConfiguration configuration)
        {
            encryptionKey = configuration["EncryptionKey"];
        }

        public string Encrypt(string plainText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(encryptionKey);
                aes.GenerateIV();
                byte[] iv = aes.IV;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, iv);
                using (var ms = new MemoryStream())
                {
                    ms.Write(iv, 0, iv.Length);
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (var sw = new StreamWriter(cs))
                        {
                            sw.Write(plainText);
                        }
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public string Decrypt(string encryptedText)
        {
            byte[] cipherTextCombined = Convert.FromBase64String(encryptedText);
            using (Aes aes = Aes.Create())
            {
                byte[] iv = new byte[aes.BlockSize / 8];
                byte[] cipherText = new byte[cipherTextCombined.Length - iv.Length];

                Array.Copy(cipherTextCombined, iv, iv.Length);
                Array.Copy(cipherTextCombined, iv.Length, cipherText, 0, cipherText.Length);

                aes.Key = Encoding.UTF8.GetBytes(encryptionKey);
                aes.IV = iv;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using (var ms = new MemoryStream(cipherText))
                {
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (var sr = new StreamReader(cs))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
