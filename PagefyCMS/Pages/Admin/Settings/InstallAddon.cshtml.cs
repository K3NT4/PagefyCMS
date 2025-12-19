using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagefyCMS.Addons;
using System.Text.Json;

namespace PagefyCMS.Pages.Admin.Settings
{
    public class InstallAddonModel : PageModel
    {
        private readonly AddonInstaller _installer;
        private readonly AddonManager _addonManager;
        private readonly IWebHostEnvironment _env;

        public InstallAddonModel(AddonInstaller installer, AddonManager addonManager, IWebHostEnvironment env)
        {
            _installer = installer;
            _addonManager = addonManager;
            _env = env;
        }

        [BindProperty]
        public IFormFile? UploadFile { get; set; }

        public string? Message { get; set; }
        public bool MessageIsError { get; set; }
        public List<InstalledAddonInfo> InstalledAddons { get; set; } = new();

        public IActionResult OnGet()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("LoggedIn")))
                return RedirectToPage("/Admin/Login");

            LoadInstalledAddons();
            return Page();
        }

        public async Task<IActionResult> OnPostUploadAsync()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("LoggedIn")))
                return RedirectToPage("/Admin/Login");

            if (UploadFile == null || UploadFile.Length == 0)
            {
                Message = "Ingen fil vald";
                MessageIsError = true;
                LoadInstalledAddons();
                return Page();
            }

            var result = await _installer.InstallAddonAsync(UploadFile);
            Message = result.IsSuccess ? result.Message : result.ErrorMessage;
            MessageIsError = !result.IsSuccess;

            LoadInstalledAddons();
            return Page();
        }

        public async Task<IActionResult> OnPostUninstallAsync(string addonId)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("LoggedIn")))
                return RedirectToPage("/Admin/Login");

            var success = await _installer.UninstallAddonAsync(addonId);
            Message = success ? $"Addon '{addonId}' avinstallerad" : $"Kunde inte avinstallera '{addonId}'";
            MessageIsError = !success;

            LoadInstalledAddons();
            return Page();
        }

        public IActionResult OnPostEnableAsync(string addonId)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("LoggedIn")))
                return RedirectToPage("/Admin/Login");

            var isCurrentlyEnabled = _installer.IsAddonEnabled(addonId);
            if (!isCurrentlyEnabled)
            {
                _installer.EnableAddonAsync(addonId).Wait();
                Message = $"Addon '{addonId}' aktiverad ✅";
                MessageIsError = false;
            }
            else
            {
                Message = $"Addon '{addonId}' är redan aktiverad";
                MessageIsError = false;
            }

            LoadInstalledAddons();
            return Page();
        }

        public IActionResult OnPostDisableAsync(string addonId)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("LoggedIn")))
                return RedirectToPage("/Admin/Login");

            var isCurrentlyEnabled = _installer.IsAddonEnabled(addonId);
            if (isCurrentlyEnabled)
            {
                _installer.DisableAddonAsync(addonId).Wait();
                Message = $"Addon '{addonId}' inaktiverad 🔒";
                MessageIsError = false;
            }
            else
            {
                Message = $"Addon '{addonId}' är redan inaktiverad";
                MessageIsError = false;
            }

            LoadInstalledAddons();
            return Page();
        }

        private void LoadInstalledAddons()
        {
            InstalledAddons.Clear();
            var addonsPath = Path.Combine(_env.ContentRootPath, "Addons");

            if (!Directory.Exists(addonsPath))
                return;

            try
            {
                var addonDirs = Directory.GetDirectories(addonsPath);
                foreach (var addonDir in addonDirs)
                {
                    var configPath = Path.Combine(addonDir, "addon.json");
                    if (System.IO.File.Exists(configPath))
                    {
                        try
                        {
                            var json = System.IO.File.ReadAllText(configPath);
                            var config = JsonSerializer.Deserialize<Dictionary<string, object>>(json);

                            var addonId = config?["id"]?.ToString() ?? Path.GetFileName(addonDir);
                            var addon = new InstalledAddonInfo
                            {
                                Id = addonId,
                                Name = config?["name"]?.ToString() ?? "Okänd",
                                Description = config?["description"]?.ToString() ?? "",
                                Version = config?["version"]?.ToString() ?? "1.0.0",
                                Author = config?["author"]?.ToString() ?? "Okänd",
                                FolderName = Path.GetFileName(addonDir),
                                InstallPath = addonDir,
                                IsEnabled = _installer.IsAddonEnabled(addonId)
                            };

                            InstalledAddons.Add(addon);
                        }
                        catch { }
                    }
                }
            }
            catch { }

            InstalledAddons = InstalledAddons.OrderBy(a => a.Name).ToList();
        }
    }

    public class InstalledAddonInfo
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string FolderName { get; set; } = string.Empty;
        public string InstallPath { get; set; } = string.Empty;
        public bool IsEnabled { get; set; }
    }
}
