using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Stub {
    class Program {
        private static Logger.Logger _log = Logger.Log.GetInstance("appname");

        static void Main(string[] args) {
            _log.Debug("Debug message");
            _log.Info("Info message");
            _log.Warn("Warn message");
            _log.Error("Error message");
            _log.Flush();
            Console.WriteLine("Done");
        }
    }
}
