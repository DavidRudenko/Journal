using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Journal.Security.Decryption
{
    class RijndaelDecrypter:IDecrypter
    {
        public string IV { get; }
        private byte[] _key { get; }

        public RijndaelDecrypter(string iv,string password)
        {
            this.IV = iv;
            this._key = KeyProvider.ObtainKey(password);
        }

        public string Decrypt(string encryptedContent)
        {
            var buffer = Convert.FromBase64String(encryptedContent);
            string decrypted;
            var transform = GetDecryptor();
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, transform, CryptoStreamMode.Write))
                {
                    cs.Write(buffer, 0, buffer.Length);
                    cs.FlushFinalBlock();
                    decrypted = Encoding.UTF8.GetString(ms.ToArray());
                    cs.Close();
                }
                ms.Close();
            }
            return decrypted;
        }

        private ICryptoTransform GetDecryptor()
        {
            var decodedIV = DecodeBase64(IV);
            var iv = Encoding.Default.GetBytes(decodedIV);

            var rijndael = new RijndaelManaged
            {
                BlockSize = 128,
                IV = iv,
                KeySize = 128,
                Key = _key
            };
            

           return rijndael.CreateDecryptor();
        }
        private string DecodeBase64(string toDecode)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(toDecode);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
