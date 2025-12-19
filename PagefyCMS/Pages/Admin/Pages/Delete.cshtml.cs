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
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("LoggedIn")))
                return RedirectToPage("/Admin/Login");

            PageData = _context.Pages.FirstOrDefault(p => p.Id == id);
            if (PageData == null)
                return RedirectToPage("/Admin/Pages/Index");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("LoggedIn")))
                return RedirectToPage("/Admin/Login");

            var page = _context.Pages.FirstOrDefault(p => p.Id == id);
            if (page != null)
            {
                _context.Pages.Remove(page);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("/Admin/Pages/Index");
        }
    }
}
