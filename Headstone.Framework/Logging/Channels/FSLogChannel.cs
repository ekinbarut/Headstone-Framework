using Headstone.Framework.Configuration;
using Headstone.Framework.Models.Configuration;
using Headstone.Framework.Models.Logging;
using System;
using System.IO;

namespace Headstone.Framework.Logging.Channels
{
    public class FSLogChannel : ILogChannel
    {
        // Set the default log parameters
        private static string _logDirectoryPath = "Logs/";
        private static string _fatalErrorLog = "fatal.log";
        private static string _errorLog = "error.log";
        private static string _warningLog = "warning.log";
        private static string _infoLog = "info.log";
        private static string _debugLog = "debug.log";
        private static string _generalLog = "general.log";
        private static string _defaultLog = "default.log";

        // Create the calculated log directory
        private static DirectoryInfo _logDirectory = null;

        /// <summary>
        /// 
        /// </summary>
        public static string LogDirectoryPath
        {
            get
            {
                return _logDirectoryPath;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DirectoryInfo LogDirectory
        {
            get
            {
                return _logDirectory;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string FatalErrorLog
        {
            get
            {
                return _fatalErrorLog;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string ErrorLog
        {
            get
            {
                return _errorLog;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string WarningLog
        {
            get
            {
                return _warningLog;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string InfoLog
        {
            get
            {
                return _infoLog;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string DebugLog
        {
            get
            {
                return _debugLog;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string DefaultLog
        {
            get
            {
                return _defaultLog;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string GeneralLog
        {
            get
            {
                return _generalLog;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static FileInfo GetLogFile(string path)
        {
            // Check whether the file with the given path exists
            FileInfo fi = new FileInfo(path);

            // Check the log directory first and create if necessary
            if (!fi.Directory.Exists) fi.Directory.Create();

            // Check the log file
            if (!fi.Exists)
            {
                // Create the file
                fi.Create().Close();
            }

            return fi;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="text"></param>
        public static void Log(string path, string text)
        {
            // Get the log file
            FileInfo fi = GetLogFile(path);

            // Open the file for appending
            StreamWriter writer = fi.AppendText();

            // Add the text
            writer.WriteLine(DateTime.Now + " : " + text);

            // Close the writer
            writer.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        public void Log(LogRecord record)
        {
            // Check the log directory
            if (LogDirectory == null)
            {
                // If no log directory found, intialize!
                Initialize();
            }

            // Check the log mode and get the according log file
            FileInfo fi;

            switch (record.Level)
            {
                case "":
                case "NONE":
                    return;
                case "FATAL":
                    fi = GetLogFile(LogDirectory + FatalErrorLog);
                    break;
                case "ERROR":
                    fi = GetLogFile(LogDirectory + ErrorLog);
                    break;
                case "WARN":
                    fi = GetLogFile(LogDirectory + WarningLog);
                    break;
                case "INFO":
                    fi = GetLogFile(LogDirectory + InfoLog);
                    break;
                case "DEBUG":
                    fi = GetLogFile(LogDirectory + DebugLog);
                    break;
                default:
                    fi = GetLogFile(LogDirectory + DebugLog);
                    break;
            }

            // Open the file for appending
            StreamWriter writer = fi.AppendText();

            // Add the text
            if (record.Exception == null)
            {
                writer.WriteLine(DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:ffff") + " : "
                                            + record.Level + " - "
                                                + (!String.IsNullOrEmpty(record.Process) ? record.Process + " - " : "")
                                                    + record.Message);
            }
            else
            {
                writer.WriteLine(DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:ffff") + " : "
                                            + record.Level + " - "
                                                + (!String.IsNullOrEmpty(record.Process) ? record.Process + " - " : "")
                                                    + record.Message + "; Exception:" + record.Exception.Message + "; Stack Trace:" + record.Exception.StackTrace);
            }

            // Close the writer
            writer.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        private static void Initialize()
        {
            // Get the log parameters
            if (!String.IsNullOrEmpty(LoggingConfig.LogDirectory))
            {
                _logDirectoryPath = LoggingConfig.LogDirectory;
            }
            if (_logDirectoryPath.Substring(_logDirectoryPath.Length - 1, 1) != "/") _logDirectoryPath += "/";

            // Set the log directory
            _logDirectory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + _logDirectoryPath);

            // Get the new log directory path
            _logDirectoryPath = _logDirectory.FullName;
        }
    }
}
