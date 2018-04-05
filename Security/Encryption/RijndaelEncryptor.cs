using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Journal.Security.Encryption
{
    public class RijndaelEncryptor:IEncryptor
    {

        public string IV { get; }
        private byte[] _key { get; }
        private int _blockSize;
        public RijndaelEncryptor(string iv, string password,int blockSize=128)
        {
            this.IV = iv;
            this._key = KeyProvider.ObtainKey(password);
            this._blockSize = blockSize;
        }
        private ICryptoTransform GetEncryptor()
        {
            var decodedIV = IV;
            var iv = Convert.FromBase64String(decodedIV);//Encoding.Default.GetBytes(decodedIV);

            var rijndael = new RijndaelManaged() {Key = KeyProvider.ObtainKey("1111"), IV = iv};


            return rijndael.CreateEncryptor();
        }

        public string Encrypt(string content)
        {
            var encryptor = GetEncryptor();
            string encrypted;
            var bytes = Encoding.UTF8.GetBytes(content);
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    cs.Write(bytes, 0, bytes.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                    encrypted = Convert.ToBase64String(ms.ToArray());
                }
                ms.Close();
            }
            return encrypted;
        }
    }
}
