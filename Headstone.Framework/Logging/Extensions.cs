using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headstone.Framework.Logging
{
    public static class ExceptionExtensions
    {
        public static string GetExceptionMessage(this Exception ex)
        {
            var message = new StringBuilder();
            message.AppendLine(ex.Message);
            var exception = ex;
            while (exception.InnerException != null)
            {
                exception = exception.InnerException;
                message.AppendLine(exception.Message);
            }

            return message.ToString();
        }
    }
}
