using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journal.StorageProviders
{
    public interface IStorageProvider
    {
        List<JournalEntry> GetEntries(string passwd);
        void AddEntry(JournalEntry entry);
    }
}
