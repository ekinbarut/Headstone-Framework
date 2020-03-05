using Headstone.Framework.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headstone.Framework.SaaS.Models
{
    public class Tag : Entity
    {
        [Key]
        public int TagId { get; set; }

        public TagType Type { get; set; }

        public string Value { get; set; }
    }
}
