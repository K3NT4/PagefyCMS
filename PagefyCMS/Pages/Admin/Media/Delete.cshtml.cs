using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using PagefyCMS.Data;
using PagefyCMS.Models;

namespace PagefyCMS.Pages.Admin.Media
{
    public class DeleteModel : PageModel
    {
        private readonly PagefyDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<DeleteModel> _logger;

        public DeleteModel(PagefyDbContext context, IWebHostEnvironment env, ILogger<DeleteModel> logger)
        {
            _context = context;
            _env = env;
            _logger = logger;
        }

        [BindProperty]
        public Guid Id { get; set; }

        public MediaItem Media { get; set; }
        public List<string> UsageList { get; set; } = new List<string>();

        public IActionResult OnGet(Guid id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("LoggedIn")))
                return RedirectToPage("/Admin/Login");

            Media = _context.MediaLibrary.FirstOrDefault(m => m.Id == id);
            if (Media == null)
            {
                return NotFound();
            }

            Id = Media.Id;

            // Perform full scan for usage
            var pages = _context.Pages
                .AsNoTracking()
                .Where(p => p.Content.Contains(Media.Slug))
                .Select(p => $"Sida: {p.Title}")
                .ToList();
            UsageList.AddRange(pages);

            var articles = _context.Articles
                .AsNoTracking()
                .Where(a => a.Content.Contains(Media.Slug))
                .Select(a => $"Artikel: {a.Headline}")
                .ToList();
            UsageList.AddRange(articles);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("LoggedIn")))
                return RedirectToPage("/Admin/Login");

            var media = await _context.MediaLibrary.FindAsync(Id);
            if (media != null)
            {
                await DeleteMediaFiles(media);
                _context.MediaLibrary.Remove(media);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Admin/Media/Index");
        }

        private async Task DeleteMediaFiles(MediaItem media)
        {
            var filesToDelete = new[] {
                media.WebpSmall,
                media.WebpMedium,
                media.WebpLarge,
                media.OriginalPath
            };

            foreach (var filePath in filesToDelete)
            {
                try
                {
                    var fullPath = Path.Combine(_env.WebRootPath, filePath.TrimStart('/'));
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to delete file: {FilePath}", filePath);
                }
            }

            await Task.CompletedTask;
        }
    }
}
