using System.Windows;
using Journal.ViewModel;
using Microsoft.Practices.ServiceLocation;

namespace Journal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _wasClicked = false;
        private MainViewModel mv;
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();
            mv = ServiceLocator.Current.GetInstance<MainViewModel>();
            mv.GetEntriesFailed += (sender,e) =>
            {
                getEntriesPasswordBox.Text = "";
                AddEntrySP.Visibility = Visibility.Collapsed;
                _wasClicked = !_wasClicked;
                MessageBox.Show("Get Entries Failed");
            };
            mv.GetEntriesCompleted += (sender, e) =>
            {
                getEntriesPasswordBox.Text = "";
                AddEntrySP.Visibility = Visibility.Collapsed;
                _wasClicked = !_wasClicked;
                MessageBox.Show("Get Entries Succeeded");
            };
            mv.AddEntryCompleted += (sender, e) =>
            {
                AddEntryPasswordBox.Text = "";
                AddEntrySP.Visibility = Visibility.Collapsed;
                _wasClicked = !_wasClicked;
                MessageBox.Show("Add Entry Succeeded");
            };
            mv.AddEntryFailed += (sender, e) =>
            {
                AddEntryPasswordBox.Text = "";
                AddEntrySP.Visibility = Visibility.Collapsed;
                _wasClicked = !_wasClicked;
                MessageBox.Show("Add Entry Failed");
            };

        }

        private void AddEntryButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (_wasClicked)
                AddEntrySP.Visibility = Visibility.Collapsed;
            if(!_wasClicked)
                AddEntrySP.Visibility = Visibility.Visible;
            _wasClicked = !_wasClicked;

        }

        private void SetPasswordBtn_OnClick(object sender, RoutedEventArgs e)
        {
            MainViewModel.SetPassword(setPasswordBox.Password);
            setPassSP.Visibility = Visibility.Collapsed;
        }
    }
}