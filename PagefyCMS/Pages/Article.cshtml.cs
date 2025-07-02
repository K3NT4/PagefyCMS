using Microsoft.AspNetCore.Mvc.RazorPages;
using PagefyCMS.Data;
using PagefyCMS.Models;
using Microsoft.EntityFrameworkCore;

namespace PagefyCMS.Pages
{
    public class ArticleModel : PageModel
    {
        private readonly PagefyDbContext _context;

        public ArticlePage? ArticleData { get; set; }

        public ArticleModel(PagefyDbContext context)
        {
            _context = context;
        }

        public void OnGet(string slug)
        {
            ArticleData = _context.Articles.FirstOrDefault(a => a.Slug == slug);
        }
    }
}
