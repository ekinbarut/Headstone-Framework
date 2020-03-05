using System;
using System.ComponentModel.DataAnnotations;
using Headstone.Framework.Models.Resources;

namespace Headstone.Framework.Models
{
    [Serializable]
    public class Entity
    {       
        [Display(Name = "Entity_Created", ResourceType = typeof(FrameworkModels))]
        public DateTime Created { get; set; }

        [Display(Name = "Entity_Updated", ResourceType = typeof(FrameworkModels))]
        public DateTime? Updated { get; set; }

        [Display(Name = "Entity_Status", ResourceType = typeof(FrameworkModels))]
        public EntityStatus Status { get; set; }
    }
}
