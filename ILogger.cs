using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logger
{
    public interface ILogger
    {
        string Identity();

        void Debug(string text);

        void Info(string text);

        void Warn(string text);

        void Error(string text);

        void Error(string text, Exception ex);
    }

    public interface IFileLogger : ILogger {
    }

    public interface IEventLogger : ILogger {
    }

    public interface IConsoleLogger : ILogger {
    }
}
