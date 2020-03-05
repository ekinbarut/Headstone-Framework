using System;
using System.Collections.Generic;
using Headstone.Framework.Models.Configuration;

namespace Headstone.Framework.Models.Events
{
    public class ConfigurationEventArgs : EventArgs
    {
        public ConfigurationEventTypes EventType { get; set; }

        public List<ConfigRecord> Records { get; set; }

        public ConfigurationEventArgs(ConfigurationEventTypes eventType, List<ConfigRecord> records)
        {
            EventType = eventType;
            Records = records;
        }
    }

    public delegate void ConfigurationEventHandler(ConfigurationEventArgs args);
}
