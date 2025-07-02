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

        public void OnGet()
        {
            Pages = _context.Pages.OrderByDescending(p => p.CreatedAt).ToList();
        }
    }
}
