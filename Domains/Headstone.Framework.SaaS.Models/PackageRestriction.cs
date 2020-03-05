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
    public class PackageRestriction : Entity
    {
        [Key]
        public int RestrictionId { get; set; }

        public int PackageId { get; set; }

        public string RestrictionCode { get; set; }

        public string ShortDescription { get; set; }

        public string LongDescription { get; set; }

        #region [ Navigation properties ]

        [ForeignKey("PackageId")]
        public virtual Package Package { get; set; }

        #endregion
    }
}
