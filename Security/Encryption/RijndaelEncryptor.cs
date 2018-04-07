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
        
        private ICryptoTransform GetEncryptor(string password)
        {
            var rijndael = new RijndaelManaged() {Key = KeyProvider.GetKey(password), IV=KeyProvider.GetIV(password)};
            return rijndael.CreateEncryptor();
        }
        
        public string Encrypt(string content,string password)
        {
            var encryptor = GetEncryptor(password);
            
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
