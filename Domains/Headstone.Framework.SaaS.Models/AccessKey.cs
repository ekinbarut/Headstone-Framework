using Headstone.Framework.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Headstone.Framework.SaaS.Models
{
    public class AccessKey : Entity
    {
        [Key]
        public int AccessKeyId { get; set; }

        public int ApplicationId { get; set; }

        public string Key { get; set; }

        public string Secret { get; set; }

        public string Token { get; set; }

        public AccessPermissions Permission { get; set; }

        #region [ Navigation Properties ]

        [ForeignKey("ApplicationId")]
        public virtual Application Application { get; set; }
        #endregion
    }
}