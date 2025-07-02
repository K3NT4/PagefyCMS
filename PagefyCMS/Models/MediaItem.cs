namespace PagefyCMS.Models
{
    public class MediaItem
    {
        public Guid Id { get; set; }
        public string Filename { get; set; }
        public string Slug { get; set; }
        public string Title { get; set; }  // Namn på bilden
        public string AltText { get; set; }  // Alt-text för SEO
        public string Description { get; set; }  // (Valfritt, kan användas för längre beskrivningar i framtiden)
        public string OriginalPath { get; set; }
        public string WebpSmall { get; set; }
        public string WebpMedium { get; set; }
        public string WebpLarge { get; set; }
        public DateTime UploadedAt { get; set; }
        public string GalleryGroup { get; set; }
    }

}
