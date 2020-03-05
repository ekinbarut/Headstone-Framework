using Headstone.Framework.Caching;
using System;
using Headstone.Framework.Models.Caching;
#if NET452
using System.Runtime.Caching;
#elif NETCOREAPP2_2
using Microsoft.Extensions.Caching.Memory;
#endif

namespace Headstone.Framework.Cache.Channels
{
    public class InProcessCacheChannel : ICacheChannel
    {
        public static readonly InProcessCacheChannel Instance = new InProcessCacheChannel();
#if NET452
        private InProcessCacheChannel() { }
        protected ObjectCache Cache
        {
            get
            {
                return MemoryCache.Default;
            }
        }
#elif NETCOREAPP2_2
        private readonly MemoryCache _cache;
        private InProcessCacheChannel()
        {
            _cache = new MemoryCache(new MemoryCacheOptions());
        }
        protected MemoryCache Cache
        {
            get
            {
                //return MemoryCache.Default;
                return _cache;

            }
        }
#endif



        public void Flush()
        {
#if NET452
            MemoryCache.Default.Dispose();

#elif NETCOREAPP2_2
            _cache.Dispose();
#endif
        }

        public T Get<T>(string key) where T : class
        {
#if NET452
            return (T)Cache[key.Replace(" ", "").ToLower()];
#elif NETCOREAPP2_2
            return Cache.Get<T>(key.Replace(" ", "").ToLower());
#endif
        }

        public T Get<T>(string key, string region) where T : class
        {
#if NET452
            return (T)Cache[(key + region).Replace(" ", "").ToLower()];
#elif NETCOREAPP2_2
            return Cache.Get<T>((key + region).Replace(" ", "").ToLower());
#endif

        }

        public void Remove(string key)
        {
            Cache.Remove(key.Replace(" ", "").ToLower());
        }

        public void Set(string key, object value)
        {
            if (value == null)
            {
                return;
            }
            Cache.Set(key.Replace(" ", "").ToLower(), value, DateTimeOffset.Now + CacheService.DefaultExpirationTime);
        }

        public void Set(string key, object value, TimeSpan expiration)
        {
            if (value == null)
            {
                return;
            }
            Cache.Set(key.Replace(" ", "").ToLower(), value, DateTimeOffset.Now + expiration);
        }

        public void Set(string key, object value, string region)
        {
            if (value == null)
            {
                return;
            }
            Cache.Set((key + region).Replace(" ", "").ToLower(), value, DateTimeOffset.Now + CacheService.DefaultExpirationTime);
        }

        public void Set(string key, object value, string region, TimeSpan expiration)
        {
            if (value == null)
            {
                return;
            }
            Cache.Set((key + region).Replace(" ", "").ToLower(), value, DateTimeOffset.Now + expiration);
        }
    }
}
