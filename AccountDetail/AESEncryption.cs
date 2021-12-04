using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Security.Cryptography;
using System.IO;
using System.Configuration;

namespace Prism.Utility
{
    public static class AesEncryption
    {
        public readonly static string Key = ConfigurationManager.AppSettings["AESEncr"].ToString();
        public static string Decode(string cipherText)
        {
            try
            {
                var KeyBytes = Encoding.UTF8.GetBytes(Key);
                var iv = Encoding.UTF8.GetBytes(Key);
                var encrypted = Convert.FromBase64String(cipherText);
                var decriptedFromJavascript = DecryptStringFromBytes(encrypted, KeyBytes, iv);
                return string.Format(decriptedFromJavascript);
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public static string Encode(string plainText)
        {
            try
            {
                var KeyBytes = Encoding.UTF8.GetBytes(Key);
                var iv = Encoding.UTF8.GetBytes(Key);
                var encryptForJavascript = EncryptStringToBytes(plainText, KeyBytes, iv);
                return string.Format(Convert.ToBase64String(encryptForJavascript));
            }
            catch (Exception e)
            {
                return null;
            }
        }
        private static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            
            if (cipherText == null || cipherText.Length <= 0)
            {
                throw new ArgumentNullException("cipherText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
         
            string plaintext = null;
            using (var rijAlg = new RijndaelManaged())
            {
               
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);
                try
                {
                  
                    using (var msDecrypt = new MemoryStream(cipherText))
                    {
                        var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);

                        var srDecrypt = new StreamReader(csDecrypt);
                      
                        plaintext = srDecrypt.ReadToEnd();

                    }
                }
                catch
                {
                    plaintext = "keyError";
                }
            }

            return plaintext;
        }
        private static byte[] EncryptStringToBytes(string plainText, byte[] key, byte[] iv)
        {
           
            if (plainText == null || plainText.Length <= 0)
            {
                throw new ArgumentNullException("plainText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            byte[] encrypted;
           
            using (var rijAlg = new RijndaelManaged())
            {
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

               
                var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);
              
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
           
            return encrypted;
        }
    }
}
