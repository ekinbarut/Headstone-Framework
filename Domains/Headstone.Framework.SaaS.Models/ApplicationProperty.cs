using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headstone.Framework.SaaS.Models
{
    public class ApplicationProperty : Property
    {
        public int ApplicationId { get; set; }

        #region [ Navigation properties ]

        [ForeignKey("ApplicationId")]
        public virtual Application Application { get; set; }

        #endregion
    }
}
