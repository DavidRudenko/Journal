using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Journal.Security.Decryption
{
    public class RijndaelDecrypter:IDecrypter
    {
        public string IV { get; }
        private byte[] _key { get; }
        private int _blockSize;
        public RijndaelDecrypter(string iv,string password,int blockSize=128)
        {
            this.IV = iv;
            this._key = KeyProvider.ObtainKey(password);
            this._blockSize = blockSize;
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
                    cs.Clear();
                    cs.Close();
                    decrypted = Encoding.UTF8.GetString(ms.ToArray());
                    ms.Close();
                }
            }
            return decrypted;
        }

        private ICryptoTransform GetDecryptor()
        {
            var iv = DecodeBase64(IV);

            var rijndael = new RijndaelManaged {Key = _key, IV = iv};
           return rijndael.CreateDecryptor();
        }
        private byte[] DecodeBase64(string toDecode)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(toDecode);
            return base64EncodedBytes;
        }
    }
}
