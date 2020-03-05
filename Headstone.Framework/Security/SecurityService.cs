using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Headstone.Framework.Models;
using Headstone.Framework.Models.Configuration;
using Headstone.Framework.Security.Cryptography;

namespace Headstone.Framework.Security
{
    public class SecurityService
    {
        #region [ Crypto ]

        private static readonly EncryptorBase CurrentEncryptor = Init();

        private static EncryptorBase Init(string encryptorName = null)
        {
            string encryption = string.Empty;

            if (string.IsNullOrEmpty(encryptorName))
            {
                encryption = SecurityConfig.Encryption;
            }
            else
            {
                encryption = encryptorName.ToLower();
            }

            switch (encryption)
            {
                case "basic":
                case "bc":
                case "basicencryption":
                    return new BasicEncryption();
                case "md5":
                case "md5encryption":
                    return new MD5Encryption();
                case "rsa":
                case "rsaencryption":
                    return new RSAEncryption();
                case "ekin":
                    return new EkinEncryption();
                case "hmd5":
                case "hmacmd5":
                case "hmacmdmd5encryption":
                    return new EkinEncryption();
                case "rijndaelencryption":
                case "rijndael":
                    return new RijndaelEncryption();
                default:
                    return new EkinEncryption();
            }
        }

        public static string Encrypt(string input)
        {
            return CurrentEncryptor.Encrypt(input);
        }

        public static string Encrypt(string input, string salt)
        {
            return CurrentEncryptor.Encrypt(input, salt);
        }

        public static string Decrypt(string input)
        {
            return CurrentEncryptor.Decrypt(input);
        }

        public static string Decrypt(string input, string salt)
        {
            return CurrentEncryptor.Decrypt(input, salt);
        }

        public static bool Verify(string input, string hash)
        {
            return CurrentEncryptor.Verify(input, hash);
        }

        public static bool Verify(string input, string hash, string salt)
        {
            return CurrentEncryptor.Verify(input, hash, salt);
        }

        public static string Encrypt(string input, EncryptorType encryptorType)
        {
            EncryptorBase enc = Init(encryptorType.ToString());
            return enc.Encrypt(input);
        }

        public static string Encrypt(string input, EncryptorType encryptorType, string salt)
        {
            EncryptorBase enc = Init(encryptorType.ToString());
            return enc.Encrypt(input, salt);
        }

        public static string Decrypt(string input, EncryptorType encryptorType)
        {
            EncryptorBase enc = Init(encryptorType.ToString());
            return enc.Decrypt(input);
        }

        public static string Decrypt(string input, EncryptorType encryptorType, string salt)
        {
            EncryptorBase enc = Init(encryptorType.ToString());
            return enc.Decrypt(input, salt);
        }

        public static bool Verify(string input, string hash, EncryptorType encryptorType)
        {
            EncryptorBase enc = Init(encryptorType.ToString());
            return enc.Verify(input, hash);
        }

        public static bool Verify(string input, string hash, EncryptorType encryptorType, string salt)
        {
            EncryptorBase enc = Init(encryptorType.ToString());
            return enc.Verify(input, hash, salt);
        }

        #endregion
    }
}
