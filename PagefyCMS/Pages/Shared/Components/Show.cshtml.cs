using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagefyCMS.Data;
using PagefyCMS.Models;

namespace PagefyCMS.Pages
{
    public class ShowModel : PageModel
    {
        private readonly PagefyDbContext _context;

        public ContentPage? Page { get; set; }
        public List<MediaItem> GalleryImages { get; set; } = new();

        public ShowModel(PagefyDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(string slug)
        {
            Page = _context.Pages.FirstOrDefault(p => p.Slug == slug);

            if (Page == null)
                return NotFound();

            if (!string.IsNullOrWhiteSpace(Page.GalleryGroup))
            {
                GalleryImages = _context.MediaLibrary
                    .Where(m => m.GalleryGroup == Page.GalleryGroup)
                    .ToList();
            }

            return Page();
        }
    }
}
