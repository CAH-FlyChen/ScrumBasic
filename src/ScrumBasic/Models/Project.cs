using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumBasic.Models
{
    public class Project
    {
        [Key]
        [MaxLength(32)]
        public string ID { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public ICollection<UserStory> Stories { get; set; }
        public ApplicationUser Creator { get; set; }
        public DateTime CreateTime { get; set; }
        public ApplicationUser Modifier { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
