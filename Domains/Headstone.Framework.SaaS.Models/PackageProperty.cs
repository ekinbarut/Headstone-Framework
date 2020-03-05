using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headstone.Framework.SaaS.Models
{
    public class PackageProperty : Property
    {
        public int PackageId { get; set; }

        #region [ Navigation properties ]

        [ForeignKey("PackageId")]
        public virtual Package Package { get; set; }

        #endregion
    }
}
