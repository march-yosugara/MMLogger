using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MMLogger
{
    /// <summary>
    /// Simple Logging Class
    /// </summary>
    public static class MMLogger
    {
        #region Logger settings
        /// <summary>
        /// Log file path
        /// </summary>
        public static string LogFilePath = string.Empty;
        /// <summary>
        /// Log file encode
        /// </summary>
        public static Encoding _enc = Encoding.GetEncoding(932);
        /// <summary>
        /// Log file writer
        /// </summary>
        public static StreamWriter _writer;
        /// <summary>
        /// Output log level
        /// </summary>
        /// <remarks>Logger outputs this level or higher</remarks>
        public static LogLevel OutLogLevel = LogLevel.Info;
        #endregion

        #region Constants
        /// <summary>
        /// Log level of Logger
        /// </summary>
        public enum LogLevel
        {
            /// <summary>
            /// Lowest level for debug log
            /// </summary>
            /// <remarks>Output only specified string</remarks>
            Debug = 0,
            /// <summary>
            /// For output informations
            /// </summary>
            /// <remarks>Output datetime and specified string</remarks>
            Info = 1,
            /// <summary>
            /// Warning level
            /// </summary>
            /// <remarks>Output datetime, loglevel and specified string</remarks>
            Warn = 2,
            /// <summary>
            /// Error level
            /// </summary>
            /// <remarks>Output datetime, loglevel and specified string</remarks>
            Error = 3,
            /// <summary>
            /// Critical error level
            /// </summary>
            /// <remarks>Output datetime, loglevel and specified string</remarks>
            Critical = 4,
        }

        /// <summary>
        /// Log level strings
        /// </summary>
        public static readonly Dictionary<LogLevel, string> LevelString = new Dictionary<LogLevel, string>
        {
            {LogLevel.Debug,    "Debug   "},
            {LogLevel.Info,     "Info    "},
            {LogLevel.Warn,     "Warning "},
            {LogLevel.Error,    "Error   "},
            {LogLevel.Critical, "Critical"},
        };

        /// <summary>
        /// Delimiter of columns(" | ")
        /// </summary>
        public const string LOG_CONTENT_SPLIT = " | ";
        /// <summary>
        /// Format of datetime("yyyy/MM/dd HH:mm:ss")
        /// </summary>
        public const string LOG_FORMAT_DATE = "yyyy/MM/dd HH:mm:ss";
        /// <summary>
        /// Log format 2 columns
        /// </summary>
        public const string LOG_FORMAT02 = "{0}" + LOG_CONTENT_SPLIT + "{1}" + LOG_CONTENT_SPLIT + "{2}";
        /// <summary>
        /// Log format 3 columns
        /// </summary>
        public const string LOG_FORMAT03 = "{0}" + LOG_CONTENT_SPLIT + "{1}" + LOG_CONTENT_SPLIT + "{2}";
        /// <summary>
        /// Log format for IDs
        /// </summary>
        public const string LOG_FORMAT04 = LOG_CONTENT_SPLIT + "{0} = {1}";
        #endregion

        #region Public methods
        /// <summary>
        /// Open for continuous use
        /// </summary>
        public static void Open()
        {
            if (_writer is null)
                _writer = new StreamWriter(LogFilePath, true, _enc);
        }

        /// <summary>
        /// Write log line
        /// </summary>
        /// <param name="level">LogLevel</param>
        /// <param name="contents">Log strings</param>
        /// <param name="lstID">Optional:Ids</param>
        public static void Write(LogLevel level, string contents, params (string name, string value)[] lstID)
        {
            if (OutLogLevel > level)
                return;

            var logline = MakeLogLine(level, contents, lstID);
            
            if (_writer is null)
            {
                using (var sw = new StreamWriter(LogFilePath, true, _enc))
                    sw.Write(logline);
            }
            else
            {
                _writer.Write(logline);
            }
        }

        /// <summary>
        /// Close for continuous use
        /// </summary>
        public static void Close()
        {
            _writer.Dispose();
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Make log line
        /// </summary>
        /// <param name="level">LogLevel</param>
        /// <param name="contents">Log strings</param>
        /// <param name="lstID">Optional:Ids</param>
        /// <returns>LogLine</returns>
        private static string MakeLogLine(LogLevel level, string contents, params (string name, string value)[] lstID)
        {
            var logline = string.Empty;

            // Main strings
            switch (level)
            {
                case LogLevel.Debug:
                    logline = contents;
                    break;
                case LogLevel.Info:
                    logline = string.Format(LOG_FORMAT02, DateTime.Now.ToString(LOG_FORMAT_DATE), contents);
                    break;
                case LogLevel.Warn:
                case LogLevel.Error:
                case LogLevel.Critical:
                    logline = string.Format(LOG_FORMAT03, DateTime.Now.ToString(LOG_FORMAT_DATE), LevelString[level], contents);
                    break;
            }

            // Add IDs
            foreach (var (name, value) in lstID)
                logline += string.Format(LOG_FORMAT04, name, value);

            // End of line
            logline += Environment.NewLine;

            return logline;
        }
        #endregion
    }
}
