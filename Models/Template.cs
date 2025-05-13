
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace finalproject.Models
{
    public class Template
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsBlocked { get; set; } = false;

        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;

        
        public bool IsPublic { get; set; } = true;
        public string RestrictedToUserIds { get; set; } = string.Empty; 

        public string CreatedById { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

[NotMapped]
public string TagNames { get; set; } = string.Empty;
  
        public List<Question> Questions { get; set; } = new List<Question>();
        public List<Tag> Tags { get; set; } = new List<Tag>();
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public List<Like> Likes { get; set; } = new List<Like>();
        public List<FilledForm> FilledForms { get; set; } = new List<FilledForm>();
    }
}
