using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagefyCMS.Data;
using PagefyCMS.Models;

namespace PagefyCMS.Pages.Admin.Media
{
    public class EditModel : PageModel
    {
        private readonly PagefyDbContext _context;

        public EditModel(PagefyDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Guid Id { get; set; }

        [BindProperty]
        public string Title { get; set; } = "";

        [BindProperty]
        public string AltText { get; set; } = "";

        [BindProperty]
        public string Description { get; set; } = "";

        public MediaItem Media { get; set; }

        public IActionResult OnGet(Guid id)
        {
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
            var media = _context.MediaLibrary.FirstOrDefault(m => m.Id == Id);
            if (media == null)
            {
                return NotFound();
            }

            media.Title = string.IsNullOrWhiteSpace(Title) ? "" : Title;
            media.AltText = string.IsNullOrWhiteSpace(AltText) ? "" : AltText;
            media.Description = string.IsNullOrWhiteSpace(Description) ? "" : Description;

            await _context.SaveChangesAsync();

            return RedirectToPage("/Admin/Media/Edit", new { id = media.Id });
        }
    }
}
