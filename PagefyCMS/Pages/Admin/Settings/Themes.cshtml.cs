using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagefyCMS.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO.Compression;
using System.IO;

namespace PagefyCMS.Pages.Admin.Settings
{
    public class ThemesModel : PageModel
    {
        private readonly IThemeManager _themeManager;
        private readonly IWebHostEnvironment _environment;

        public List<ThemeInfo> AvailableThemes { get; set; } = new List<ThemeInfo>();
        public string? ActiveThemeId { get; set; }
        public string? Message { get; set; }
        public bool IsSuccess { get; set; }

        public ThemesModel(IThemeManager themeManager, IWebHostEnvironment environment)
        {
            _themeManager = themeManager;
            _environment = environment;
            AvailableThemes = new List<ThemeInfo>();
        }

        public void OnGet()
        {
            // Initialize to empty list to prevent null references
            AvailableThemes = new List<ThemeInfo>();
            
            // Check authentication
            if (User?.Identity?.IsAuthenticated != true)
            {
                RedirectToPage("/Admin/Login");
                return;
            }

            // Get message from query params if redirected
            if (!string.IsNullOrEmpty(Request.Query["message"]))
            {
                Message = Request.Query["message"];
                IsSuccess = Request.Query["success"] == "true";
            }

            try
            {
                var themes = _themeManager.GetAvailableThemes();
                AvailableThemes = themes ?? new List<ThemeInfo>();
                ActiveThemeId = _themeManager.GetActiveThemeId();
            }
            catch
            {
                AvailableThemes = new List<ThemeInfo>();
            }
        }

        public IActionResult OnPostActivateTheme(string themeId)
        {
            if (User?.Identity?.IsAuthenticated != true)
            {
                return Forbid();
            }

            if (string.IsNullOrEmpty(themeId) || !_themeManager.ThemeExists(themeId))
            {
                return RedirectToPage(new { message = "Theme not found!", success = false });
            }

            try
            {
                _themeManager.SetActiveTheme(themeId);
                return RedirectToPage(new { message = $"Theme activated successfully!", success = true });
            }
            catch
            {
                return RedirectToPage(new { message = "Error activating theme", success = false });
            }
        }

        public IActionResult OnPostDeactivateTheme(string themeId)
        {
            if (User?.Identity?.IsAuthenticated != true)
            {
                return Forbid();
            }

            try
            {
                // Deactivate current theme and revert to default
                _themeManager.SetActiveTheme("framtidsdesign");
                return RedirectToPage(new { message = "Theme deactivated, reverted to default", success = true });
            }
            catch
            {
                return RedirectToPage(new { message = "Error deactivating theme", success = false });
            }
        }

        public async Task<IActionResult> OnPostUploadThemeAsync(IFormFile? themeFile)
        {
            if (User?.Identity?.IsAuthenticated != true)
            {
                return Forbid();
            }

            if (themeFile == null || themeFile.Length == 0)
            {
                return RedirectToPage(new { message = "Please select a file", success = false });
            }

            // Validate file is zip
            if (!themeFile.FileName.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToPage(new { message = "Only .zip files are supported", success = false });
            }

            try
            {
                var themesPath = Path.Combine(_environment.WebRootPath, "themes");
                
                // Create temp directory for extraction
                var tempPath = Path.Combine(Path.GetTempPath(), $"theme_{Guid.NewGuid()}");
                Directory.CreateDirectory(tempPath);

                // Save uploaded file
                var zipPath = Path.Combine(tempPath, "theme.zip");
                using (var stream = new FileStream(zipPath, FileMode.Create))
                {
                    await themeFile.CopyToAsync(stream);
                }

                // Extract zip
                ZipFile.ExtractToDirectory(zipPath, tempPath, overwriteFiles: true);

                // Find theme.json to get theme ID
                var themeJsonFiles = Directory.GetFiles(tempPath, "theme.json", SearchOption.AllDirectories);
                if (themeJsonFiles.Length == 0)
                {
                    Directory.Delete(tempPath, true);
                    return RedirectToPage(new { message = "Invalid theme package: theme.json not found", success = false });
                }

                // Get the directory containing theme.json
                var themeSourceDir = Path.GetDirectoryName(themeJsonFiles[0])!;
                
                // Read theme.json to get theme ID
                var themeJsonPath = themeJsonFiles[0];
                var themeJson = System.IO.File.ReadAllText(themeJsonPath);
                var themeInfo = System.Text.Json.JsonSerializer.Deserialize<ThemeInfo>(themeJson, 
                    new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (themeInfo?.Id == null)
                {
                    Directory.Delete(tempPath, true);
                    return RedirectToPage(new { message = "Invalid theme: missing theme ID", success = false });
                }

                // Copy theme to themes directory
                var targetThemeDir = Path.Combine(themesPath, themeInfo.Id);
                
                // Remove existing theme if it exists
                if (Directory.Exists(targetThemeDir))
                {
                    Directory.Delete(targetThemeDir, true);
                }

                // Copy from source to target
                CopyDirectory(themeSourceDir, targetThemeDir);

                // Clean up temp directory
                Directory.Delete(tempPath, true);

                return RedirectToPage(new { message = $"Theme '{themeInfo.Name}' uploaded successfully!", success = true });
            }
            catch (Exception ex)
            {
                return RedirectToPage(new { message = $"Error uploading theme: {ex.Message}", success = false });
            }
        }

        public IActionResult OnPostDeleteTheme(string themeId)
        {
            if (User?.Identity?.IsAuthenticated != true)
            {
                return Forbid();
            }

            if (string.IsNullOrEmpty(themeId) || themeId == "framtidsdesign")
            {
                return RedirectToPage(new { message = "Cannot delete default theme", success = false });
            }

            try
            {
                var themeDir = Path.Combine(_environment.WebRootPath, "themes", themeId);
                
                if (!Directory.Exists(themeDir))
                {
                    return RedirectToPage(new { message = "Theme not found", success = false });
                }

                // If it's the active theme, deactivate it first
                if (_themeManager.GetActiveThemeId() == themeId)
                {
                    _themeManager.SetActiveTheme("framtidsdesign");
                }

                Directory.Delete(themeDir, true);
                return RedirectToPage(new { message = $"Theme deleted successfully!", success = true });
            }
            catch
            {
                return RedirectToPage(new { message = "Error deleting theme", success = false });
            }
        }

        private void CopyDirectory(string sourceDir, string destDir)
        {
            Directory.CreateDirectory(destDir);

            foreach (var file in Directory.GetFiles(sourceDir))
            {
                System.IO.File.Copy(file, Path.Combine(destDir, Path.GetFileName(file)), true);
            }

            foreach (var dir in Directory.GetDirectories(sourceDir))
            {
                CopyDirectory(dir, Path.Combine(destDir, Path.GetFileName(dir)));
            }
        }
    }
}
