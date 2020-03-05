using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headstone.Framework.SaaS.Models
{
    public class TenantProperty : Property
    {
        public int TenantId { get; set; }

        #region [ Navigation properties ]

        [ForeignKey("TenantId")]
        public virtual Tenant Tenant { get; set; }

        #endregion
    }
}
