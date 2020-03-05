using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Headstone.Framework.Security.Cryptography
{
    class RSAEncryption : EncryptorBase
    {
        public int DwKeySize { get; set; }
        public string XmlString { get; set; }

        public RSAEncryption()
        {
            DwKeySize = 10;
            XmlString = "<key>1234567890</key>";
        }

        public override string Encrypt(string input)
        {
            RSACryptoServiceProvider rsaCryptoServiceProvider =
                                      new RSACryptoServiceProvider(DwKeySize);
            rsaCryptoServiceProvider.FromXmlString(XmlString);
            int keySize = DwKeySize / 8;
            byte[] bytes = Encoding.UTF32.GetBytes(input);
            //RSACryptoServiceProvider here 
            int maxLength = keySize - 42;
            int dataLength = bytes.Length;
            int iterations = dataLength / maxLength;
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i <= iterations; i++)
            {
                byte[] tempBytes = new byte[
                        (dataLength - maxLength * i > maxLength) ? maxLength :
                                                      dataLength - maxLength * i];
                Buffer.BlockCopy(bytes, maxLength * i, tempBytes, 0,
                                  tempBytes.Length);
                byte[] encryptedBytes = rsaCryptoServiceProvider.Encrypt(tempBytes,
                                                                          true);
                stringBuilder.Append(Convert.ToBase64String(encryptedBytes));
            }
            return stringBuilder.ToString();
        }

        public override string Decrypt(string inputString)
        {
            RSACryptoServiceProvider rsaCryptoServiceProvider
                                         = new RSACryptoServiceProvider(DwKeySize);
            rsaCryptoServiceProvider.FromXmlString(XmlString);
            int base64BlockSize = ((DwKeySize / 8) % 3 != 0) ?
              (((DwKeySize / 8) / 3) * 4) + 4 : ((DwKeySize / 8) / 3) * 4;
            int iterations = inputString.Length / base64BlockSize;
            ArrayList arrayList = new ArrayList();
            for (int i = 0; i < iterations; i++)
            {
                byte[] encryptedBytes = Convert.FromBase64String(
                     inputString.Substring(base64BlockSize * i, base64BlockSize));
                Array.Reverse(encryptedBytes);
                arrayList.AddRange(rsaCryptoServiceProvider.Decrypt(
                                    encryptedBytes, true));
            }
            return Encoding.UTF32.GetString(arrayList.ToArray(
                                      Type.GetType("System.Byte")) as byte[]);
        }



    }
}
