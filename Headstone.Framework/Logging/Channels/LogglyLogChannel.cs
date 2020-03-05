using Headstone.Framework.Models.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;

namespace Headstone.Framework.Logging.Channels
{
    public class LogglyLogChannel : ILogChannel
    {
        public static string Token { get; set; }

        public void Log(LogRecord record)
        {
            using (var client = new HttpClient()){

                client.BaseAddress = new Uri("http://logs-01.loggly.com/inputs");

                var result = client.PostAsync("/inputs/" + Token + "/tag/" + record.AppKey, new StringContent(JsonConvert.SerializeObject(record), Encoding.UTF8, "application/json")).Result;

                string resultContent = result.Content.ReadAsStringAsync().Result;                               
            }
            
        }
    }
}
