using System.ComponentModel.DataAnnotations;

namespace PagefyCMS.Models
{
    public class CmsSetting
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Key { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Value { get; set; } = string.Empty;

        [StringLength(100)]
        public string? ActiveTheme { get; set; }
    }
}
