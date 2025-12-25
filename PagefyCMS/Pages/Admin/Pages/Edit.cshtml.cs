using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagefyCMS.Data;
using PagefyCMS.Models;
using PagefyCMS.Services;

namespace PagefyCMS.Pages.Admin.Pages
{
    public class EditModel : PageModel
    {
        private readonly PagefyDbContext _context;
        private readonly AssetUsageService _usageService;

        [BindProperty]
        public ContentPage EditPage { get; set; } = new ContentPage();

        public EditModel(PagefyDbContext context, AssetUsageService usageService)
        {
            _context = context;
            _usageService = usageService;
        }

        public IActionResult OnGet(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("LoggedIn")))
                return RedirectToPage("/Admin/Login");

            var page = _context.Pages.FirstOrDefault(p => p.Id == id);
            if (page == null)
                return RedirectToPage("./Index");

            EditPage = page;
            return Page();
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("LoggedIn")))
                return RedirectToPage("/Admin/Login");

            if (!ModelState.IsValid)
                return Page();

            if (string.IsNullOrWhiteSpace(EditPage.Slug) && !string.IsNullOrWhiteSpace(EditPage.Title))
            {
                EditPage.Slug = SlugHelper.GenerateSlug(EditPage.Title);
            }

            EditPage.UpdatedAt = DateTime.Now;

            _context.Pages.Update(EditPage);
            _context.SaveChanges();

            // Index content for asset usage
            _usageService.IndexContentAsync(EditPage.Id, "Page", EditPage.Content, EditPage.Title).Wait();

            return RedirectToPage("./Index");
        }
    }
}
