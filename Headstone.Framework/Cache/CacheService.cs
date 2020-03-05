using System;
using System.Collections.Generic;
using System.Configuration;
using Headstone.Framework.Cache.Channels;
using Headstone.Framework.Configuration;
using Headstone.Framework.Logging;
using Headstone.Framework.Models;
using Headstone.Framework.Models.Caching;
using Headstone.Framework.Models.Configuration;
using Headstone.Framework.Models.Events;
using Headstone.Framework.Models.Logging;
#if NETCOREAPP2_2
using Microsoft.Extensions.Configuration;
#endif

namespace Headstone.Framework.Caching
{
    public class CacheService
    {
        #region [ Private Member(s) ]

        private static ICacheChannel _activeChannel = null;
        private static bool _isEnabled = false;
        private static string _server = string.Empty;
        private static string _bucket = string.Empty;
        private static TimeSpan _defaultExpirationTime = default(TimeSpan);
        private static readonly object _lockObject = new object();

#if NETCOREAPP2_2
        private static IConfiguration _configuration;
        public CacheService(IConfiguration configuration)
        {
            // Add the configuration change handler
            ConfigurationService.ConfigurationChanged += ConfigurationChanged;
            _configuration = configuration;
        }
#endif

        public static ICacheChannel CacheChannel
        {
            get
            {
                if (_activeChannel == null)
                {
                    switch (CacheConfig.Provider)
                    {
                        case "CB":
                        case "RDS":
                        case "REDIS":
                            _activeChannel = RedisCacheChannel.Instance;
                            break;
                        case "PRC":
                        case "INPROC":
                        case "INPROCESS":
                            _activeChannel = InProcessCacheChannel.Instance;
                            break;
                        default:
                            _activeChannel = InProcessCacheChannel.Instance;
                            break;
                    }

                    if (_activeChannel == null) // If the channel is stil not initialized
                    {
                        throw new ArgumentNullException("No valid cache channel found!");
                    }

                }

                return _activeChannel;
            }
        }

        public static object Get<T>(object cacheKeys)
        {
            throw new NotImplementedException();
        }

        public static TimeSpan DefaultExpirationTime{
            get
            {
                if(_defaultExpirationTime == default(TimeSpan))
                {
                    _defaultExpirationTime = CacheConfig.DefaultExpirationTime;
                }

                return _defaultExpirationTime;
            }
        }

        public static string Server
        {
            get
            {
                if (String.IsNullOrEmpty(_server))
                {
                    _server = CacheConfig.Server;
                }

                return _server;
            }
        }

        public static string Bucket
        {
            get
            {
                if (String.IsNullOrEmpty(_bucket))
                {
                    _bucket = CacheConfig.CacheBucket;
                }

                return _bucket;
            }
        }

        private static bool Enabled
        {
            get
            {
                return CacheConfig.IsEnabled;
            }
        }
        
        static CacheService()
        {
            // Add the configuration change handler
            ConfigurationService.ConfigurationChanged += ConfigurationChanged;
        }

        static void ConfigurationChanged(ConfigurationEventArgs args)
        {
            // Empty the default values
            _activeChannel = null;
            _isEnabled = false;
            _server = string.Empty;
            _bucket = string.Empty;
            _defaultExpirationTime = default(TimeSpan);
        }

        public static void Setup(ICacheChannel channel, bool isEnabled, TimeSpan defaultExpirationTime, string bucket, string server)
        {
            _activeChannel = channel;
            _isEnabled = isEnabled;
            _server = server;
            _bucket = bucket;
            _defaultExpirationTime = defaultExpirationTime;
        }

        #endregion

        #region [ Cache key handling ]

        public static string ModifyKey(string key)
        {
#if NET452
            // Create the extra key
            string extraKey = ConfigurationManager.AppSettings["AppKey"] + "." + ConfigurationManager.AppSettings["Environment"] ?? string.Empty;
#elif NETCOREAPP2_2
            // Create the extra key
            string extraKey = _configuration.GetValue<string>("AppKey") + "." + _configuration.GetValue<string>("Environment") ?? string.Empty;
#endif


            // Add the extra key
            if (!key.Contains(extraKey)) key = extraKey + "." +key;

            return key;
        }

        private static void AddCacheKeyToCollection(string cacheKey)
        {
            // Get current cache keys
            List<string> allCacheKeys = CacheChannel.Get<List<string>>("AllCacheKeys");
            if (allCacheKeys == null)
            {
                allCacheKeys = new List<string>();
            }

            // Check the key existence
            var key = ModifyKey(cacheKey);

            //if (key.Length > 150)
            //{
            //    key = SecurityService.Encrypt(key, EncryptorType.MD5Encryption);
            //}

            if (!allCacheKeys.Contains(key))
            {
                allCacheKeys.Add(key);
                CacheChannel.Set("AllCacheKeys", allCacheKeys);
            }
        }

        public static List<string> GetAllCacheKeys()
        {
            return CacheChannel.Get<List<string>>("AllCacheKeys");
        }

        #endregion

        public static void Add(string cacheKey, object data)
        {
            if (!Enabled)
            {
                return;
            }

            // Modify the key
            cacheKey = ModifyKey(cacheKey);

            try
            {
                // Add the cache key
                AddCacheKeyToCollection(cacheKey);

                // Set the expiration time
                TimeSpan expiration = CacheConfig.DefaultExpirationTime;

                // Insert the cache item
                CacheChannel.Set(cacheKey, data, expiration);
            }
            catch (Exception exc)
            {
                // Log the error
                LogService.Log(LogMode.Error, "Can not insert the item into the cache", exc, data:cacheKey);
            }
        }

        public static void Add(string cacheKey, object data, TimeSpan expiration)
        {
            if (!Enabled)
            {
                return;
            }

            // Modify the key
            cacheKey = ModifyKey(cacheKey);

            try
            {
                // Add the cache key
                AddCacheKeyToCollection(cacheKey);

                // Insert the cache item
                CacheChannel.Set(cacheKey, data, expiration);
            }
            catch (Exception exc)
            {
                // Log the error
                LogService.Log(LogMode.Error, "Can not insert the item into the cache", exc, data: cacheKey);
            }
        }

        public static void Remove(string cacheKey)
        {
            if (!Enabled)
            {
                return;
            }

            // Modify the key
            cacheKey = ModifyKey(cacheKey);

            try
            {
                // Remove the cache item
                CacheChannel.Remove(cacheKey);
            }
            catch (Exception exc)
            {
                LogService.Log(LogMode.Error, "Can not remove the cache item", exc, cacheKey);
            }
        }

        #region [ Sync Get(s) ]

        public static T Get<T>(string cacheKey, Func<T> getData, bool loadFromCache = true) where T : class
        {
            // Modify the key 
            cacheKey = ModifyKey(cacheKey);

            T data = null;

            if (loadFromCache && Enabled)
            {
                // Get the data
                data = CacheChannel.Get<T>(cacheKey);
            }

            if (data == null)
            {
                // Get data using the given function
                data = getData();
            }

            // Check for the data
            if (data != null)
            {
                // Add the data to cache
                Add(cacheKey, data);
            }
            return data;
        }

        public static T Get<T>(string cacheKey, Func<T> getData, TimeSpan expiration, bool loadFromCache = true) where T : class
        {
            // Modify the key 
            cacheKey = ModifyKey(cacheKey);

            T data = null;

            if (loadFromCache && Enabled)
            {
                // Get the data
                data = CacheChannel.Get<T>(cacheKey);
            }

            if (data == null)
            {
                // Get data using the given function
                data = getData();
            }

            // Check for the data
            if (data != null)
            {
                // Add the data to cache
                Add(cacheKey, data, expiration);
            }
            return data;

        }

        #endregion
    }
}
