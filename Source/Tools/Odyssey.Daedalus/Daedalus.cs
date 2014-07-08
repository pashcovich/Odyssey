using Odyssey.Tools.ShaderGenerator.View;
using Odyssey.Tools.ShaderGenerator.Viewer;
using Odyssey.Utilities.Logging;
using System;
using System.IO;
using System.Windows.Threading;
using Settings = Odyssey.Tools.ShaderGenerator.Properties.Settings;

namespace Odyssey.Tools.ShaderGenerator
{
    public static class Daedalus
    {
        internal static DirectXWindow DirectXWindow;

        public static void Shutdown()
        {
            DirectXWindow.Shutdown();
        }

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