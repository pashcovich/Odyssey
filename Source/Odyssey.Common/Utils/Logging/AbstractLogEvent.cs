#region Using directives

using Odyssey.Utils.Logging;
using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Diagnostics.Tracing;

#endregion Using directives

namespace Odyssey.Utils.Logging
{
    public delegate void DebugWriter(LogData logEntry);

    public enum EventLevel
    {
        Information = 0,
        Warning = 1,
        Error = 2,
        Verbose = 3,
    }

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

    public struct LogData
    {
        public DateTime TimeStamp;
        public EventLevel Level;
        public EventCode Code;
        public string Source;
        public string Message;
        public object[] Args;

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
                string.Format("{0}:", Level),
                Source,
                Args.Length == 0 ? Message : string.Format(Message, Args));
        }
    }

    public abstract class AbstractLogEvent
    {
        protected DebugWriter WriterMethod { get; private set; }
        protected AbstractLogEvent(string source, DebugWriter method)
        {
            Contract.Requires(!string.IsNullOrEmpty(source));
            Contract.Requires(method != null);

            Source = source;
            WriterMethod = method;
        }

        internal protected string Source { get; set; }

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