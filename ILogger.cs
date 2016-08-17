using System;

namespace Logger {
    public interface ILogger
    {
        string Identity();

        void Debug(string text);

        void Info(string text);

        void Warn(string text);

        void Error(string text);

        void Error(string text, Exception ex);

        void Flush();
    }
}
