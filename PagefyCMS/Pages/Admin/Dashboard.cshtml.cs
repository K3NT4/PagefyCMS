using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagefyCMS.Data;
using PagefyCMS.Models;

namespace PagefyCMS.Pages.Admin
{
    public class DashboardModel : PageModel
    {
        private readonly PagefyDbContext _context;

        public int PageCount { get; set; }
        public int ArticleCount { get; set; }
        public int MediaCount { get; set; }

        public DashboardModel(PagefyDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("LoggedIn")))
                return RedirectToPage("/Admin/Login");

            PageCount = _context.Pages.Count();
            ArticleCount = _context.Articles.Count();
            MediaCount = _context.MediaLibrary.Count();

            return Page();
        }
    }
}
