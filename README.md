# Logger
Logging information in .Net
---------------------------

Don't want a Jumbo Jet tyre 3rd party solution that takes weeks to configure when you're looking for a skateboard wheel?
Logger is a quick and easy lightweight alternative to Log4Net.

It is used in an almost identical fashion without the confusing learning curve.
Thats because there's very little to configure and it's easy to implement.

It's extensible. The logging is written uses late binding dependecy injection via interfaces so you can add your own interface without rebuilding the code.

Currently there are 3 interfaces or methods of logging.
- To the console
- To a file
- To the Windows Event Logger

Picking which one to log to is as easy as declaring them in your app.config file in the <appsettings> area.
> ** Example line in app.config **
    <add key="Logger" value="Logger.ConsoleLogger,Logger.FileLogger" />
    
In the case above, events will be logged to the File system and the console. Nothing is logged to the Event Viewer.

Now to the code:
For each class that will be logging, add the following 1 line.

> **Example line in your class**
>   private static Logger.Logger _log = Logger.Log.GetInstance("appname");

"logname" can be anything you desire. It will create a file in the local folder called "logname.log"

Logging actual data is the same as Log4Net with the addition of a debug member, which only dumps to Console and ignores writing to
other streams in NON debug builds.

> **Examples are**
> -_log.Debug(string)
> -_log.Info(string)
> -_log.Warn(string)
> -_log.Error(string)
> -_log.Error(string, Exception)








