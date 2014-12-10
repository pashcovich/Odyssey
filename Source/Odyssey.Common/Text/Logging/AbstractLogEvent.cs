#region Using Directives

using System;
using System.Diagnostics.Contracts;

#endregion

namespace Odyssey.Text.Logging
{
    public delegate void DebugWriter(LogData logEntry);

    public enum EventLevel
    {
        Information = 0,
        Warning = 1,
        Error = 2,
        Verbose = 3,
    }

    public enum EventCode
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

    public struct LogData
    {
        public object[] Args;
        public EventLevel Level;
        public string Message;
        public string Source;
        public DateTime TimeStamp;

        public LogData(DateTime timeStamp, EventLevel level, EventCode code, string source, string message, params object[] args)
        {
            TimeStamp = timeStamp;
            Level = level;
            Source = source;
            Message = message;
            Args = args;
        }

        public override string ToString()
        {
            return string.Format("[{0}] ({1}) {2}: {3}",
                TimeStamp.ToString("HH:mm:ss"),
                Source,
                Level,
                Args.Length == 0 ? Message : string.Format(Message, Args));
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

        protected DebugWriter WriterMethod { get; private set; }

        protected internal string Source { get; set; }

        /// <summary>
        ///     Writes formatted text by passing the relevant arguments.
        /// </summary>
        /// <param name="logEntry"></param>
        protected void Log(LogData logEntry)
        {
            WriterMethod(logEntry);
        }
    }
}