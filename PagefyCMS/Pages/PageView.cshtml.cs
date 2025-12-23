using Microsoft.AspNetCore.Mvc.RazorPages;
using PagefyCMS.Data;
using PagefyCMS.Models;

namespace PagefyCMS.Pages
{
    public class PageViewModel : PageModel
    {
        private readonly PagefyDbContext _context;

        public ContentPage? PageData { get; set; }

        public PageViewModel(PagefyDbContext context)
        {
            _context = context;
        }

        public void OnGet(string slug)
        {
            PageData = _context.Pages.FirstOrDefault(p => p.Slug == slug);

            if (PageData != null)
            {
                ViewData["MetaTitle"] = PageData.MetaTitle;
                ViewData["MetaDescription"] = PageData.MetaDescription;
            }
        }
    }
}
