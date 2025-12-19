using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagefyCMS.Data;
using PagefyCMS.Models;

namespace PagefyCMS.Pages.Admin.Articles
{
    public class DeleteArticleModel : PageModel
    {
        private readonly PagefyDbContext _context;

        public ArticlePage? ArticleData { get; set; }

        public DeleteArticleModel(PagefyDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("LoggedIn")))
                return RedirectToPage("/Admin/Login");

            ArticleData = _context.Articles.FirstOrDefault(a => a.Id == id);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("LoggedIn")))
                return RedirectToPage("/Admin/Login");

            var article = _context.Articles.FirstOrDefault(a => a.Id == id);
            if (article != null)
            {
                _context.Articles.Remove(article);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("/Admin/Articles/Index");
        }
    }
}
