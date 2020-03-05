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
    public class PackageVariant : Entity
    {
        [Key]
        public int VariantId { get; set; }

        public int PackageId { get; set; }

        public string Name { get; set; }

        public decimal? ListPrice { get; set; }

        public decimal Price { get; set; }

        public string Currency { get; set; }

        public string Period { get; set; }

        #region [ Navigation properties ]

        [ForeignKey("PackageId")]
        public virtual Package Package { get; set; }

        public virtual List<PackageVariantProperty> Properties { get; set; } = new List<PackageVariantProperty>();

        public virtual List<PackageVariantTag> Tags { get; set; } = new List<PackageVariantTag>();

        #endregion
    }
}
