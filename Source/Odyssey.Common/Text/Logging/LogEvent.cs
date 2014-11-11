using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Text;

namespace Odyssey.Utilities.Logging
{
    

    public class LogEvent : AbstractLogEvent
    {
        public static LogEvent Engine = new LogEvent(Global.EngineTag);
        public static LogEvent UserInterface = new LogEvent(Global.UITag);
        public static LogEvent Network = new LogEvent(Global.NetworkTag);
        public static LogEvent Io = new LogEvent(Global.IoTag);
        public static LogEvent Tool = new LogEvent(Global.ToolTag);

        public LogEvent(string source) 
            : this (source, l => Debug.WriteLine(l.ToString()))
        {
        }

        public LogEvent(string source, DebugWriter method)
            : base(source, method)
        { }

        public void TraceSource(TraceData data, Exception ex, EventCode eventCode)
        {
            Contract.Requires(ex != null);
            StringBuilder sbMessage = new StringBuilder();
            sbMessage.Append(string.Format("\tSource: {0}\n", data.MethodSignature));
            if (data.AdditionalData.Count > 0)
                foreach (KeyValuePair<string, string> additionalData in data.AdditionalData)
                {
                    sbMessage.Append(string.Format("\t{0}: {1}", additionalData.Key, additionalData.Value));
                }

            Write(sbMessage.ToString());
        }

        protected void Write(string message)
        {
            Debug.WriteLine(message);
        }

        public void Info(string message, params object[] args)
        {
            LogData logEntry = new LogData(DateTime.Now, EventLevel.Information, EventCode.Info, Source, message, args);
            Log(logEntry);
        }

        public void Warning(string message, params object[] args)
        {
            LogData logEntry = new LogData(DateTime.Now, EventLevel.Warning, EventCode.Warning, Source, message, args);
            Log(logEntry);
        }

        public void Error(string message, params object[] args)
        {
            LogData logEntry = new LogData(DateTime.Now, EventLevel.Error, EventCode.CriticalFault, Source, message, args);
            Log(logEntry);
        }

        public void Error(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(ex.Message);
            sb.AppendLine(ex.StackTrace);
            LogData logEntry = new LogData(DateTime.Now, EventLevel.Error, EventCode.CriticalFault, Source, sb.ToString());
            Log(logEntry);
        }

        public void Error(string message, EventCode code, params object[] args)
        {
            LogData logEntry = new LogData(DateTime.Now, EventLevel.Error, code, Source, message, args);
            Log(logEntry);
        }
    }
}