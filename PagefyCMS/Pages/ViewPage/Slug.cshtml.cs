using Microsoft.AspNetCore.Mvc.RazorPages;
using PagefyCMS.Data;
using PagefyCMS.Models;

namespace PagefyCMS.Pages.ViewPage
{
    public class SlugModel : PageModel
    {
        private readonly PagefyDbContext _context;

        public ContentPage? PageData { get; set; }

        public SlugModel(PagefyDbContext context)
        {
            _context = context;
        }

        public void OnGet(string slug)
        {
            PageData = _context.Pages.FirstOrDefault(p => p.Slug == slug);
        }
    }
}
