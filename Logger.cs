using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Configuration;

namespace Logger
{
    // singleton class
    public class Logger : IEventLogger, IFileLogger, ILogger
    {
        private static Logger _instance;

        private string _appName = "";
        IFileLogger _IFileLogger;
        IEventLogger _IEventLogger;
        IConsoleLogger _IConsoleLogger;

        // prevent construction
        protected Logger() { }

        public static Logger GetSingleton(string appName) {
            if ( _instance == null ) {
                _instance = new Logger();
                _instance.Init(appName);
            }
            return _instance;
        }

        protected void Init(string appName)
        {
            _instance._appName = appName;
            _instance._IFileLogger = new FileLogger(appName);
            _instance._IEventLogger = new EventLogger(appName);
            _instance._IConsoleLogger = new ConsoleLogger(appName);

            string keyName = _instance._IFileLogger.Identity();
            string val = ConfigurationManager.AppSettings[keyName].ToLower();
            if (val != "enable")
                _instance._IFileLogger = null;

            keyName = _instance._IEventLogger.Identity();
            val = ConfigurationManager.AppSettings[keyName].ToLower();
            if (val != "enable")
                _instance._IEventLogger = null;

            keyName = _instance._IConsoleLogger.Identity();
            val = ConfigurationManager.AppSettings[keyName].ToLower();
            if (val != "enable")
                _instance._IEventLogger = null;

            Console.WriteLine("Logging Engine Started.");
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
            if (_instance._IConsoleLogger != null) _instance._IConsoleLogger.Debug(text);
            if (_instance._IFileLogger != null) _instance._IFileLogger.Debug(text);
            if (_instance._IEventLogger != null) _instance._IEventLogger.Debug(text);
        }

        public void Info(string text)
        {
            if (_instance._IConsoleLogger != null) _instance._IConsoleLogger.Info(text);
            if (_instance._IFileLogger != null) _instance._IFileLogger.Info(text);
            if (_instance._IEventLogger != null) _instance._IEventLogger.Info(text);
        }

        public void Warn(string text)
        {
            if (_instance._IConsoleLogger != null) _instance._IConsoleLogger.Warn(text);
            if (_instance._IFileLogger != null) _instance._IFileLogger.Warn(text);
            if (_instance._IEventLogger != null) _instance._IEventLogger.Warn(text);
        }

        public void Error(string text)
        {
            if (_instance._IConsoleLogger != null) _instance._IConsoleLogger.Error(text);
            if (_instance._IFileLogger != null) _instance._IFileLogger.Error(text);
            if (_instance._IEventLogger != null) _instance._IEventLogger.Error(text);
        }

        public void Error(string text, Exception ex)
        {
            if (_instance._IConsoleLogger != null) _instance._IConsoleLogger.Error(text, ex);
            if (_instance._IFileLogger != null) _instance._IFileLogger.Error(text, ex);
            if (_instance._IEventLogger != null) _instance._IEventLogger.Error(text, ex);
        }
    }
}
