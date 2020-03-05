using Headstone.Framework.Models;
using Headstone.Framework.SaaS.Models.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headstone.Framework.SaaS.Models
{
    public class Tenant : Entity
    {
        [Key]
        public int TenantId { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }        

        public string Description { get; set; }

        public string Culture { get; set; }

        public string TimeZone { get; set; }

        #region [ Navigation properties ]

        public virtual List<Application> Applications { get; set; } = new List<Application>();

        public virtual List<TenantPaymentInfo> PaymentInformation { get; set; } = new List<TenantPaymentInfo>();

        public virtual List<TenantBillingInfo> BillingInformation { get; set; } = new List<TenantBillingInfo>();

        public virtual List<TenantContactInfo> ContactInformation { get; set; } = new List<TenantContactInfo>();

        public virtual List<TenantProperty> Properties { get; set; } = new List<TenantProperty>();

        public virtual List<HeadstoneUserRole> Users { get; set; } = new List<HeadstoneUserRole>();

        #endregion
    }
}
