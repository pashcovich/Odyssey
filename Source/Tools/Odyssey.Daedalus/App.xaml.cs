using System;
using System.Windows;
using Odyssey.Utilities.Logging;

namespace Odyssey.Daedalus
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        public App()
        {
            //AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            //System.Windows.Application.Current.Dispatcher.UnhandledException += Dispatcher_UnhandledException;
        }

        private void Dispatcher_UnhandledException(object sender,
            System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception) e.Exception;
            HandleException(ex);
            e.Handled = true;
        }


        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception) e.ExceptionObject;
            HandleException(ex);
        }

        private void HandleException(Exception ex)
        {
            string errorMessage = string.Format(
                "An unhandled exception occurred: {0}\nPlease check the log file:\n{1}", ex.Message, Log.FilePath);
            MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Log.Daedalus.Error(ex.Message);
        }
    };

}
