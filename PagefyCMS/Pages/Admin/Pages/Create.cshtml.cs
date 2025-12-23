using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagefyCMS.Data;
using PagefyCMS.Models;
using PagefyCMS.Services;

namespace PagefyCMS.Pages.Admin.Pages
{
    public class CreateModel : PageModel
    {
        private readonly PagefyDbContext _context;

        [BindProperty]
        public ContentPage NewPage { get; set; } = new ContentPage();

        public CreateModel(PagefyDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("LoggedIn")))
                return RedirectToPage("/Admin/Login");

            return Page();
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("LoggedIn")))
                return RedirectToPage("/Admin/Login");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (string.IsNullOrWhiteSpace(NewPage.Slug) && !string.IsNullOrWhiteSpace(NewPage.Title))
            {
                NewPage.Slug = SlugHelper.GenerateSlug(NewPage.Title);
            }

            NewPage.CreatedAt = DateTime.Now;
            NewPage.UpdatedAt = DateTime.Now;

            if (string.IsNullOrWhiteSpace(NewPage.GalleryGroup))
            {
                NewPage.GalleryGroup = null;
            }

            _context.Pages.Add(NewPage);
            _context.SaveChanges();

            return RedirectToPage("./Index");
        }
    }
}
