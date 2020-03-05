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
    public class PackageFeatures : Entity
    {
        [Key]
        public int FeatureId { get; set; }

        public int PackageId { get; set; }

        public string GroupCode { get; set; }

        public string Code { get; set; }

        public string ShortDescription { get; set; }

        public string LongDescription { get; set; }

        #region [ Calculation fields ]

        public decimal? Amount { get; set; }

        public string Unit { get; set; }

        public bool AllowExcessUsage { get; set; }

        #endregion

        #region [ Navigation properties ]

        [ForeignKey("PackageId")]
        public virtual Package Package { get; set; }

        #endregion
    }
}
