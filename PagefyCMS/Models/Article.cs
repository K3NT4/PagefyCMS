using System.ComponentModel.DataAnnotations;

namespace PagefyCMS.Models
{
    public class ArticlePage
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Rubrik är obligatorisk")]
        [StringLength(255, ErrorMessage = "Rubriken kan inte överstiga 255 tecken")]
        public string Headline { get; set; } = string.Empty;

        [Required(ErrorMessage = "Slug är obligatorisk")]
        [StringLength(255)]
        public string Slug { get; set; } = string.Empty;

        [Required(ErrorMessage = "Innehål är obligatoriskt")]
        public string Content { get; set; } = string.Empty;

        public DateTime PublishedAt { get; set; }

        [StringLength(255)]
        public string MetaTitle { get; set; } = string.Empty;

        [StringLength(500)]
        public string MetaDescription { get; set; } = string.Empty;

        public DateTime UpdatedAt { get; set; }
    }
}
