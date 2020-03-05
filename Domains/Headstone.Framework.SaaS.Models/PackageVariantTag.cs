using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headstone.Framework.SaaS.Models
{
    public class PackageVariantTag : Tag
    {
        public int VariantId { get; set; }

        #region [ Navigation properties ]

        [ForeignKey("VariantId")]
        public virtual PackageVariant Variant { get; set; }

        #endregion
    }
}
