using System;
using Journal.Model;
using Journal.StorageProviders;

namespace Journal.Design
{
    public class DesignDataService : IDataService
    {
        public void GetData(Action<IStorageProvider, Exception> callback)
        {
            // Use this to create design time data
        }
    }
}