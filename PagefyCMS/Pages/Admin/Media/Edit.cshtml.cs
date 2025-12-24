using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagefyCMS.Data;
using PagefyCMS.Models;

namespace PagefyCMS.Pages.Admin.Media
{
    public class EditModel : PageModel
    {
        private readonly PagefyDbContext _context;
        private readonly IWebHostEnvironment _env;

        public EditModel(PagefyDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [BindProperty]
        public Guid Id { get; set; }

        [BindProperty]
        public IFormFile ReplaceFile { get; set; }

        [BindProperty]
        public string Title { get; set; } = "";

        [BindProperty]
        public string AltText { get; set; } = "";

        [BindProperty]
        public string Description { get; set; } = "";

        public MediaItem Media { get; set; }

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
            Title = Media.Title ?? "";
            AltText = Media.AltText ?? "";
            Description = Media.Description ?? "";

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("LoggedIn")))
                return RedirectToPage("/Admin/Login");

            var media = _context.MediaLibrary.FirstOrDefault(m => m.Id == Id);
            if (media == null)
            {
                return NotFound();
            }

            media.Title = string.IsNullOrWhiteSpace(Title) ? "" : Title;
            media.AltText = string.IsNullOrWhiteSpace(AltText) ? "" : AltText;
            media.Description = string.IsNullOrWhiteSpace(Description) ? "" : Description;

            if (ReplaceFile != null && ReplaceFile.Length > 0)
            {
                 // Basic validation
                var ext = Path.GetExtension(ReplaceFile.FileName).ToLower();
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                if (allowedExtensions.Contains(ext))
                {
                    await ProcessReplaceFileAsync(media, ReplaceFile);
                }
                else
                {
                    ModelState.AddModelError("ReplaceFile", "Ogiltig filtyp.");
                    Media = media;
                    return Page();
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("/Admin/Media/Edit", new { id = media.Id });
        }

        private async Task ProcessReplaceFileAsync(MediaItem media, IFormFile file)
        {
            // We use the existing Slug to overwrite files, maintaining URLs.
            // Note: Browser caching might prevent immediate visual update for users.

            var uploadsPath = Path.Combine(_env.WebRootPath, "uploads");
            var originalPath = Path.Combine(uploadsPath, "originals", media.Slug);

            // Overwrite original
            using (var stream = new FileStream(originalPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Re-generate WebP versions
            var fileName = Path.GetFileNameWithoutExtension(media.Slug); // Slug is filename-guid.ext, we need the base name for webp files?
            // Actually, in IndexModel, slug was "filename-guid.ext".
            // WebP files were saved as "filename-guid.webp" in subdirectories.
            // We need to parse the base name from the Slug or store it.
            // media.Slug is "mypic-GUID.jpg".
            // The WebP files are "mypic-GUID.webp".
            // So we can strip extension from Slug to get the base name.

            var baseName = Path.GetFileNameWithoutExtension(media.Slug);

            var smallPath = Path.Combine(uploadsPath, "webp", "small", $"{baseName}.webp");
            var mediumPath = Path.Combine(uploadsPath, "webp", "medium", $"{baseName}.webp");
            var largePath = Path.Combine(uploadsPath, "webp", "large", $"{baseName}.webp");

            using (var image = SixLabors.ImageSharp.Image.Load(originalPath))
            {
                image.Mutate(x => x.Resize(400, 0));
                await image.SaveAsWebpAsync(smallPath);

                image.Mutate(x => x.Resize(800, 0));
                await image.SaveAsWebpAsync(mediumPath);

                image.Mutate(x => x.Resize(1600, 0));
                await image.SaveAsWebpAsync(largePath);
            }

            media.UploadedAt = DateTime.UtcNow; // Update timestamp
            // media.Filename = file.FileName; // Optionally update original filename if we want to track the new name, but Slug remains same.
            // Let's update Filename so admin sees the new name, but Slug (URL) is constant.
            media.Filename = file.FileName;
        }
    }
}
