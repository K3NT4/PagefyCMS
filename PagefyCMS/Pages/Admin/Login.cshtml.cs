using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PagefyCMS.Pages.Admin
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Username { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        public bool LoginFailed { get; set; } = false;

        public IActionResult OnGet()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("LoggedIn")))
                return RedirectToPage("/Admin/Dashboard");

            return Page();
        }

        public IActionResult OnPost()
        {
            if (Username == "admin" && Password == "admin123")
            {
                HttpContext.Session.SetString("LoggedIn", "true");
                return RedirectToPage("/Admin/Dashboard");
            }

            LoginFailed = true;
            return Page();
        }
    }
}
