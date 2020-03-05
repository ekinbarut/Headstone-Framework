using System.ComponentModel.DataAnnotations.Schema;
#if NET452
using Microsoft.AspNet.Identity.EntityFramework;
#elif NETCOREAPP2_2
using Microsoft.AspNetCore.Identity;
#endif

namespace Headstone.Framework.SaaS.Models.Identity
{
    public class HeadstoneUserRole : IdentityUserRole<int>
    {
        public int? TenantId { get; set; }

        #region [ Navigation properties ]

        [ForeignKey("UserId")]
        public virtual HeadstoneUser User { get; set; }

        [ForeignKey("TenantId")]
        public virtual Tenant Tenant { get; set; }

        #endregion
    }
}
