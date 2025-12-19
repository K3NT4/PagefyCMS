using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagefyCMS.Data;
using PagefyCMS.Models;

namespace PagefyCMS.Pages.Admin.Settings
{
    public class SettingsModel : PageModel
    {
        private readonly PagefyDbContext _context;

        [BindProperty]
        public string SiteTitle { get; set; } = string.Empty;

        [BindProperty]
        public bool EditorEnabled { get; set; }

        [BindProperty]
        public string EditorType { get; set; } = "tinymce";

        [BindProperty]
        public string ActiveTheme { get; set; } = "site";

        public bool Saved { get; set; } = false;

        public SettingsModel(PagefyDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("LoggedIn")))
                return RedirectToPage("/Admin/Login");

            SiteTitle = _context.Settings.FirstOrDefault(s => s.Key == "SiteTitle")?.Value ?? "";
            EditorEnabled = _context.Settings.FirstOrDefault(s => s.Key == "EditorEnabled")?.Value == "true";
            EditorType = _context.Settings.FirstOrDefault(s => s.Key == "EditorType")?.Value ?? "tinymce";
            ActiveTheme = _context.Settings.FirstOrDefault(s => s.Key == "ActiveTheme")?.Value ?? "site";

            return Page();
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("LoggedIn")))
                return RedirectToPage("/Admin/Login");

            // Get or set a default ActiveTheme before saving other settings
            var activeTheme = _context.Settings.FirstOrDefault(s => s.Key == "ActiveTheme")?.Value ?? "framtidsdesign";

            SaveSetting("SiteTitle", SiteTitle, activeTheme);
            SaveSetting("EditorEnabled", EditorEnabled.ToString().ToLower(), activeTheme);
            SaveSetting("EditorType", EditorType, activeTheme);
            SaveSetting("ActiveTheme", ActiveTheme, ActiveTheme);

            Saved = true;
            return Page();
        }

        private void SaveSetting(string key, string value, string activeTheme)
        {
            var setting = _context.Settings.FirstOrDefault(s => s.Key == key);
            if (setting == null)
            {
                _context.Settings.Add(new CmsSetting { Key = key, Value = value, ActiveTheme = activeTheme });
            }
            else
            {
                setting.Value = value;
                // Also update ActiveTheme if this is the ActiveTheme setting
                if (key == "ActiveTheme")
                {
                    setting.ActiveTheme = value;
                }
            }

            _context.SaveChanges();
        }
    }
}
