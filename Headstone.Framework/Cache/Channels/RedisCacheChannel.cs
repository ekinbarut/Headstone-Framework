using Headstone.Framework.Caching;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Headstone.Framework.Models.Caching;

namespace Headstone.Framework.Cache.Channels
{
    public class RedisCacheChannel : ICacheChannel
    {
        public static readonly RedisCacheChannel Instance = new RedisCacheChannel();
        private static ConnectionMultiplexer _redisServer = null;

        private RedisCacheChannel() {

            // Set the redis server
            _redisServer = ConnectionMultiplexer.Connect(CacheService.Server);
        }

        public void Flush()
        {
            throw new NotImplementedException();
        }

        public T Get<T>(string key) where T : class
        {
            // Get the database
            var db = _redisServer.GetDatabase(0);

            // Get the value
            return db.StringGet(key.Replace(" ", "").ToLower()) as T;
        }

        public T Get<T>(string key, string region) where T : class
        {
            // Get the database
            var db = _redisServer.GetDatabase(0);

            // Get the value
            return db.StringGet((key + region).Replace(" ", "").ToLower()) as T;
        }
        
        public void Remove(string key)
        {
            // Get the database
            var db = _redisServer.GetDatabase(0);

            // Get the value
            //return db.(key + region).Replace(" ", "").ToLower()];            
        }

        public void Set(string key, object value)
        {
            throw new NotImplementedException();
        }

        public void Set(string key, object value, TimeSpan expiration)
        {
            throw new NotImplementedException();
        }

        public void Set(string key, object value, string region)
        {
            throw new NotImplementedException();
        }

        public void Set(string key, object value, string region, TimeSpan expiration)
        {
            throw new NotImplementedException();
        }
    }
}
