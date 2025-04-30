using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace finalproject.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsBlocked { get; set; }
        public bool IsAdmin { get; set; } 
        public virtual ICollection<Template> Templates { get; set; } = new List<Template>();

      public virtual ICollection<FilledForm> Forms { get; set; } = new List<FilledForm>();

    }
}


