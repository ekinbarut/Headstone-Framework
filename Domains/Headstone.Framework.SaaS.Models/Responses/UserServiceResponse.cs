using Headstone.Framework.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headstone.Framework.SaaS.Models.Responses
{
    public class UserServiceResponse<T> : ServiceResponse<T>
    {
        public int UserId { get; set; }
    }
}
