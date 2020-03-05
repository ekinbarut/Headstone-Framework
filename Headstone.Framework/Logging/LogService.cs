using Newtonsoft.Json;
using System;
using Headstone.Framework.Configuration;
using Headstone.Framework.Logging.Channels;
using Headstone.Framework.Models;
using Headstone.Framework.Models.Configuration;
using Headstone.Framework.Models.Logging;
#if NET452
using System.Configuration;
using System.Web;
#elif NETCOREAPP2_2
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
#endif

namespace Headstone.Framework.Logging
{
    public class LogService
    {
        private static ILogChannel _activeChannel = null;
        private static LogMode _logMode = LogMode.Error;
        private static long lastTick = 0;
        private static object idGenLock = new Object();   

        public static ILogChannel LogChannel
        {
            get
            {
                if (_activeChannel == null)
                {
                    switch (LoggingConfig.Channel)
                    {
                        case "DB":
                        case "LOGDB":
                            _activeChannel = new DbLogChannel();
                            break;
                        case "FS":
                        case "BASICFS":
                        case "FILESYSTEM":
                            _activeChannel = new FSLogChannel();
                            break;
                        case "ES":
                        case "ELASTIC":
                        case "ELASTICSEARCH":
                            _activeChannel = new ElasticsearchLogChannel();
                            break;
                        case "LGL":
                        case "LOGGLY":
                            _activeChannel = new LogglyLogChannel();
                            break;
                    }

                    if (_activeChannel == null) // If the channel is stil not initialized
                    {
                        throw new ArgumentNullException("No valid log channel found!");
                    }

                }

                return _activeChannel;
            }
        }

        public static LogMode Mode
        {
            get
            {
                switch (LoggingConfig.Mode)
                {
                    case "ALL":
                        _logMode = LogMode.All;
                        break;
                    case "DEBUG":
                        _logMode = LogMode.Debug;
                        break;
                    case "INFO":
                        _logMode = LogMode.Info;
                        break;
                    case "WARN":
                        _logMode = LogMode.Warn;
                        break;
                    case "ERROR":
                        _logMode = LogMode.Error;
                        break;
                    case "FATAL":
                        _logMode = LogMode.Fatal;
                        break;
                    case "NONE":
                        _logMode = LogMode.None;
                        break;
                }

                return _logMode;
            }
        }

#if NET452

        static LogService()
        {
            // Add the configuration change handler
            ConfigurationService.ConfigurationChanged += ConfigurationChanged;
        }

#elif NETCOREAPP2_2

        private static IHttpContextAccessor _httpContextAccessor;
        private static IConfiguration _configuration;
        LogService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            // Add the configuration change handler
            ConfigurationService.ConfigurationChanged += ConfigurationChanged;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }
#endif

        static void ConfigurationChanged(Models.Events.ConfigurationEventArgs args)
        {
            // Empty the default values
            _activeChannel = null;
            _logMode = LogMode.Error;
        }

        public static void Setup(ILogChannel channel, LogMode mode)
        {
            _activeChannel = channel;
            _logMode = mode;
        }

        public static void Debug(string message)
        {
            Log(LogMode.Debug, message, null, string.Empty, null);
        }       

        public static void Info(string message)
        {
            Log(LogMode.Info, message, null, string.Empty, null);
        }

        public static void Warn(string message)
        {
            Log(LogMode.Warn, message, null, string.Empty, null);
        }

        public static void Error(string message)
        {
            Log(LogMode.Error, message, null, string.Empty, null);
        }

        public static void Fatal(string message)
        {
            Log(LogMode.Fatal, message, null, string.Empty, null);
        }
        
        public static void Log(LogMode mode, string message, Exception ex, string process = "", object data = null)
        {
            // Check for an active log channel
            if (LogChannel == null)
            {
                throw new ArgumentNullException("No active log channel! Please enter a valid channel name in to the system configuration table!");
            }

            // Check the log level
            if (mode < Mode) return;            

            // Create the log entry
            LogRecord logRecord = new LogRecord
            {
                LogID = GetNextId().ToString(),               
                Process = process,
                Host = Environment.MachineName,
                Level = mode.ToString(),
                Message = message,
                Exception = ex,
                ExceptionString = ex == null ? string.Empty : ex.ToString(),
                Data = data,
                DataString = data == null ? string.Empty : JsonConvert.SerializeObject(data),
                Created = DateTime.UtcNow,
                TimeStamp = DateTime.UtcNow
            };

#if NET452

            // Set the app key and environment
            logRecord.AppKey = ConfigurationManager.AppSettings["AppKey"];
            logRecord.Environment = ConfigurationManager.AppSettings["Environment"];

            // Try to get the host ip
            try
            {
                if (HttpContext.Current != null && HttpContext.Current.Request != null && !string.IsNullOrEmpty(HttpContext.Current.Request.UserHostAddress))
                {
                    // Set the host ip
                    logRecord.HostIP = HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"];
                }
            }
            catch { }

#elif NETCOREAPP2_2

            // Set the app key and environment
            logRecord.AppKey = _configuration.GetValue<string>("AppKey");
            logRecord.Environment = _configuration.GetValue<string>("Environment");

            // Try to get the host ip
            try
            {
                if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.Request != null && !string.IsNullOrEmpty(_httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString()))
                {
                    // Set the host ip
                    logRecord.HostIP = _httpContextAccessor.HttpContext.Connection.LocalIpAddress.ToString();
                }
            }
            catch { }            

#endif
            // Log with the default channel
            LogChannel.Log(logRecord);

        }

        public static void Log(LogRecord record)
        {
            // Check for an active channel
            if (LogChannel == null)
            {
                throw new ArgumentNullException("No log channel! Please enter a valid channel name in to the system configuration table!");
            }

#if NET452

            // Set the app key and environment
            record.AppKey = ConfigurationManager.AppSettings["AppKey"];
            record.Environment = ConfigurationManager.AppSettings["Environment"];

            // Try to get the host ip
            try
            {
                if (HttpContext.Current != null && HttpContext.Current.Request != null && !string.IsNullOrEmpty(HttpContext.Current.Request.UserHostAddress))
                {
                    // Set the host ip
                    record.HostIP = HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"];
                }
            }
            catch { }

#elif NETCOREAPP2_2

            // Set the app key and environment
            record.AppKey = _configuration.GetValue<string>("AppKey");
            record.Environment = _configuration.GetValue<string>("Environment");

            // Try to get the host ip
            try
            {
                if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.Request != null && !string.IsNullOrEmpty(_httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString()))
                {
                    // Set the host ip
                    record.HostIP = _httpContextAccessor.HttpContext.Connection.LocalIpAddress.ToString();
                }
            }
            catch { }

#endif
            // Check the activity id
            if (String.IsNullOrEmpty(record.LogID))
            {
                record.LogID = GetNextId().ToString();
            }

            // Check the host name
            if (String.IsNullOrEmpty(record.Host))
            {
                record.Host = Environment.MachineName;
            }

            // Check for an incoming exception
            if (record.Exception != null)
            {
                record.ExceptionString = record.Exception.ToString();
            }

            // Check for an incoming data object
            if (record.Data != null)
            {
                record.Data = JsonConvert.SerializeObject(record.Data);
            }

            // Override the time stamps
            record.Created = DateTime.UtcNow;
            record.TimeStamp = DateTime.UtcNow;            

            // Add the log record
            LogChannel.Log(record);
        }

        /// <summary>
        /// Generates a unique id for the log entry
        /// </summary>
        /// <returns></returns>
        private static long GetNextId()
        {
            // Make it thread safe
            lock (idGenLock)
            {
                // Get the tick count
                long tick = DateTime.UtcNow.Ticks;

                // Compare it against the previous one
                if (lastTick == tick)
                {
                    // If same make it bigger
                    tick = lastTick + 1;
                }

                // Set this one as actual
                lastTick = tick;

                // Return the tick count as id
                return tick;
            }
        }
    }
}
