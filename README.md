# Logger
Logging information in .Net made easy
-------------------------------------

Don't want a Jumbo Jet tyre 3rd party solution that takes weeks to configure when you're only looking for a skateboard wheel?
Logger is a quick and easy lightweight alternative to Log4Net.

It is used in an almost identical fashion without the confusing learning curve.
That's because there's very little to configure and this makes it easy to implement.

It's extensible. The logger uses late binding dependency injection via interfaces so you can add your own interface without rebuilding the code.

Currently there are 4 interfaces or methods of logging.
- To the console (Logger.ConsoleLogger)
- To a file (Logger.FileLogger)
- To the Windows Event Logger (Logger.EventLogger)
- Send an email (Logger.EmailLogger)

Picking which one to log to is as easy as declaring them in your app.config file in the <appsettings> area.
> **Example line in app.config**
>
> ```xml
>    <add key="Logger" value="Logger.ConsoleLogger,Logger.FileLogger" />
> ```

In the case above, events will be logged to the File system and the console. Nothing is logged to the Event Viewer.

Now to the code:
For each class that will be logging, add the following 1 line.

> **Example line in your class**
>
>   "private static Logger.Logger _log = Logger.Log.GetInstance("appname");"

"appname" can be anything you desire. It will create a file in the local folder called "logname.log"

Logging actual data is the same as Log4Net with the addition of a debug member, which only dumps to Console and ignores writing to
other streams in NON debug builds.

> **Examples are**
>
> - _log.Debug(string)
> - _log.Info(string)
> - _log.Warn(string)
> - _log.Error(string)
> - _log.Error(string, Exception)
> - _log.Flush() - only applies to the EmailLogger. Flushes all previous logging into the body of an email and sends it.


Extending functionality
-----------------------

Create a class in the Logger namespace that implements the ILogger interface.

So for example you want to add email logging, you would add the full class name to your app.config file as such.

> **Email logging**
>
> ```xml
>    "<add key="Logger" value="Logger.ConsoleLogger,Logger.FileLogger,Logger.EmailLogger" />"
> ```
