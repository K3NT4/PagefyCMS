using Microsoft.AspNetCore.Mvc.RazorPages;
using PagefyCMS.Data;
using PagefyCMS.Models;
using Microsoft.EntityFrameworkCore;

namespace PagefyCMS.Pages
{
    public class IndexModel : PageModel
    {
        private readonly PagefyDbContext _context;

        public List<ArticlePage> Articles { get; set; } = new();

        public IndexModel(PagefyDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            Articles = _context.Articles
                .OrderByDescending(a => a.PublishedAt)
                .Take(10)
                .ToList();
        }
    }
}
