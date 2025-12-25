using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PagefyCMS.Models
{
    public class AssetUsage
    {
        public int Id { get; set; }

        public Guid AssetId { get; set; } // FK to MediaItem.Id

        public int ContentId { get; set; } // FK to Page.Id or Article.Id

        [StringLength(50)]
        public string ContentType { get; set; } = string.Empty; // "Page", "Article"

        [StringLength(255)]
        public string ContentTitle { get; set; } = string.Empty; // Cached title for display

        [StringLength(50)]
        public string ReferenceKind { get; set; } = "BodyHtml"; // "BodyHtml", "FeaturedImage", etc.

        public DateTime LastSeenAt { get; set; }
    }
}
