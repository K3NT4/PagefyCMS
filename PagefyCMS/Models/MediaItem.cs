using System.ComponentModel.DataAnnotations;

namespace PagefyCMS.Models
{
    public class MediaItem
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(500)]
        public string Filename { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Slug { get; set; } = string.Empty;

        [StringLength(255)]
        public string Title { get; set; } = string.Empty;  // Namn på bilden

        [StringLength(500)]
        public string AltText { get; set; } = string.Empty;  // Alt-text för SEO

        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;  // (Valfritt, kan användas för längre beskrivningar i framtiden)

        [Required]
        [StringLength(500)]
        public string OriginalPath { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string WebpSmall { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string WebpMedium { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string WebpLarge { get; set; } = string.Empty;

        public DateTime UploadedAt { get; set; }

        [StringLength(255)]
        public string? GalleryGroup { get; set; }
    }
}
