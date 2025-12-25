using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagefyCMS.Data;
using PagefyCMS.Models;
using PagefyCMS.Services;

namespace PagefyCMS.Pages.Admin.Articles
{
    public class EditModel : PageModel
    {
        private readonly PagefyDbContext _context;
        private readonly AssetUsageService _usageService;

        [BindProperty]
        public ArticlePage EditArticle { get; set; } = new ArticlePage();

        public EditModel(PagefyDbContext context, AssetUsageService usageService)
        {
            _context = context;
            _usageService = usageService;
        }

        public IActionResult OnGet(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("LoggedIn")))
                return RedirectToPage("/Admin/Login");

            var article = _context.Articles.FirstOrDefault(a => a.Id == id);
            if (article == null)
                return RedirectToPage("./Index");

            EditArticle = article;
            return Page();
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("LoggedIn")))
                return RedirectToPage("/Admin/Login");

            if (!ModelState.IsValid)
                return Page();

            if (string.IsNullOrWhiteSpace(EditArticle.Slug) && !string.IsNullOrWhiteSpace(EditArticle.Headline))
            {
                EditArticle.Slug = SlugHelper.GenerateSlug(EditArticle.Headline);
            }

            EditArticle.UpdatedAt = DateTime.Now;

            _context.Articles.Update(EditArticle);
            _context.SaveChanges();

            // Index content for asset usage
            _usageService.IndexContentAsync(EditArticle.Id, "Article", EditArticle.Content, EditArticle.Headline).Wait();

            return RedirectToPage("./Index");
        }
    }
}
