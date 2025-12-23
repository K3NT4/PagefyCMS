using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagefyCMS.Data;
using PagefyCMS.Models;
using PagefyCMS.Services;

namespace PagefyCMS.Pages.Admin.Articles
{
    public class CreateModel : PageModel
    {
        private readonly PagefyDbContext _context;

        [BindProperty]
        public ArticlePage NewArticle { get; set; } = new ArticlePage();

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
                return Page();

            if (string.IsNullOrWhiteSpace(NewArticle.Slug) && !string.IsNullOrWhiteSpace(NewArticle.Headline))
            {
                NewArticle.Slug = SlugHelper.GenerateSlug(NewArticle.Headline);
            }

            NewArticle.PublishedAt = DateTime.Now;
            NewArticle.UpdatedAt = DateTime.Now;

            _context.Articles.Add(NewArticle);
            _context.SaveChanges();

            return RedirectToPage("./Index");
        }
    }
}
