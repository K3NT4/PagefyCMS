using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagefyCMS.Services;

namespace PagefyCMS.Pages.Admin.Updates
{
    public class IndexModel : PageModel
    {
        private readonly IVersionService _versionService;
        private readonly IUpdateService _updateService;
        private readonly IWebHostEnvironment _env;

        public IndexModel(IVersionService versionService, IUpdateService updateService, IWebHostEnvironment env)
        {
            _versionService = versionService;
            _updateService = updateService;
            _env = env;
        }

        public string CurrentVersion { get; set; }
        public string EnvironmentName { get; set; }
        public UpdateInfo? UpdateInfo { get; set; }
        public bool DeploymentTriggered { get; set; }

        public void OnGet()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("LoggedIn")))
            {
                Response.Redirect("/Admin/Login");
                return;
            }

            CurrentVersion = _versionService.GetCurrentVersion();
            EnvironmentName = _env.EnvironmentName;
        }

        public async Task<IActionResult> OnPostCheckAsync()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("LoggedIn")))
                return RedirectToPage("/Admin/Login");

            CurrentVersion = _versionService.GetCurrentVersion();
            EnvironmentName = _env.EnvironmentName;

            UpdateInfo = await _updateService.CheckForUpdatesAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostDeployAsync()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("LoggedIn")))
                return RedirectToPage("/Admin/Login");

            CurrentVersion = _versionService.GetCurrentVersion();
            EnvironmentName = _env.EnvironmentName;

            // Re-fetch info to show consistent UI
            UpdateInfo = await _updateService.CheckForUpdatesAsync();

            var success = await _updateService.TriggerDeploymentAsync();
            if (success)
            {
                DeploymentTriggered = true;
            }

            return Page();
        }
    }
}
