using Headstone.Framework.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headstone.Framework.SaaS.Models
{
    public class Package : Entity
    {
        [Key]
        public int PackageId { get; set; }

        public int TenantId { get; set; }

        public int? ApplicationId { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string ShortDescription { get; set; }

        public string LongDescription { get; set; }

        #region [ Navigation properties ]

        public virtual Tenant Tenant { get; set; }

        public virtual Application Application { get; set; }

        public virtual List<PackageVariant> Variants { get; set; } = new List<PackageVariant>();
        
        public virtual List<PackageRestriction> Restrictions { get; set; } = new List<PackageRestriction>();

        public virtual List<PackageFeatures> Features { get; set; } = new List<PackageFeatures>();

        public virtual List<PackageProperty> Properties { get; set; } = new List<PackageProperty>();

        public virtual List<PackageTag> Tags { get; set; } = new List<PackageTag>();

        #endregion
    }
}
