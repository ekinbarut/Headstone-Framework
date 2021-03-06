﻿using Headstone.Framework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headstone.Framework.SaaS.Models.Requests
{
    public class TenantQueryRequest : BaseRequest
    {
        #region [ Tenant information ]

        public List<int> TenantIds { get; set; } = new List<int>();

        #endregion

        #region [ Application information ]

        public List<int> ApplicationIds { get; set; } = new List<int>();

        #endregion

        public List<EntityStatus> Status { get; set; } = new List<EntityStatus>();
    }
}
