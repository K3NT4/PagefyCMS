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

        public bool Saved { get; set; } = false;

        public SettingsModel(PagefyDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            SiteTitle = _context.Settings.FirstOrDefault(s => s.Key == "SiteTitle")?.Value ?? "";
            EditorEnabled = _context.Settings.FirstOrDefault(s => s.Key == "EditorEnabled")?.Value == "true";
            EditorType = _context.Settings.FirstOrDefault(s => s.Key == "EditorType")?.Value ?? "tinymce";
        }

        public IActionResult OnPost()
        {
            SaveSetting("SiteTitle", SiteTitle);
            SaveSetting("EditorEnabled", EditorEnabled.ToString().ToLower());
            SaveSetting("EditorType", EditorType);

            Saved = true;
            return Page();
        }

        private void SaveSetting(string key, string value)
        {
            var setting = _context.Settings.FirstOrDefault(s => s.Key == key);
            if (setting == null)
                _context.Settings.Add(new CmsSetting { Key = key, Value = value });
            else
                setting.Value = value;

            _context.SaveChanges();
        }
    }
}
