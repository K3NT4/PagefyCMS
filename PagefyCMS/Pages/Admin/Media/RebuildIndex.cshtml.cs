using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagefyCMS.Services;

namespace PagefyCMS.Pages.Admin.Media
{
    public class RebuildIndexModel : PageModel
    {
        private readonly AssetUsageService _usageService;

        public RebuildIndexModel(AssetUsageService usageService)
        {
            _usageService = usageService;
        }

        public bool IsDone { get; set; }

        public IActionResult OnGet()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("LoggedIn")))
                return RedirectToPage("/Admin/Login");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("LoggedIn")))
                return RedirectToPage("/Admin/Login");

            await _usageService.RebuildIndexAsync();
            IsDone = true;

            return Page();
        }
    }
}
