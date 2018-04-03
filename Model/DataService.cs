using System;
using Journal.StorageProviders;

namespace Journal.Model
{
    public class DataService : IDataService
    {
        public void GetData(Action<IStorageProvider, Exception> callback)
        {
            callback(new StorageProvider(new JournalEntities()), null);
        }
    }
}