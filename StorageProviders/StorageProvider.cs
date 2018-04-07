using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
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
        
        public List<JournalEntry> GetEntries(string password)
        {
            var entitiesContext = (_context as JournalEntities);
            if(entitiesContext==null)
                throw new ArgumentException($"{nameof(_context)} must be a JournalEntities instance");
            var list = new List<JournalEntry>();
            foreach (var entry in entitiesContext.Entries)
            {
                var decryptedContent = new RijndaelDecrypter().Decrypt(entry.Content,password);
                var deryptedTimeStamp = new RijndaelDecrypter().Decrypt(entry.TimeStamp,password);
                list.Add(new JournalEntry(decryptedContent,DateTime.Parse(deryptedTimeStamp)));
            }
            return list;
        }

        public void AddEntry(JournalEntry entry,string password)
        {
            var entitiesContext = (_context as JournalEntities);
            if (entitiesContext == null)
                throw new ArgumentException($"{nameof(_context)} must be a JournalEntities instance");
            var encryptedTimeStamp =new RijndaelEncryptor().Encrypt(entry.TimeStamp.ToString(CultureInfo.InvariantCulture),password);
            var encryptedContent=new RijndaelEncryptor().Encrypt(entry.Content,password);
            entitiesContext.Entries.Add(new Entry()
            {
                Content = encryptedContent,
                TimeStamp = encryptedTimeStamp
            });
            entitiesContext.SaveChanges();
        }

        
    }
}
