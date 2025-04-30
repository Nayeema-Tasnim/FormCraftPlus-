using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace finalproject.Models
{
    public class Topic
    {
        public int Id { get; set; }
        
        [Required]
        public string? Name { get; set; }
        
        public virtual ICollection<Template> Templates { get; set; } =new List<Template>();
    }
}
