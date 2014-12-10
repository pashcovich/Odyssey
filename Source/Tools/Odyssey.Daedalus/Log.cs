using Odyssey.Daedalus.Properties;
using System.IO;
using Odyssey.Text.Logging;

namespace Odyssey.Daedalus
{
    public static class Log
    {
        public static LogEvent Daedalus = new LogEvent("Daedalus", Write);
        static StreamWriter logFile;
        public static string FilePath { get; private set; }

        public static void Init()
        {
            FilePath = Path.Combine(Settings.Default.DefaultPath, "Daedalus.log");
            logFile = new StreamWriter(FilePath) { AutoFlush = true };
        }

        static void Write(LogData entry)
        {
            logFile.WriteLine(entry.ToString());
        }

        public static void Close()
        {
            Log.Daedalus.Info("Daedalus out.");
            logFile.Close();
        }
    }
}
