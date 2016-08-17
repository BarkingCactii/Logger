using System;

namespace Logger {

    // singleton class
    public class Logger : IEventLogger, IFileLogger, ILogger
    {
        static Logger _instance;
        static string _appName = "";
        static ILogger [] _plugins;

        // prevent construction
        protected Logger() { }

        internal static Logger GetInstance(string appName, ILogger [] plugins) {
            _appName = appName;
            _plugins = plugins;
            if ( _instance == null ) {
                _instance = new Logger();
            }
            return _instance;
        }
        
        public static string TimeStamp
        {
            get { return DateTime.Now.ToString("dd/MM/yy HH:mm:ss"); }
        }

        public string Identity() {
            return "Logger";
        }

        public void Debug(string text)
        {
            foreach (ILogger plugin in _plugins) {
                plugin.Debug(text);
            }
        }

        public void Info(string text)
        {
            foreach (ILogger plugin in _plugins) {
                plugin.Info(text);
            }
        }

        public void Warn(string text)
        {
            foreach (ILogger plugin in _plugins) {
                plugin.Warn(text);
            }
        }

        public void Error(string text)
        {
            foreach (ILogger plugin in _plugins) {
                plugin.Error(text);
            }
        }

        public void Error(string text, Exception ex)
        {
            foreach (ILogger plugin in _plugins) {
                plugin.Error(text, ex);
            }
        }

        public void Flush() {
            foreach (ILogger plugin in _plugins) {
                plugin.Flush();
            }
        }

    }
}
