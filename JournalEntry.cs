using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Journal.Security.Decryption;
using Journal.Security.Encryption;
using Journal.StorageProviders;

namespace Journal
{
    public sealed class JournalEntry
    {
        public string Content { get; private set; }
        public DateTime TimeStamp { get; private set; }
        private IDecrypter _decrypter;
        private IEncryptor _encrypter;
        public JournalEntry(string content, DateTime timeStamp,IDecrypter decrypter, IEncryptor encrypter)
        {
             TimeStamp = timeStamp;
             Content = content;
            this._decrypter = decrypter;
            this._encrypter = encrypter;
        }
        public void Decrypt(JournalEntry encryptedEntry)
        {
            this.TimeStamp =
                DateTime.Parse(_decrypter.Decrypt(encryptedEntry.TimeStamp.ToString(CultureInfo.InvariantCulture)));
            this.Content = _decrypter.Decrypt(encryptedEntry.Content);
        }
    }
}
