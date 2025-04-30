using System.Collections.Generic;

namespace finalproject.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        
                public List<TemplateTag> TemplateTags { get; set; } = new List<TemplateTag>();
    }
}
