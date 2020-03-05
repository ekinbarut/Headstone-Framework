using Headstone.Framework.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headstone.Framework.SaaS.Models
{
    public class Property : Entity
    {
        [Key]
        public int PropertyId { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        public string Extra { get; set; }
    }
}
