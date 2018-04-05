using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace Journal.Security
{
    public static class KeyProvider
    {
        private static  byte[] _key;
        private static string _passwd = "";
        private static string keyPath = "../../sv.bin";
        private static string passPath = "../.../pass.bin";
        private static readonly object _locker = new object();
        /// <summary>
        /// Regenerates the key and resets the password
        /// </summary>
        /// <param name="passwd">new password</param>
        /// <param name="blockSize">block size(in bits)</param>
        public static void GenerateKey(string passwd,int blockSize)
        {
            lock (_locker)
            {
                _key = new byte[blockSize / 8];
                for (int i = 0; i < blockSize/8; i++)
                {
                    _key[i] = (byte) new Random(Guid.NewGuid().GetHashCode()).Next(0, 256);
                }
                _passwd = passwd;
            }
        }

        /// <summary>
        /// Returns the key if the password is correct, null otherwise
        /// </summary>
        /// <param name="passwd">password</param>
        /// <returns></returns>
        public static byte[] ObtainKey(string passwd)
        {
            if (_key == null)
            {
                GetKey();
                
            }
            if (_passwd.Equals(passwd))
                return _key;
            return null;
            
        }

        static KeyProvider()
        {
            GetKey();
        }
        private static void GetKey()
        {
            if (!File.Exists(keyPath))
                return;
            var bf=new BinaryFormatter();
            var keyDesBase64 = (string)bf.Deserialize(new FileStream(keyPath,FileMode.Open));
            _passwd = (string) bf.Deserialize(new FileStream(passPath, FileMode.Open));
            _key = Convert.FromBase64String(keyDesBase64);
        }
        public static void SaveKey()
        {
            var bf = new BinaryFormatter();
            bf.Serialize(new FileStream(keyPath,FileMode.OpenOrCreate),Convert.ToBase64String(_key));
            bf.Serialize(new FileStream(passPath,FileMode.OpenOrCreate),_passwd);
            
        }
        public static string GenerateIV(int blockSize)
        {
            var iv = new byte[blockSize / 8];
            for (int i = 0; i < blockSize / 8; i++)
            {
                iv[i] = (byte)new Random(Guid.NewGuid().GetHashCode()).Next(0, 256);
            }
            return Convert.ToBase64String(iv);
        }
    }
}
