#if !ODYSSEY_ENGINE
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;

namespace Odyssey.Utils.Logging
{
    

    public class LogEvent : AbstractLogEvent
    {
        public static LogEvent Io = new LogEvent("IO");
        public static LogEvent UserInterface = new LogEvent("UI");

        public LogEvent(string source) 
            : this (source, (l) => System.Diagnostics.Debug.WriteLine(l.ToString()))
        {
        }

        public LogEvent(string source, DebugWriter method)
            : base(source, method)
        { }

        public void Error(string message, params object[] args)
        {
            LogData logEntry = new LogData(DateTime.Now, EventLevel.Error, EventCode.CriticalFault, Source, message, args);
            Log(logEntry);
        }

        public void Error(string message, EventCode code, params object[] args)
        {
            LogData logEntry = new LogData(DateTime.Now, EventLevel.Error, code, Source, message, args);
            Log(logEntry);
        }

        public void Info(string message, params object[] args)
        {
            LogData logEntry = new LogData(DateTime.Now, EventLevel.Information, EventCode.Info, Source, message, args);
            Log(logEntry);
        }

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

        public void Warning(string message, params object[] args)
        {
            LogData logEntry = new LogData(DateTime.Now, EventLevel.Warning, EventCode.Warning, Source, message, args);
            Log(logEntry);
        }

        protected void Write(string message)
        {
            System.Diagnostics.Debug.WriteLine(message);
        }
    }
}
#endif