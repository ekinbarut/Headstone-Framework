using Headstone.Framework.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Headstone.Framework.SaaS.Models
{
    [Table("Applications")]
    public class Application : Entity
    {
        [Key]
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public int TenantId { get; set; }

        public string Code { get; set; }
        
        public string Domain { get; set; }

        public string Name { get; set; }

        public string Culture { get; set; }

        public string TimeZone { get; set; }

        #region [ Navigation properties ]

        [ForeignKey("ParentId")]
        public virtual Application Parent { get; set; }

        [ForeignKey("TenantId")]
        public virtual Tenant Tenant { get; set; }

        [InverseProperty("Parent")]
        public virtual List<Application> Children { get; set; } = new List<Application>();

        public virtual List<AccessKey> AccessKeys { get; set; } = new List<AccessKey>();

        public virtual List<ApplicationProperty> Properties { get; set; } = new List<ApplicationProperty>();

        #endregion
    }
}