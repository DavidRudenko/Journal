using System;
using System.Windows;
using GalaSoft.MvvmLight.Threading;
using Journal.Security;

namespace Journal
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            DispatcherHelper.Initialize();
            AppDomain.CurrentDomain.DomainUnload += (s, e) => KeyProvider.SaveKey();
        }
    }
}
