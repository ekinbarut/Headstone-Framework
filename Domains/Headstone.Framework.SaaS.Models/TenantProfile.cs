using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Headstone.Framework.SaaS.Models
{
    public class TenantProfile
    {
        [Key, ForeignKey("Tenant")]
        public int TenantId { get; set; }

        #region [ Navigation properties ]

        public virtual Tenant Tenant { get; set; }

        #endregion
    }
}