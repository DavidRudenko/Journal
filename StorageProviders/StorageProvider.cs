using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Journal.Security;
using Journal.Security.Decryption;
using Journal.Security.Encryption;

namespace Journal.StorageProviders
{
    public class StorageProvider:IStorageProvider
    {
        private DbContext _context { get; set; }

        public StorageProvider(DbContext context)
        {
            if(context==null)
                throw new ArgumentException(nameof(context));
            this._context = context;
        }
        
        public List<JournalEntry> GetEntries(string passwd)
        {
            var entitiesContext = (_context as JournalEntities);
            if(entitiesContext==null)
                throw new ArgumentException($"{nameof(_context)} must be a JournalEntities instance");
            var list = new List<JournalEntry>();
            foreach (var entry in entitiesContext.Entries)
            {
                var decryptedContent = new RijndaelDecrypter(entry.IV, "1111").Decrypt(entry.Content);
                var deryptedTimeStamp = new RijndaelDecrypter(entry.IV, "1111").Decrypt(entry.TimeStamp);
                list.Add(new JournalEntry(decryptedContent,DateTime.Parse(deryptedTimeStamp), null,null));
            }
            return list;
        }

        public void AddEntry(JournalEntry entry)
        {
            var entitiesContext = (_context as JournalEntities);
            if (entitiesContext == null)
                throw new ArgumentException($"{nameof(_context)} must be a JournalEntities instance");
            var iv = KeyProvider.GenerateIV(128);
            var encryptedTimeStamp =new RijndaelEncryptor(iv,"1111").Encrypt(entry.TimeStamp.ToString(CultureInfo.InvariantCulture));
            var encryptedContent=new RijndaelEncryptor(iv,"1111").Encrypt(entry.Content);
            entitiesContext.Entries.Add(new Entry()
            {
                Content = encryptedContent,
                IV = iv,
                TimeStamp = encryptedTimeStamp
            });
            entitiesContext.SaveChanges();
            KeyProvider.SaveKey();
        }

        
    }
}
