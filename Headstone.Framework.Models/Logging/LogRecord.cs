using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Headstone.Framework.Models.Logging
{
    [Table("Logs")]
    public class LogRecord
    {
        [Key]
        public int RecordId { get; set; }

        #region [ NoSQL entity properties ]

        public string LogID { get; set; }

        public string Namespace
        {
            get
            {
                return "LOG";
            }
        }

        public string UniqueKey
        {
            get
            {
                return "L#" + AppKey + "#" + Environment + "#" + LogID;
            }
        }

        #endregion

        #region [ Application information ]

        public string TenantId { get; set; }

        public string ApplicationId { get; set; }

        [MaxLength(100)]
        public string AppKey { get; set; }

        [MaxLength(50)]
        public string Environment { get; set; }

        public string ApplicationIp { get; set; }

        #endregion

        #region [ Host information ]

        [MaxLength(50)]
        public string Host { get; set; }

        [MaxLength(50)]
        public string HostIP { get; set; }

        #endregion

        #region [ End user infomation ]

        [MaxLength(100)]
        public string UserToken { get; set; }

        public string UserId { get; set; }

        public string Username { get; set; }

        public string UserIP { get; set; }

        public string IdentityKey
        {
            get
            {
                if (String.IsNullOrEmpty(UserId)) return string.Empty;

                return AppKey + "#" + Environment + "#" + UserId;
            }
        }

        #endregion
        
        public string Level { get; set; }

        public string Process { get; set; }

        public string ThreadId { get; set; }

        public string Message { get; set; }

        [NotMapped]
        public Exception Exception { get; set; }

        public string ExceptionString { get; set; }

        public dynamic Data { get; set; }

        public string DataString { get; set; }

        public DateTime Created { get; set; }

        #region [ Log standartization ]

        public DateTime TimeStamp { get; set; }

        #endregion
    }
}
