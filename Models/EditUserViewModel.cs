using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace finalproject.Models
{
    public class EditUserViewModel
    {
        public string? Id { get; set; }
        
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        
       
        public List<string> Roles { get; set; } = new List<string>();

        public List<string> AllRoles { get; set; } = new List<string>();

 
        public List<string> SelectedRoles { get; set; } = new List<string>();
    }
}

