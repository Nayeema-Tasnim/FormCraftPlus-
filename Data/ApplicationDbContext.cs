using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using finalproject.Models;

namespace finalproject.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Template> Templates { get; set; }
        public DbSet<FilledForm> FilledForms { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Topic> Topics { get; set; }
        
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TemplateTag> TemplateTags { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

        
            builder.Entity<TemplateTag>()
                   .HasKey(tt => new { tt.TemplateId, tt.TagId });

 
            builder.Entity<Answer>()
                .HasOne(a => a.Question)
                .WithMany(q => q.Answers)
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);

            
            builder.Entity<Answer>()
                .HasOne(a => a.FilledForm)
                .WithMany(f => f.Answers)
                .HasForeignKey(a => a.FormId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}


