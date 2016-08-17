using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Logger {
    public class FileLogger : IFileLogger
    {
        private string _appName = "";
        static string _buffer = "";

        public FileLogger(string appName)
        {
            _appName = appName;
        }

        public string Identity() {
            return "FileLogger";
        }

        public void Debug(string text)
        {
#if DEBUG
            // don't write debugging info in production builds
            WriteFile(Logger.TimeStamp + " DEBUG: " + text);
#endif
        }

        public void Info(string text)
        {
            WriteFile(Logger.TimeStamp + " INFO:  " + text);
        }

        public void Warn(string text)
        {
            WriteFile(Logger.TimeStamp + " WARN:  " + text);
        }

        public void Error(string text)
        {
            WriteFile(Logger.TimeStamp + " ERROR: " + text);
        }

        public void Error(string text, Exception ex)
        {

            if (ex.InnerException != null)
            {
                Exception inner = ex.InnerException;

                while (inner.InnerException != null)
                    inner = inner.InnerException;

                Error(inner.Message + Environment.NewLine + inner.StackTrace);
            }
            else
            {
                Error(text + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        public void Flush() {
            try {
                if (_buffer == "")
                    return;

                string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
                path = path.Replace("file:\\", "");
                string fileName = path + "\\" + _appName + ".log";

                FileInfo fileInfo = new FileInfo(fileName);
                int count = 0;

                while (IsFileLocked(fileInfo)) {
                    Thread.Sleep(2000);
                    count++;
                    if (count >= 30)
                        throw new Exception("File still locked after 1 minute, contents cannot be flushed");
                }

                using (TextWriter tw = new StreamWriter(fileName, true)) {
                    tw.WriteLine(_buffer);
                    tw.Close();
                    _buffer = "";
                }
            }
            catch (Exception ex) {
                Debug("Flushing Log:" + ex.Message);
                EventLog.WriteEntry(_appName, "Flush error", EventLogEntryType.Error);
            }
        }

        private void WriteFile(string text) {
            try {
                string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
                path = path.Replace("file:\\", "");
                using (TextWriter tw = new StreamWriter(path + "\\" + _appName + ".log", true)) {
                    tw.WriteLine(_buffer + text);
                    tw.Close();
                    _buffer = "";
                }
            }
            catch (Exception ex) {
                _buffer += text;
                Debug("Buffering Log: " + ex.Message);
                //Console.WriteLine("Buffering: " + ex.Message);
                EventLog.WriteEntry(_appName, text, EventLogEntryType.Error);
            }
        }

        protected virtual bool IsFileLocked(FileInfo file) {
            FileStream stream = null;

            try {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException) {
                //the file is unavailable because it is:
                //  still being written to
                //  or being processed by another thread
                //  or does not exist (has already been processed)
                return true;
            }
            finally {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }
    }
}
