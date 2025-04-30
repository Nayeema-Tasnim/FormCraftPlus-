using System;
using System.Collections.Generic;

namespace finalproject.Models
{
    public class FilledForm
    {
        public int Id { get; set; }
        
        public int TemplateId { get; set; }
        public virtual Template Template { get; set; }= default!;
        public string FilledById { get; set; } = string.Empty;
        public DateTime SubmittedAt { get; set; } = DateTime.Now;
        public string? SubmittedById { get; set; }
        public virtual ApplicationUser SubmittedBy { get; set; }=default!;
        
        public DateTime SubmissionDate { get; set; }
            public string CreatedById { get; set; } = string.Empty;
            
        public DateTime CreatedAt { get; set; } = DateTime.Now;
      public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

    }
}

