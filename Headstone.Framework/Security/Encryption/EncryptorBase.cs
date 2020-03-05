using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headstone.Framework.Security.Cryptography
{
    public abstract class EncryptorBase
    {
        private string _salt;

        protected byte[] Salt
        {
            get
            {
                if (string.IsNullOrEmpty(_salt))
                {
                    //if (ConfigurationManager.AppSettings["EncryptSalt"] != null)
                    //{
                    //    _salt = ConfigurationManager.AppSettings["EncryptSalt"];
                    //}
                    //else
                    //{
                        _salt = "EK1NFR@M0RKs4l7";// "defaultSalt";
                    //}
                }
                return System.Text.Encoding.UTF8.GetBytes(_salt);
            }
        }

        public abstract string Encrypt(string plainText);

        public string Encrypt(string plainText, string salt)
        {
            this._salt = salt;
            return Encrypt(plainText);
        }

        public abstract string Decrypt(string encryptedString);

        public string Decrypt(string encryptedString, string salt)
        {
            this._salt = salt;
            return Decrypt(encryptedString);
        }

        public bool Verify(string inputText, string hashedText, string salt)
        {
            this._salt = salt;
            return Verify(inputText, hashedText);
        }

        public bool Verify(string inputText, string hashedText)
        {
            string hashOfInput = Encrypt(inputText);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            if (0 == comparer.Compare(hashOfInput, hashedText))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
