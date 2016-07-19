using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;


namespace Logger {
    // high level class responsible for orchestrating the dependency injection from
    // declarations in the app.config file
    public class Log {
        static Logger _instance = null;

        protected Log () {}

        public static Logger GetInstance(string name) {
            if (_instance != null)
                return _instance;

            const string keyName = "logger";
            name = name.ToLower();

            if (name == "" || name == null) {
                name = keyName;
            }

            // get interfaces from app.config file
            List<ILogger> plugins = new List<ILogger>();
            NameValueCollection appSettings = new NameValueCollection();
            appSettings = ConfigurationManager.AppSettings;

            foreach (string key in appSettings.AllKeys) {
                if (key.ToLower() == (keyName)) {
                    string[] interfaces = appSettings[key].Split(',');
                    foreach (string pluginName in interfaces) {
                        Type t = Type.GetType(pluginName);
                        try {
                            var o = Activator.CreateInstance(t, name);
                            plugins.Add((ILogger)o);
                            Console.WriteLine("Found plugin " + pluginName);
                        }
                        catch (Exception ex) {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }

            // pass the interfaces to the logger class, which does all the work
            _instance = Logger.GetInstance(name, plugins.ToArray());

            Console.WriteLine("Log GetInstance() Init complete.");

            return _instance;
        }
    }
}
