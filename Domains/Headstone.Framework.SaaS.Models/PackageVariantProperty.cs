using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headstone.Framework.SaaS.Models
{
    public class PackageVariantProperty : Property
    {
        public int PackageVariantId { get; set; }

        #region [ Navigation properties ]

        [ForeignKey("PackageVariantId")]
        public virtual PackageVariant Variant { get; set; }

        #endregion
    }
}
