namespace finalproject.Models
{
    public class TemplateTag
    {
        public int TemplateId { get; set; }
        public virtual Template Template { get; set; } =default!;
        
        public int TagId { get; set; }
      public List<TemplateTag> TemplateTags { get; set; } = new List<TemplateTag>();
    }
}
