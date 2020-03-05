using Headstone.Framework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headstone.Framework.SaaS.Models.Requests
{
    public class UserQueryRequest : BaseRequest
    {
        #region [ User information ]

        public List<int> UserIds { get; set; } = new List<int>();

        public List<string> Emails { get; set; } = new List<string>();

        #endregion

        public List<EntityStatus> Status { get; set; } = new List<EntityStatus>();
    }
}
