using System;

namespace finalproject.Models
{
    public class Comment
    {
        public int Id { get; set; }
        
        public int TemplateId { get; set; }
     public virtual Template Template { get; set; } = default!;

        
       public string UserId { get; set; } = default!;

        public virtual ApplicationUser User { get; set; } = default!;
        
        public string Content { get; set; } =default!;
          public string CreatedById { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime CommentDate { get; set; }
    }
}
