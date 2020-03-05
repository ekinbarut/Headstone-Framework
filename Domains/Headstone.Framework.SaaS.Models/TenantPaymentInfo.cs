using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Headstone.Framework.SaaS.Models
{
    [Table("TenantPaymentInformation")]
    public class TenantPaymentInfo
    {
        [Key]
        public int Id { get; set; }

        public int TenantId { get; set; }


        #region [ Navigation properties ]

        [ForeignKey("TenantId")]
        public virtual Tenant Tenant { get; set; }

        #endregion
    }
}