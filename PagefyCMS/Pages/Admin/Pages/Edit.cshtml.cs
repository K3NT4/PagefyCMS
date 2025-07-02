using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagefyCMS.Data;
using PagefyCMS.Models;

namespace PagefyCMS.Pages.Admin.Pages
{
    public class EditModel : PageModel
    {
        private readonly PagefyDbContext _context;

        [BindProperty]
        public ContentPage EditPage { get; set; } = new ContentPage();

        public EditModel(PagefyDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(int id)
        {
            var page = _context.Pages.FirstOrDefault(p => p.Id == id);
            if (page == null)
                return RedirectToPage("./Index");

            EditPage = page;
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            _context.Pages.Update(EditPage);
            _context.SaveChanges();

            return RedirectToPage("./Index");
        }
    }
}
