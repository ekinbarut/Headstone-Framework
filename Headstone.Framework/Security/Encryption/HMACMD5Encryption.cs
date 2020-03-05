using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Headstone.Framework.Security.Cryptography
{
    class HMACMDMD5Encryption : EncryptorBase
    {
        public override string Encrypt(string input)
        {
            using (var hmacMD5 = new HMACMD5(Salt))
            {
                // Convert the input string to a byte array and compute the hash.
                byte[] data = hmacMD5.ComputeHash(Encoding.UTF8.GetBytes(input));
                // Create a new Stringbuilder to collect the bytes
                // and create a string.
                StringBuilder sBuilder = new StringBuilder();
                // Loop through each byte of the hashed data 
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                // Return the hexadecimal string.
                return sBuilder.ToString();
            }
        }


        public override string Decrypt(string input)
        {
            throw new Exception("Cannot Decrypt MD5 Code!");
        }
    }
}
