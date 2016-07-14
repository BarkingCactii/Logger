using System;
using System.Diagnostics;

namespace Logger {
    public class EventLogger : IEventLogger {
        private string _appName = "";

        public EventLogger(string appName) {
            _appName = appName;
        }

        private static System.Diagnostics.EventLog eventLog = new System.Diagnostics.EventLog();

        public string Identity() {
            return "EventLogger";
        }

        public void Debug(string text) {
#if DEBUG
            // don't write debugging info in production builds
            EventLog.WriteEntry(_appName, text, EventLogEntryType.Information);
#endif
        }

        public void Info(string text) {
            EventLog.WriteEntry(_appName, text, EventLogEntryType.Information);
        }

        public void Warn(string text) {
            EventLog.WriteEntry(_appName, text, EventLogEntryType.Warning);
        }

        public void Error(string text) {
            EventLog.WriteEntry(_appName, text, EventLogEntryType.Error);
        }

        public void Error(string text, Exception ex) {
            if (ex.InnerException != null) {
                Exception inner = ex.InnerException;

                while (inner.InnerException != null)
                    inner = inner.InnerException;

                Error(inner.Message + Environment.NewLine + inner.StackTrace);
            } else {
                Error(text + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }
    }
}
