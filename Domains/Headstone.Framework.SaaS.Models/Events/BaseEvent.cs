
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headstone.Framework.SaaS.Models.Events
{
    public class BaseEvent
    {
        #region [ Service identity ]

        public string ClientId { get; set; }

        public string AppKey { get; set; }

        public string Token { get; set; }

        public string Environment { get; set; }

        #endregion

        #region [ User identity ]

        public string UserToken { get; set; }

        public string SessionId { get; set; }

        public string UserIP { get; set; }

        #endregion

        public DateTime TimeStamp { get; set; }
    }
}
