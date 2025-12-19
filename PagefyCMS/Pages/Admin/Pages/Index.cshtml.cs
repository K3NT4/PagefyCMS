using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagefyCMS.Data;
using PagefyCMS.Models;

namespace PagefyCMS.Pages.Admin.Pages
{
    public class IndexModel : PageModel
    {
        private readonly PagefyDbContext _context;

        public List<ContentPage> Pages { get; set; } = new();

        public IndexModel(PagefyDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("LoggedIn")))
                return RedirectToPage("/Admin/Login");

            Pages = _context.Pages.OrderByDescending(p => p.CreatedAt).ToList();
            return Page();
        }
    }
}
