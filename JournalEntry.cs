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
        public JournalEntry(string content, DateTime timeStamp)
        {
             TimeStamp = timeStamp;
             Content = content;
            
        }

        public override string ToString()
        {
            return "TimeStamp: " + TimeStamp + ", Content: " + Content;
        }
        
    }
}
