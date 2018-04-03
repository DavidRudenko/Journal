using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Journal.Model;
using Journal.Security.Decryption;
using Journal.StorageProviders;

namespace Journal.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;
        private ObservableCollection<JournalEntry> _entries;
        private IStorageProvider _entriesProvider;
        public RelayCommand<string> GetEntriesCommand { get; set; }
        public ObservableCollection<JournalEntry> Entries
        {
            get { return _entries; }
            set { Set(ref _entries, value); }
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IDataService dataService)
        {
            _dataService = dataService;
            GetEntriesCommand=new RelayCommand<string>(GetEntries);
            _dataService.GetData((provider, error) =>
            {
                if (error != null)
                {}
                //error handling
                this._entriesProvider = provider;
            });
        }

        private void GetEntries(string passwd)
        {
            this.Entries = new ObservableCollection<JournalEntry>(_entriesProvider.GetEntries(passwd));
        }
        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}