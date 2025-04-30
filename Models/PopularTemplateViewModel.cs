namespace finalproject.Models
{
    public class PopularTemplateViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public int LikeCount { get; set; }
    }
}
