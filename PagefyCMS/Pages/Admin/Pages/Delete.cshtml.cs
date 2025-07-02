using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagefyCMS.Data;
using PagefyCMS.Models;

namespace PagefyCMS.Pages.Admin.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly PagefyDbContext _context;

        public ContentPage? PageData { get; set; }

        public DeleteModel(PagefyDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(int id)
        {
            PageData = _context.Pages.FirstOrDefault(p => p.Id == id);
            if (PageData == null)
                return RedirectToPage("/Admin/Pages/Index");

            return Page();
        }

        public IActionResult OnPost(int id)
        {
            var page = _context.Pages.FirstOrDefault(p => p.Id == id);
            if (page != null)
            {
                _context.Pages.Remove(page);
                _context.SaveChanges();
            }
            return RedirectToPage("/Admin/Pages/Index");
        }
    }
}
