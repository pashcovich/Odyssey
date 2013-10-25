using Odyssey.Tools.ShaderGenerator.Properties;
using Odyssey.Utils.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Odyssey.Tools.ShaderGenerator
{
    public static class ShaderGen
    {

        public static void Initialize()
        {
            LogEvent.Tool.Source = "ShaderGen";
            if (Settings.Default.OutputPath == "{AppDir}")
            {
                string path = System.IO.Path.GetDirectoryName(
                    System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase) + "\\Output";
                Settings.Default.OutputPath = path;
                Settings.Default.Save();
            }

        }
    }
}
