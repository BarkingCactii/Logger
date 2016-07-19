# Logger
Logging information in .Net
Don't want a Jumbo Jet tyre solution when you're looking for a skateboard wheel?
This is a lightweight alternative to Log4Net.

It is used in an almost identical fashion without the confusing learning corve on hot to configure it.
Thats because theres very littoe to configure. It's dont that way to make it easy to implement.

It's extensible. The logging is written using interfaces so feel free to add your own interface.
Currently there are 3 interfaces or methods of logging.
1. To the console
2. To a file
3. To the Windows Event Logger

Picking which one to log to is as easy as declaring them in your app.config file in the <appsettings> area.
For example
    <add key="FileLogger" value="Enable" />
    <add key="EventLogger" value="Disable" />
    <add key="ConsoleLogger" value="Enable" />
    
In the case above, events will be logged to the File system and the console. Nothing is logged to the Event Viewer.

Now to the code:
For each class that will be logging, add the following 1 line.
        private static readonly Logger.Logger _log = Logger.Logger.GetSingleton("logname");

"logname" can be anything you desire. It will create a file in the local folder called "logname.log"

Logging actual data is the same as Log4Net with the addition of a debug member, which only dumps to Console and ignores writing to
other streams in NON debug builds.

Examples are
_log.Debug(string)
_log.Info(string)
_log.Warn(string)
_log.Error(string)
_log.Error(string, Exception)

If the code is multithreaded, there is some support for buffering messages until the log file is unlocked, but it is *not* bulletproof.
If you are going to use this with hardcore multi-threading processes you will have to add locks to the source code.






