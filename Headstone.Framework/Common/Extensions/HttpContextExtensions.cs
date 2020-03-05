#if NET452
using System.Web;
#elif NETCOREAPP2_2
using Microsoft.AspNetCore.Http;
#endif

namespace Headstone.Framework.Common.Extensions
{
    public static class HttpContextExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetClientIP(this HttpContext context)
        {
#if NET452
            // Get the customer IP
            //string customerIP = HttpContext.Current.Request.Headers.Get("Client-IP") != null ? HttpContext.Current.Request.Headers.Get("Client-IP") : HttpContext.Current.Request.UserHostAddress;

            // Try to get the ip list
            string ipList = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (!string.IsNullOrEmpty(ipList))
            {
                return ipList.Split(',')[0];
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
#elif NETCOREAPP2_2
            return context.Connection.RemoteIpAddress.ToString();
#endif
        }

#if NET452
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetClientIP(this HttpContextBase context)
        {
            // Get the customer IP
            //string customerIP = HttpContext.Current.Request.Headers.Get("Client-IP") != null ? HttpContext.Current.Request.Headers.Get("Client-IP") : HttpContext.Current.Request.UserHostAddress;

            // Try to get the ip list
            string ipList = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (!string.IsNullOrEmpty(ipList))
            {
                return ipList.Split(',')[0];
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }
#endif
    }
}
