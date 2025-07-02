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
            ArticleData = _context.Articles.FirstOrDefault(a => a.Id == id);
            return Page();
        }

        public IActionResult OnPost(int id)
        {
            var article = _context.Articles.FirstOrDefault(a => a.Id == id);
            if (article != null)
            {
                _context.Articles.Remove(article);
                _context.SaveChanges();
            }
            return RedirectToPage("/Admin/Articles/Index");
        }
    }
}
