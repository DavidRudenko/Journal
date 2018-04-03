using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Journal.Security.Decryption;
using Journal.Security.Encryption;

namespace Journal.StorageProviders
{
    class StorageProvider:IStorageProvider
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
            return  entitiesContext.Entries.Select(c => new JournalEntry
            (c.Content, DateTime.Parse(c.TimeStamp),new RijndaelDecrypter(c.IV,passwd),new RijndaelEncryptor(c.IV,passwd))).ToList();
        }

        public void AddEntry(JournalEntry entry)
        {
            var entitiesContext = (_context as JournalEntities);
            if (entitiesContext == null)
                throw new ArgumentException($"{nameof(_context)} must be a JournalEntities instance");
            var iv = GenerateIV();
            var encryptedTimeStamp =new RijndaelEncryptor(iv,"1111").Encrypt(entry.TimeStamp.ToString(CultureInfo.InvariantCulture));
            var encryptedContent=new RijndaelEncryptor(iv,"1111").Encrypt(entry.Content);
            entitiesContext.Entries.Add(new Entry()
            {
                Content = encryptedContent,
                IV = iv,
                TimeStamp = encryptedTimeStamp
            });
            entitiesContext.SaveChanges();
        }

        private string GenerateIV()
        {
            var iv = new byte[128];
            for (int i = 0; i < 128; i++)
            {
                iv[i] = (byte)new Random(Guid.NewGuid().GetHashCode()).Next(0, 256);
            }
            return System.Text.Encoding.UTF8.GetString(iv);
        }
    }
}
