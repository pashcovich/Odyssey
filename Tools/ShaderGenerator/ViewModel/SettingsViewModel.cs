﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Odyssey.Tools.ShaderGenerator.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {

        public ICommand SaveSettingsCommand
        {
            get
            {
                return new RelayCommand<Window>((window) =>
                {
                    Properties.Settings.Default.Save();
                    window.Close();
                });
            }
        }

    }
}
