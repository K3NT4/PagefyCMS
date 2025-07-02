using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PagefyCMS.Pages.Admin
{
    public class _AdminLayoutModel : PageModel
    {
        public string AdminName { get; set; } = "Admin";

        public void OnGet()
        {
            // Här kan du ladda data till layouten, t.ex. från sessions eller databas
            // Exempel: AdminName = User.Identity.Name;
        }
    }
}
