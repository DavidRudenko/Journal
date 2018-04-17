using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Journal
{
    /// <summary>
    /// Interaction logic for EntryView.xaml
    /// </summary>
    public partial class EntryView : UserControl
    {
        public static readonly DependencyProperty EntryContentProperty=
            DependencyProperty.Register("EntryContent",typeof(string),typeof(EntryView));

        public static readonly DependencyProperty TimestampProperty=
            DependencyProperty.Register("Timestamp",typeof(string),typeof(EntryView));

        public string Timestamp
        {
            get { return (string) GetValue(TimestampProperty); }
            set { SetValue(TimestampProperty,value);}
        }

        public string EntryContent
        {
            get { return (string) GetValue(EntryContentProperty); }
            set {SetValue(EntryContentProperty,value); }
        }
        
        public EntryView()
        {
            InitializeComponent();
        }
    }
}
