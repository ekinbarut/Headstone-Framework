using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Headstone.Framework.Models.Configuration
{
    [Table("SystemConfiguration")]
    public class ConfigRecord
    {
        [Key]
        public int RecordId { get; set; }

        public string AppKey { get; set; }

        public string Class { get; set; }

        public string Property { get; set; }

        public string Environment { get; set; }

        public string Value { get; set; }

        public string Description { get; set; }

    }
}
