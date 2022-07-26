# MMLogger

## About

A simple log output DLL.  
And it should be fast.  

## Format

- Debug  
  Only Contents (and IDs)  
  [{Contents}( | {ID} = {Value})...]  
- Info  
  Datetime can be output  
  [{Datetime} | {Contents}( | {ID} = {Value})...]  
- Warn, Error, Critical  
  Full infomation can be output  
  [{Datetime} | {Loglevel} | {Contents}( | {ID} = {Value})...]  

### Columns

- Datetime  
  The datetime when the log line output  
- Loglevel  
  Specified log level to the line  
- Contents  
  Free chars  
- ID, Value  
  Option for key infmations  
  Multiple of IDs can be set  

### Other

- Loglevel and its strings  
  5 levels has defined  
  Strings defined constant number of chars  
  1. Debug  
    [Debug   ]  
  1. Info  
    [Info    ]  
  1. Warn  
    [Warn    ]  
  1. Error  
    [Error   ]  
  1. Critical  
    [Critical]  
- Datetime format  
  [yyyy/MM/dd HH:mm:ss]  
- Delimiter  
  One space, Vertical bar, One space  
  [ | ]  

## How to use

Add reference **MMLogger**  
Set path  
Write!  
To write continuously, open and close manually  

### Settings

- LogFilePath  
  [**Must be set**]  
  MMLogger will output a log file of this path  
- FileEncoding  
  [*Default:SJIS*]  
  Log file encoding  
- OutLogLevel  
  [*Default:Info*]  
  Output log level  
  MMLogger outputs this level or higher  

### Methods

- Open()  
  For continuous use  
  Open a stream to write a lot of logs  
- Write(LogLevel, Contents, params IDs (string name, string value)[])  
  Write a log line  
  IDs can be omitted  
- Close()  
  For continuous use  
  Close and dispose the open stream  

### Sample

#### Setup

~~~csharp:setting
using MMLogger;
...
Logger.LogFilePath = @"c:\mm.log";
Logger.FileEncoding = Encoding.UTF8;
Logger.OutLogLevel = Logger.LogLevel.Debug;
~~~

#### Write

~~~csharp:write once
Logger.Write(Logger.LogLevel.Error, "Write once");
~~~

~~~csharp:write continuous
Logger.Open();
for (;;)
  Logger.Write(Logger.LogLevel.Info, "Write continuously");
Logger.Close();
~~~

~~~csharp:set IDs
// Named ValueTuple struct
(string name, string value)[] ids = 
{
  ("ID1", "Val1"),
  ("ID2", "Val2"),
};
Logger.Write(Logger.LogLevel.Error, "Set 2 IDs", ids);
~~~
