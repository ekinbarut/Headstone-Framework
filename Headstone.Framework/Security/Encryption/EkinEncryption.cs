using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Headstone.Framework.Security.Cryptography
{
    class EkinEncryption : EncryptorBase
    {
        private static string key = "sfdjf48mdfdf3054";

        public EkinEncryption()
        {
        }

        public override string Encrypt(String plainText)
        {
            string encrypted = null;
            try
            {
                byte[] inputBytes = Encoding.GetEncoding("ISO-8859-9").GetBytes(plainText);
                byte[] pwdhash = null;
                MD5CryptoServiceProvider hashmd5;

                //generate an MD5 hash from the password. 
                //a hash is a one way encryption meaning once you generate
                //the hash, you cant derive the password back from it.
                hashmd5 = new MD5CryptoServiceProvider();
                pwdhash = hashmd5.ComputeHash(Encoding.GetEncoding("ISO-8859-9").GetBytes(key));
                hashmd5 = null;

                // Create a new TripleDES service provider 
                TripleDESCryptoServiceProvider tdesProvider = new TripleDESCryptoServiceProvider();
                tdesProvider.Key = pwdhash;
                tdesProvider.Mode = CipherMode.ECB;

                encrypted = Convert.ToBase64String(
                    tdesProvider.CreateEncryptor().TransformFinalBlock(inputBytes, 0, inputBytes.Length));
            }
            catch (Exception e)
            {
                string str = e.Message;
                throw;
            }
            return encrypted;
        }

        public override String Decrypt(string encryptedString)
        {
            string decyprted = null;
            byte[] inputBytes = null;

            try
            {
                inputBytes = Convert.FromBase64String(encryptedString);
                byte[] pwdhash = null;
                MD5CryptoServiceProvider hashmd5;

                //generate an MD5 hash from the password. 
                //a hash is a one way encryption meaning once you generate
                //the hash, you cant derive the password back from it.
                hashmd5 = new MD5CryptoServiceProvider();
                pwdhash = hashmd5.ComputeHash(Encoding.Default.GetBytes(key));
                hashmd5 = null;

                // Create a new TripleDES service provider 
                TripleDESCryptoServiceProvider tdesProvider = new TripleDESCryptoServiceProvider();
                tdesProvider.Key = pwdhash;
                tdesProvider.Mode = CipherMode.ECB;

                decyprted = Encoding.GetEncoding("ISO-8859-9").GetString(
                    tdesProvider.CreateDecryptor().TransformFinalBlock(inputBytes, 0, inputBytes.Length));
            }
            catch (Exception e)
            {
                string str = e.Message;
                throw;
            }
            return decyprted;
        }
    }
}
