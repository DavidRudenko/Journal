using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Journal.StorageProviders;

namespace Journal.Model
{
    public interface IDataService
    {
        void GetData(Action<IStorageProvider, Exception> callback);
    }
}
