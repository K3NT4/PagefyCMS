using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagefyCMS.Addons;

namespace PagefyCMS.Pages.Admin.Settings
{
    public class AddonsModel : PageModel
    {
        private readonly AddonManager _addonManager;

        public List<IAddon> LoadedAddons { get; set; } = new();
        public string? Message { get; set; }
        public bool MessageIsError { get; set; }

        public AddonsModel(AddonManager addonManager)
        {
            _addonManager = addonManager;
        }

        public IActionResult OnGet()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("LoggedIn")))
                return RedirectToPage("/Admin/Login");

            LoadedAddons = _addonManager.GetAllAddons().ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostEnableAsync(string addonId)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("LoggedIn")))
                return RedirectToPage("/Admin/Login");

            var success = await _addonManager.EnableAddonAsync(addonId);
            Message = success ? $"Addon '{addonId}' aktiverad" : $"Kunde inte aktivera addon '{addonId}'";
            MessageIsError = !success;

            LoadedAddons = _addonManager.GetAllAddons().ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostDisableAsync(string addonId)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("LoggedIn")))
                return RedirectToPage("/Admin/Login");

            var success = await _addonManager.DisableAddonAsync(addonId);
            Message = success ? $"Addon '{addonId}' deaktiverad" : $"Kunde inte deaktivera addon '{addonId}'";
            MessageIsError = !success;

            LoadedAddons = _addonManager.GetAllAddons().ToList();
            return Page();
        }
    }
}
