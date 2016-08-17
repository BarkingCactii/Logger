using System;

namespace Logger {
    public class ConsoleLogger : IConsoleLogger
    {
        private string _appName = "";

        public ConsoleLogger(string appName)
        {
            _appName = appName;
        }

        public string TimeStamp
        {
            get { return DateTime.Now.ToString("dd/MM/yy HH:mm:ss"); }
        }

        public string Identity() {
            return "ConsoleLogger";
        }

        public void Debug(string text)
        {
            Console.WriteLine("DEBUG: " + text);
        }

        public void Info(string text)
        {
            Console.WriteLine("INFO:  " + text);
        }

        public void Warn(string text)
        {
            Console.WriteLine("WARN:  " + text);
        }

        public void Error(string text)
        {
            Console.WriteLine("ERROR: " + text);
        }

        public void Error(string text, Exception ex)
        {
            if (ex.InnerException != null)
            {
                Exception inner = ex.InnerException;

                while (inner.InnerException != null)
                    inner = inner.InnerException;

                Console.WriteLine(inner.Message);
                Error(inner.Message + Environment.NewLine + inner.StackTrace);
            }
            else
            {
                Console.WriteLine(ex.Message);
                Error(text + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        public void Flush () {
        }
    }
}
