using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;

namespace Journal.Security
{
    public static class KeyProvider
    {
        private static byte[] _salt=new byte[16]  { 0x26, 0xdc, 0xff, 0x00, 0xad, 0xed, 0x7a,
            0xee, 0xc5, 0xfe, 0x07, 0xaf, 0x4d, 0x08, 0x22, 0x3c };//Random salt, might put it into 
        // file,but leaving it open won`t do much damage to security
        private static object _locker = new object();

        private static string password = null;
        public static byte[] GetKey()
        {
            var db=new Rfc2898DeriveBytes(password,_salt);
            return db.GetBytes(32);
        }

        public static void SetPassword(string oldPassword, string newPassword)
        {
            if (oldPassword == password)
                password = newPassword;
            else
                throw new Exception("Passwords don`t match");
        }

        public static bool CorrectPassword(string pass)
        {
            return pass == password;
        }
        public static byte[] GetIV()
        {
            var db=new Rfc2898DeriveBytes(password,_salt);
            return db.GetBytes(16);
        }
    }
}
