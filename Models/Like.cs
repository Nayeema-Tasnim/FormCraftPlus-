namespace finalproject.Models
{
    public class Like
    {
        public int Id { get; set; }
        
        public int TemplateId { get; set; }
        public virtual Template Template { get; set; }= default!;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? UserId { get; set; }
        public virtual ApplicationUser User { get; set; }= default!;
    }
}
