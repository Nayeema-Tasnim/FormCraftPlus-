

namespace finalproject.Models
{
    public class HomePageViewModel
    {
        public List<Template> LatestTemplates { get; set; } = new List<Template>();
      public List<PopularTemplateViewModel> PopularTemplates { get; set; } = new List<PopularTemplateViewModel>();
        public List<Tag> Tags { get; set; } = new List<Tag>();
    }
}
