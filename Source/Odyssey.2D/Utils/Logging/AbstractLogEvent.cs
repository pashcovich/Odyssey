#if !ODYSSEY_ENGINE
using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Diagnostics.Tracing;


namespace Odyssey.Utils.Logging
{
    public enum EventCode : int
    {
        VerboseMessage = 00001,
        Info = 10001,

        // Warnings
        Warning = 70001,
        // Exceptions

        // Critical Errors
        CriticalFault = 90001,
        UnhandledException = 90002,

    }

    public enum EventLevel
    {
        Information = 0,
        Warning = 1,
        Error = 2,
        Verbose = 3,
    }

    public delegate void DebugWriter(LogData logEntry);
    public struct LogData
    {
        public object[] Args;
        public EventCode Code;
        public EventLevel Level;
        public string Message;
        public string Source;
        public DateTime TimeStamp;
        public LogData(DateTime timeStamp, EventLevel level, EventCode code, string source, string message, params object[] args)
        {
            TimeStamp = timeStamp;
            Level = level;
            Code = code;
            Source = source;
            Message = message;
            Args = args;
        }

        public override string ToString()
        {
            return string.Format("[{0}] {1} ({2}) {3}",
                TimeStamp.ToString("HH:mm:ss"),
                //Level == EventLevel.Error ? string.Format("{0}: {1}", Level, (int)Code) : Level.ToString(),
                string.Format("{0}:", Level),
                Source,
                string.Format(Message, Args));
        }
        }

    public abstract class AbstractLogEvent
    {
        protected AbstractLogEvent(string source, DebugWriter method)
        {
            Contract.Requires(!string.IsNullOrEmpty(source));
            Contract.Requires(method != null);

            Source = source;
            WriterMethod = method;
        }

        protected string Source { get; private set; }
        protected DebugWriter WriterMethod { get; private set; }
        /// <summary>
        /// Writes formatted text by passing the relevant arguments.
        /// </summary>
        /// <param name="args"></param>
        /// 
        protected void Log(LogData logEntry)
        {
            WriterMethod(logEntry);
        }
    }
}
#endif