using Microsoft.AspNetCore.Mvc.RazorPages;
using PagefyCMS.Data;
using PagefyCMS.Models;
using Microsoft.EntityFrameworkCore;

namespace PagefyCMS.Pages
{
    /// <summary>
    /// Index page model - displays latest articles and featured content.
    /// </summary>
    public class IndexModel : PageModel
    {
        private readonly PagefyDbContext _context;
        private const int ArticlesPerPage = 10;

        /// <summary>
        /// List of published articles to display.
        /// </summary>
        public List<ArticlePage> Articles { get; set; } = new();

        /// <summary>
        /// Site title from settings.
        /// </summary>
        public string? SiteTitle { get; set; }

        public IndexModel(PagefyDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Load articles on page load.
        /// </summary>
        public async Task OnGetAsync()
        {
            try
            {
                // Get published articles ordered by date (newest first)
                Articles = await _context.Articles
                    .AsNoTracking() // Improve performance for read-only operations
                    .Where(a => a.PublishedAt <= DateTime.UtcNow)
                    .OrderByDescending(a => a.PublishedAt)
                    .Take(ArticlesPerPage)
                    .ToListAsync();

                // Get site title from settings
                SiteTitle = await _context.Settings
                    .AsNoTracking()
                    .Where(s => s.Key == "SiteTitle")
                    .Select(s => s.Value)
                    .FirstOrDefaultAsync() ?? "PagefyCMS";
            }
            catch (Exception ex)
            {
                // Log error (can be extended with logging framework)
                System.Diagnostics.Debug.WriteLine($"Error loading articles: {ex.Message}");
                
                // Don't crash - just show empty list
                Articles = new List<ArticlePage>();
            }
        }
    }
}
