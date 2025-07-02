namespace PagefyCMS.Models
{
    public class ContentPage
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? GalleryGroup { get; set; }
        public bool ShowInMenu { get; set; }
    }
}
