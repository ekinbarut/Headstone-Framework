using Headstone.Framework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headstone.Framework.SaaS.Models.Requests
{
    public class ApplicationQueryRequest : BaseRequest
    {
        #region [ Tenant information ]

        public int? TenantId { get; set; }

        #endregion

        #region [ Application information ]

        public List<int> ApplicationIds { get; set; } = new List<int>();

        #endregion

        public List<EntityStatus> Status { get; set; } = new List<EntityStatus>();
    }
}
