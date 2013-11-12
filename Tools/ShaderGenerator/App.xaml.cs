using Odyssey.Tools.ShaderGenerator.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Odyssey.Tools.ShaderGenerator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App() 
        {
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            Application.Current.Dispatcher.UnhandledException += Dispatcher_UnhandledException;
        }

        void Dispatcher_UnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.Exception;
            HandleException(ex);
            e.Handled = true;
        }

       
        void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            HandleException(ex);
        }

        void HandleException(Exception ex)
        {
            string errorMessage = string.Format("An unhandled exception occurred: {0}\nPlease check the log file:\n{1}", ex.Message, Log.FilePath);
            MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Log.Daedalus.Error(ex.Message);
        }

    }
}
