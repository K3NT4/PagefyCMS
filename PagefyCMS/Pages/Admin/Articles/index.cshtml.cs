using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagefyCMS.Data;
using PagefyCMS.Models;

namespace PagefyCMS.Pages.Admin.Articles
{
    public class IndexModel : PageModel
    {
        private readonly PagefyDbContext _context;

        public List<ArticlePage> Articles { get; set; } = new();

        public IndexModel(PagefyDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("LoggedIn")))
                return RedirectToPage("/Admin/Login");

            Articles = _context.Articles.OrderByDescending(a => a.PublishedAt).ToList();
            return Page();
        }
    }
}
