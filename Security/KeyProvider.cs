using System;
using System.Linq;

namespace Journal.Security
{
    static class KeyProvider
    {
        private static readonly byte[] _key = new byte[128];
        private static string _passwd = "";
        private static readonly object _locker = new object();
        /// <summary>
        /// Regenerates the key and resets the password
        /// </summary>
        /// <param name="passwd">new password</param>
        public static void GenerateKey(string passwd)
        {
            lock (_locker)
            {
                for (int i = 0; i < 128; i++)
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
            if(_key.All(c=>c==0))
                GenerateKey(passwd);
            if (_passwd.Equals(passwd))
                return _key;
            return null;
            
        }
    }
}
