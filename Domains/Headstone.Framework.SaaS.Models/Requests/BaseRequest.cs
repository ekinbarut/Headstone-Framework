using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headstone.Framework.SaaS.Models.Requests
{
    public class BaseRequest
    {
        #region [ Service identity ]

        public string ClientId { get; set; }

        public string AppKey { get; set; }

        public string Token { get; set; }

        public string Environment { get; set; }

        #endregion

        public string Envelope { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}
