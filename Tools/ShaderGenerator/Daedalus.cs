using Odyssey.Tools.ShaderGenerator.Properties;
using Odyssey.Tools.ShaderGenerator.ViewModel;
using Odyssey.Utils.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace Odyssey.Tools.ShaderGenerator
{
    public static class Daedalus
    {

        public static void Initialize()
        {
            string appDataPath = Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData, // Search under %APPDATA%
                Environment.SpecialFolderOption.Create // Create the folder if it does not exists
                    );
            string dataFolderPath = System.IO.Path.Combine(
                appDataPath,
                "Avengers_UTD",
                "Daedalus"
            );

            if (!Directory.Exists(dataFolderPath))
                Directory.CreateDirectory(dataFolderPath);

            string outputPath = Path.Combine(dataFolderPath, "Output");

            if (!Directory.Exists(outputPath))
                Directory.CreateDirectory(outputPath);
            
            if (string.IsNullOrEmpty(Settings.Default.DefaultPath))
            {
                
                Settings.Default.DefaultPath = dataFolderPath;
                Settings.Default.OutputPath = outputPath;
                Settings.Default.Save();
            }

            Log.Init();
            Log.Daedalus.Info("Daedalus in.");
        }
    }
}
