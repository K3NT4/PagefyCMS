using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagefyCMS.Data;
using PagefyCMS.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace PagefyCMS.Pages.Admin.Media
{
    public class UploadModel : PageModel
    {
        private readonly IWebHostEnvironment _env;
        private readonly PagefyDbContext _context;

        public UploadModel(IWebHostEnvironment env, PagefyDbContext context)
        {
            _env = env;
            _context = context;
        }

        [BindProperty]
        public IFormFile UploadFile { get; set; }

        [BindProperty]
        public string Title { get; set; }

        [BindProperty]
        public string AltText { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (UploadFile == null || UploadFile.Length == 0)
            {
                ModelState.AddModelError(string.Empty, "Ingen fil vald.");
                return Page();
            }

            var fileName = Path.GetFileNameWithoutExtension(UploadFile.FileName);
            var ext = Path.GetExtension(UploadFile.FileName).ToLower();
            var id = Guid.NewGuid();
            var slug = $"{fileName}-{id}{ext}";

            if (string.IsNullOrWhiteSpace(Title))
                Title = "";

            if (string.IsNullOrWhiteSpace(AltText))
                AltText = fileName;

            var uploadsPath = Path.Combine(_env.WebRootPath, "uploads");
            Directory.CreateDirectory(Path.Combine(uploadsPath, "originals"));
            Directory.CreateDirectory(Path.Combine(uploadsPath, "webp", "small"));
            Directory.CreateDirectory(Path.Combine(uploadsPath, "webp", "medium"));
            Directory.CreateDirectory(Path.Combine(uploadsPath, "webp", "large"));

            var originalsPath = Path.Combine(uploadsPath, "originals", slug);

            using (var stream = new FileStream(originalsPath, FileMode.Create))
            {
                await UploadFile.CopyToAsync(stream);
            }

            var smallPath = Path.Combine(uploadsPath, "webp", "small", $"{fileName}-{id}.webp");
            var mediumPath = Path.Combine(uploadsPath, "webp", "medium", $"{fileName}-{id}.webp");
            var largePath = Path.Combine(uploadsPath, "webp", "large", $"{fileName}-{id}.webp");

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
                Filename = UploadFile.FileName,
                Slug = slug,
                Title = Title ?? "",
                AltText = string.IsNullOrWhiteSpace(AltText) ? fileName : AltText,
                Description = "",  // Viktigt, tom sträng så det inte blir null
                OriginalPath = $"/uploads/originals/{slug}",
                WebpSmall = $"/uploads/webp/small/{fileName}-{id}.webp",
                WebpMedium = $"/uploads/webp/medium/{fileName}-{id}.webp",
                WebpLarge = $"/uploads/webp/large/{fileName}-{id}.webp",
                UploadedAt = DateTime.UtcNow
            };

            _context.MediaLibrary.Add(media);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Admin/Media/Index");
        }
    }
}
