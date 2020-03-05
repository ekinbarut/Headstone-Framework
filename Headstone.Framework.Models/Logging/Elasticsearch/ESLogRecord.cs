using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Headstone.Framework.Models.Logging.Elasticsearch
{
    public class ESLogRecord
    {
        #region [ NoSQL entity properties ]

        public string LogID { get; set; }

        public string Namespace { get; set; }

        public string UniqueKey { get; set; }

        #endregion

        #region [ Application information ]

        [MaxLength(100)]
        public string AppKey { get; set; }

        [MaxLength(50)]
        public string Environment { get; set; }

        #endregion

        #region [ Host information ]

        [MaxLength(50)]
        public string Host { get; set; }

        [MaxLength(50)]
        public string HostIP { get; set; }

        #endregion

        #region [ End user infomation ]

        [MaxLength(100)]
        public string Token { get; set; }

        public string UserId { get; set; }

        public string Username { get; set; }

        public string UserIP { get; set; }

        public string IdentityKey { get; set; }

        #endregion
        
        public string Level { get; set; }

        public string Process { get; set; }

        public string ThreadId { get; set; }

        public string Message { get; set; }

        public string Exception { get; set; }

        public string Data { get; set; }

        #region [ Log standartization ]

        public DateTime TimeStamp { get; set; }

        #endregion
    }
}
