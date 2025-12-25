using System.Text.RegularExpressions;

namespace PagefyCMS.Services
{
    public static class SlugHelper
    {
        public static string GenerateSlug(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            string slug = input.ToLowerInvariant();
            // Replace whitespace with hyphens
            slug = Regex.Replace(slug, @"\s+", "-");
            // Remove non-alphanumeric characters except hyphens
            slug = Regex.Replace(slug, @"[^a-z0-9\-]", "");
            // Remove multiple hyphens
            slug = Regex.Replace(slug, @"\-+", "-");
            // Trim hyphens from start and end
            slug = slug.Trim('-');
            return slug;
        }
    }
}
