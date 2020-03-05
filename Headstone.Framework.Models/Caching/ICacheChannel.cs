using System;
using System.Collections.Generic;

namespace Headstone.Framework.Models.Caching
{
    public interface ICacheChannel
    {
        T Get<T>(string key) where T : class;

        T Get<T>(string key, string region) where T : class;

        void Set(string key, object value);

        void Set(string key, object value, string region);

        void Set(string key, object value, TimeSpan expiration);

        void Set(string key, object value, string region, TimeSpan expiration);
        
        void Remove(string key);

        void Flush();
    }
}
