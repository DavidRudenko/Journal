using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Journal.Model;
using Journal.Security;
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
        public event EventHandler GetEntriesCompleted;
        public event EventHandler GetEntriesFailed;
        public event EventHandler AddEntryCompleted;
        public event EventHandler AddEntryFailed;
        private string _content;
        public string Content
        {
            get { return _content; }
            set { Set(ref _content, value); }
        }
        public RelayCommand<string> GetEntriesCommand { get;  }
        public RelayCommand<string> AddEntryCommand { get; }
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
            AddEntryCommand=new RelayCommand<string>(AddEntry);
            _dataService.GetData((provider, error) =>
            {
                if (error != null)
                {}
                //error handling
                this._entriesProvider = provider;
            });
        }

        public static void SetPassword(string password)
        {
            KeyProvider.SetPassword(null, password);
        }
        private async void AddEntry(string passwd)
        {
            if (!KeyProvider.CorrectPassword(passwd))
                RaiseAddEntryFailed();
            var newEntry=new JournalEntry(Content,DateTime.Now);
            var task = Task.Factory.StartNew(() =>
            {
                _entriesProvider.AddEntry(newEntry, passwd);
            });
            try
            {
                await task;
                RaiseAddEntryCompleted();
            }
            catch (Exception)
            {
                RaiseAddEntryFailed();
                
            }
        }
        private async void GetEntries(string passwd)
        {
            if (!KeyProvider.CorrectPassword(passwd))
                RaiseGetEntriesFailed();
            var task = Task<List<JournalEntry>>.Factory.StartNew(() =>
            {
                return _entriesProvider.GetEntries(passwd);
            });
            var result = new List<JournalEntry>();
            try
            {
                result=await task;
                RaiseGetEntriesCompleted();
            }
            catch (Exception)
            {
                RaiseGetEntriesFailed();
            }
            this.Entries = new ObservableCollection<JournalEntry>(result);
        }

        private void RaiseGetEntriesCompleted()
        {
            GetEntriesCompleted?.Invoke(this,EventArgs.Empty);
        }
        private void RaiseGetEntriesFailed()
        {
            GetEntriesFailed?.Invoke(this, EventArgs.Empty);
        }
        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
        protected virtual void RaiseAddEntryCompleted()
        {
            AddEntryCompleted?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void RaiseAddEntryFailed()
        {
            AddEntryFailed?.Invoke(this, EventArgs.Empty);
        }
    }
}