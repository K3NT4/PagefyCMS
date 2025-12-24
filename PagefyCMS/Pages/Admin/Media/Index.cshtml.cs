using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using PagefyCMS.Data;
using PagefyCMS.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace PagefyCMS.Pages.Admin.Media
{
    public class IndexModel : PageModel
    {
        private readonly IWebHostEnvironment _env;
        private readonly PagefyDbContext _context;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(IWebHostEnvironment env, PagefyDbContext context, ILogger<IndexModel> logger)
        {
            _env = env;
            _context = context;
            _logger = logger;
        }

        [BindProperty]
        public IFormFile UploadFile { get; set; }

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public int PageSize { get; set; } = 20;

        public List<MediaItem> MediaList { get; set; }
        public Dictionary<Guid, List<string>> MediaUsage { get; set; } = new Dictionary<Guid, List<string>>();

        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        private const long MaxFileSize = 50 * 1024 * 1024; // 50 MB

        public IActionResult OnGet()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("LoggedIn")))
                return RedirectToPage("/Admin/Login");

            var totalItems = _context.MediaLibrary.Count();
            TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);

            if (CurrentPage < 1) CurrentPage = 1;
            if (TotalPages > 0 && CurrentPage > TotalPages) CurrentPage = TotalPages;

            MediaList = _context.MediaLibrary
                .OrderByDescending(m => m.UploadedAt)
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            // Analyze media usage
            // performing a query per media item to avoid loading all content into memory.
            foreach (var media in MediaList)
            {
                var usedIn = new List<string>();

                // Check Pages
                var pages = _context.Pages
                    .AsNoTracking()
                    .Where(p => p.Content.Contains(media.Slug))
                    .Select(p => $"Sida: {p.Title}")
                    .Take(5)
                    .ToList();
                usedIn.AddRange(pages);

                // Check Articles
                var articles = _context.Articles
                    .AsNoTracking()
                    .Where(a => a.Content.Contains(media.Slug))
                    .Select(a => $"Artikel: {a.Headline}")
                    .Take(5)
                    .ToList();
                usedIn.AddRange(articles);

                if (usedIn.Any())
                {
                    MediaUsage[media.Id] = usedIn;
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("LoggedIn")))
                return RedirectToPage("/Admin/Login");

            if (UploadFile != null && UploadFile.Length > 0)
            {
                // Validate file
                if (!ValidateFile(UploadFile))
                    return RedirectToPage();

                try
                {
                    await ProcessAndSaveMediaAsync(UploadFile);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Fel vid filuppladdning: {ex.Message}");
                    MediaList = _context.MediaLibrary.OrderByDescending(m => m.UploadedAt).ToList();
                    return Page();
                }
            }

            return RedirectToPage();
        }

        private bool ValidateFile(IFormFile file)
        {
            if (file.Length > MaxFileSize)
            {
                ModelState.AddModelError("UploadFile", "Filen är för stor. Maximal storlek är 50 MB.");
                return false;
            }

            var ext = Path.GetExtension(file.FileName).ToLower();
            if (!AllowedExtensions.Contains(ext))
            {
                ModelState.AddModelError("UploadFile", "Filtypen är inte tillåten. Använd JPG, PNG, GIF eller WebP.");
                return false;
            }

            return true;
        }

        private async Task ProcessAndSaveMediaAsync(IFormFile uploadFile)
        {
            var fileName = Path.GetFileNameWithoutExtension(uploadFile.FileName);
            var ext = Path.GetExtension(uploadFile.FileName).ToLower();
            var id = Guid.NewGuid();
            var slug = $"{fileName}-{id}{ext}";

            var uploadsPath = Path.Combine(_env.WebRootPath, "uploads");
            var originalDir = Path.Combine(uploadsPath, "originals");
            var smallDir = Path.Combine(uploadsPath, "webp", "small");
            var mediumDir = Path.Combine(uploadsPath, "webp", "medium");
            var largeDir = Path.Combine(uploadsPath, "webp", "large");

            // Ensure directories exist
            Directory.CreateDirectory(originalDir);
            Directory.CreateDirectory(smallDir);
            Directory.CreateDirectory(mediumDir);
            Directory.CreateDirectory(largeDir);

            var originalsPath = Path.Combine(originalDir, slug);

            // Save original
            using (var stream = new FileStream(originalsPath, FileMode.Create))
            {
                await uploadFile.CopyToAsync(stream);
            }

            // Convert to WebP
            var smallPath = Path.Combine(smallDir, $"{fileName}-{id}.webp");
            var mediumPath = Path.Combine(mediumDir, $"{fileName}-{id}.webp");
            var largePath = Path.Combine(largeDir, $"{fileName}-{id}.webp");

            using (var image = SixLabors.ImageSharp.Image.Load(originalsPath))
            {
                image.Mutate(x => x.Resize(400, 0));
                await image.SaveAsWebpAsync(smallPath);

                image.Mutate(x => x.Resize(800, 0));
                await image.SaveAsWebpAsync(mediumPath);

                image.Mutate(x => x.Resize(1600, 0));
                await image.SaveAsWebpAsync(largePath);
            }

            var media = new MediaItem
            {
                Id = id,
                Filename = uploadFile.FileName,
                Slug = slug,
                OriginalPath = $"/uploads/originals/{slug}",
                WebpSmall = $"/uploads/webp/small/{fileName}-{id}.webp",
                WebpMedium = $"/uploads/webp/medium/{fileName}-{id}.webp",
                WebpLarge = $"/uploads/webp/large/{fileName}-{id}.webp",
                UploadedAt = DateTime.UtcNow
            };

            _context.MediaLibrary.Add(media);
            await _context.SaveChangesAsync();
        }
    }
}
