using Headstone.Framework.Models;
using Headstone.Framework.Models.Configuration;
using Headstone.Framework.Models.Contexts;
using Headstone.Framework.Models.Events;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
#if NETCOREAPP2_2
using Microsoft.Extensions.Configuration;
#endif

namespace Headstone.Framework.Configuration
{
    public class ConfigurationService
    {
        private static string _appKey = string.Empty;
        private static string _env = string.Empty;
        private static List<ConfigRecord> _records = null;
        private static List<ConfigRecord> _relatedRecords = null;

        #region [ Events ]

        public static event ConfigurationEventHandler ConfigurationLoaded;
        public static event ConfigurationEventHandler ConfigurationRefreshed;
        public static event ConfigurationEventHandler ConfigurationChanged;

        #endregion

#if NETCOREAPP2_2
        private static IConfiguration _configuration;
        public ConfigurationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
#endif

        public static string AppKey {
            get
            {
                if (String.IsNullOrEmpty(_appKey))
                {
#if NET452
                    // Get the app key from the config file
                    _appKey = ConfigurationManager.AppSettings["AppKey"];
#elif NETCOREAPP2_2
                    // Get the app key from the config file
                    _appKey = _configuration.GetValue<string>("AppKey");
#endif

                    if (String.IsNullOrEmpty(_appKey))
                    {
                        throw new ArgumentNullException("No valid app key found! Please add a valid AppKey to application settings section of the config file.");
                    }
                }

                return _appKey;
            }
        }

        public static string Environment {

            get
            {
                if (String.IsNullOrEmpty(_env))
                {
#if NET452
                    // Get the environment from the config file
                    _env = ConfigurationManager.AppSettings["Environment"];
#elif NETCOREAPP2_2
                    // Get the environment from the config file
                    _env = _configuration.GetValue<string>("Environment");
#endif

                    if (String.IsNullOrEmpty(_env))
                    {
                        throw new ArgumentNullException("No valid environment found! Please add a environment to application settings section of the config file.");
                    }
                }

                return _env;
            }
        }

        public static List<ConfigRecord> Records {
            get
            {
                if (_records == null)
                {
                    using (var db = new FrameworkDbContext())
                    {
                        // Get records
                        _records = db.SystemConfiguration.ToList();
                    }
                }

                return _records;
            }
        }

        public static List<ConfigRecord> RelatedRecords
        {
            get
            {
                if (_relatedRecords == null)
                {
                    using (var db = new FrameworkDbContext())
                    {
                        // Get records
                        _relatedRecords = db.SystemConfiguration.Where(r => r.AppKey == AppKey && r.Environment == Environment).ToList();
                    }
                }

                return _relatedRecords;
            }
        }

        public static void Load(bool isRefresh = false)
        {
            // Get the related records
            var relatedRecords = RelatedRecords;

            // Default type results
#pragma warning disable CS0219 // The variable 'intResult' is assigned but its value is never used
            int intResult = 0;
#pragma warning restore CS0219 // The variable 'intResult' is assigned but its value is never used
#pragma warning disable CS0219 // The variable 'decimalResult' is assigned but its value is never used
            decimal decimalResult = 0;
#pragma warning restore CS0219 // The variable 'decimalResult' is assigned but its value is never used
            TimeSpan timespanResult;

            // Set the records
            //#region [ Services ]

            //var securityModel = relatedRecords.FirstOrDefault(r => r.Class == "Services" && r.Property == "SecurityModel");
            //if (securityModel != null)
            //{
            //    switch (securityModel.Value)
            //    {
            //        case "KEY":
            //            ServiceConfig.SecurityModel = SecurityModel.Key;
            //            break;
            //        case "K&S":
            //        case "KEY_SECRET":
            //            ServiceConfig.SecurityModel = SecurityModel.Key_Secret;
            //            break;
            //        case "K&U&P":
            //        case "KEY_USERNAME_PASSWORD":
            //            ServiceConfig.SecurityModel = SecurityModel.Key_Username_Password;
            //            break;
            //        case "U&P":
            //        case "USERNAME_PASSWORD":
            //            ServiceConfig.SecurityModel = SecurityModel.Username_Password;
            //            break;
            //        case "ALL":
            //            ServiceConfig.SecurityModel = SecurityModel.All;
            //            break;
            //    }                
            //}

            //#endregion

            #region [ Logging ]

            var logChannel = relatedRecords.FirstOrDefault(r => r.Class == "Logging" && r.Property == "Channel");
            if (logChannel != null) LoggingConfig.Channel = logChannel.Value;

            var logMode = relatedRecords.FirstOrDefault(r => r.Class == "Logging" && r.Property == "Mode");
            if (logMode != null) LoggingConfig.Mode = logMode.Value;

            var logDirectory = relatedRecords.FirstOrDefault(r => r.Class == "Logging" && r.Property == "LogDirectory");
            if (logDirectory != null) LoggingConfig.LogDirectory = logDirectory.Value;

            var logBucket = relatedRecords.FirstOrDefault(r => r.Class == "Logging" && r.Property == "LogBucket");
            if (logBucket != null) LoggingConfig.LogBucket = logBucket.Value;

            #endregion

            //#region [ SaveActivity ]

            //var activityChannel = relatedRecords.FirstOrDefault(r => r.Class == "SaveActivity" && r.Property == "Channel");
            //if (activityChannel != null) ActivityConfig.Channel = activityChannel.Value;

            //var activityBucket = relatedRecords.FirstOrDefault(r => r.Class == "SaveActivity" && r.Property == "ActivityBucket");
            //if (activityBucket != null) ActivityConfig.ActivityBucket = activityBucket.Value;

            //#endregion

            #region [ Caching ]

            var cacheEnabled = relatedRecords.FirstOrDefault(r => r.Class == "Caching" && r.Property == "IsEnabled");
            if (cacheEnabled != null) CacheConfig.IsEnabled = (cacheEnabled.Value == "True" || cacheEnabled.Value == "1");

            var cacheProvider = relatedRecords.FirstOrDefault(r => r.Class == "Caching" && r.Property == "Provider");
            if (cacheProvider != null) CacheConfig.Provider = cacheProvider.Value;

            var cacheServer = relatedRecords.FirstOrDefault(r => r.Class == "Caching" && r.Property == "Server");
            if (cacheServer != null) CacheConfig.Server = cacheServer.Value;

            var cacheBucket = relatedRecords.FirstOrDefault(r => r.Class == "Caching" && r.Property == "CacheBucket");
            if (cacheBucket != null) CacheConfig.CacheBucket = cacheBucket.Value;

            var cacheDefaultExpirationTime = relatedRecords.FirstOrDefault(r => r.Class == "Caching" && r.Property == "DefaultExpirationTime");
            if (cacheDefaultExpirationTime != null) CacheConfig.DefaultExpirationTime = TimeSpan.TryParse(cacheDefaultExpirationTime.Value, out timespanResult) ? timespanResult : TimeSpan.MinValue;

            #endregion

            #region [ Security ]

            var salt = relatedRecords.FirstOrDefault(r => r.Class == "Security" && r.Property == "Salt");
            if (salt != null) SecurityConfig.Salt = salt.Value;

            var encryption = relatedRecords.FirstOrDefault(r => r.Class == "Security" && r.Property == "Encryption");
            if (encryption != null) SecurityConfig.Encryption = encryption.Value;

            #endregion

            #region [ Session ]

            var sessionChannel = relatedRecords.FirstOrDefault(r => r.Class == "Session" && r.Property == "Channel");
            if (sessionChannel != null) SessionConfig.Channel = sessionChannel.Value;

            var sessionBucket = relatedRecords.FirstOrDefault(r => r.Class == "Session" && r.Property == "SessionBucket");
            if (sessionBucket != null) SessionConfig.SessionBucket = sessionBucket.Value;

            #endregion

            //#region [ Contexts ]

            //var contextChannel = relatedRecords.FirstOrDefault(r => r.Class == "Context" && r.Property == "Channel");
            //if (contextChannel != null) ContextConfig.Channel = contextChannel.Value;

            //var contextBucket = relatedRecords.FirstOrDefault(r => r.Class == "Context" && r.Property == "ContextBucket");
            //if (contextBucket != null) ContextConfig.ContextBucket = contextBucket.Value;

            //#endregion

            //#region [ Commerce ]

            //var maxPackagePurchaseLimit = relatedRecords.FirstOrDefault(r => r.Class == "Commerce" && r.Property == "maxPackagePurchaseLimit");
            //if (maxPackagePurchaseLimit != null) CommerceConfig.MaxPackagePurchaseLimit = Int32.TryParse(maxPackagePurchaseLimit.Value, out intResult) ? intResult : 0;

            //var collectorUri = relatedRecords.FirstOrDefault(r => r.Class == "Commerce" && r.Property == "HeadstoneCollectorURI");
            //if (collectorUri != null) CommerceConfig.HeadstoneCollectorURI = collectorUri.Value;

            //var collectorKey = relatedRecords.FirstOrDefault(r => r.Class == "Commerce" && r.Property == "HeadstoneCollectorKey");
            //if (collectorKey != null) CommerceConfig.HeadstoneCollectorKey = collectorKey.Value;

            //#endregion

            //#region [ Charging ]

            //var defaultInstrumentId = relatedRecords.FirstOrDefault(r => r.Class == "Charging" && r.Property == "DefaultInstrumentID");
            //if (defaultInstrumentId != null) ChargingConfig.DefaultInstrumentId = Int32.TryParse(defaultInstrumentId.Value, out intResult) ? intResult : 0;

            //var paymentUser = relatedRecords.FirstOrDefault(r => r.Class == "Charging" && r.Property == "PaymentUser");
            //if (paymentUser != null) ChargingConfig.PaymentUser = paymentUser.Value;

            //var paymentPassword = relatedRecords.FirstOrDefault(r => r.Class == "Charging" && r.Property == "PaymentPassword");
            //if (paymentPassword != null) ChargingConfig.PaymentPassword = paymentPassword.Value;

            //#endregion

            // Raise related event
            if (!isRefresh){
                RaiseConfigurationEvents(ConfigurationEventTypes.Loaded, Records);
            }
            else
            {
                RaiseConfigurationEvents(ConfigurationEventTypes.Refreshed, Records);
            }
        }

        public static void Refresh()
        {
            // Reset the records
            _appKey = string.Empty;
            _env = string.Empty;
            _records = null;
            _relatedRecords = null;

            // Re-load records
            Load(true);
        }

        #region [ Event triggers ]

        public static void RaiseConfigurationEvents(ConfigurationEventTypes eventType, List<ConfigRecord> records)
        {
            // Raise the item related event
            switch (eventType)
            {
                case ConfigurationEventTypes.Loaded:
                    if (ConfigurationLoaded != null)
                    {
                        ConfigurationLoaded(new ConfigurationEventArgs(eventType, records));
                    }
                    break;

                case ConfigurationEventTypes.Refreshed:
                    if (ConfigurationRefreshed != null)
                    {
                        ConfigurationRefreshed(new ConfigurationEventArgs(eventType, records));
                    }
                    break;
            }

            // Raise context level basket update event
            if (ConfigurationChanged != null)
            {
                ConfigurationChanged(new ConfigurationEventArgs(eventType, records));
            }
        }

        #endregion
    }
}
