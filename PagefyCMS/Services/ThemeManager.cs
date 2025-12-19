using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Linq;
using PagefyCMS.Data;
using PagefyCMS.Models;

namespace PagefyCMS.Services
{
    public interface IThemeManager
    {
        List<ThemeInfo> GetAvailableThemes();
        ThemeInfo? GetThemeInfo(string themeId);
        string? GetActiveThemeId();
        void SetActiveTheme(string themeId);
        bool ThemeExists(string themeId);
    }

    public class ThemeInfo
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Version { get; set; }
        public string? Author { get; set; }
        public string? AuthorUri { get; set; }
        public string? License { get; set; }
        public string? Homepage { get; set; }
        public List<string>? Tags { get; set; }
        public ThemeColors? Colors { get; set; }
        public ThemeFeatures? Features { get; set; }
        public string? Screenshot { get; set; }
        public string? StyleFile { get; set; }
    }

    public class ThemeColors
    {
        public string? Primary { get; set; }
        public string? Secondary { get; set; }
        public string? Accent { get; set; }
    }

    public class ThemeFeatures
    {
        public bool Animations { get; set; }
        public bool DarkMode { get; set; }
        public bool GlowEffects { get; set; }
        public bool ResponsiveDesign { get; set; }
        public bool CustomFont { get; set; }
    }

    public class ThemeManager : IThemeManager
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly PagefyDbContext _dbContext;

        public ThemeManager(IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor, PagefyDbContext dbContext)
        {
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
        }

        public List<ThemeInfo> GetAvailableThemes()
        {
            try
            {
                var themesPath = Path.Combine(_environment.WebRootPath, "themes");
                var themes = new List<ThemeInfo>();

                if (!Directory.Exists(themesPath))
                {
                    return themes ?? new List<ThemeInfo>();
                }

                foreach (var themeDir in Directory.GetDirectories(themesPath))
                {
                    var themeJsonPath = Path.Combine(themeDir, "theme.json");
                    if (File.Exists(themeJsonPath))
                    {
                        var theme = GetThemeInfoFromJson(themeJsonPath);
                        if (theme != null)
                        {
                            themes.Add(theme);
                        }
                    }
                }

                var result = themes.OrderBy(t => t.Name).ToList();
                return result ?? new List<ThemeInfo>();
            }
            catch
            {
                return new List<ThemeInfo>();
            }
        }

        public ThemeInfo? GetThemeInfo(string themeId)
        {
            var themePath = Path.Combine(_environment.WebRootPath, "themes", themeId);
            var themeJsonPath = Path.Combine(themePath, "theme.json");

            if (File.Exists(themeJsonPath))
            {
                return GetThemeInfoFromJson(themeJsonPath);
            }

            return null;
        }

        public string GetActiveThemeId()
        {
            try
            {
                var setting = _dbContext.Settings.FirstOrDefault(s => s.Key == "ActiveTheme");
                
                if (setting != null && !string.IsNullOrEmpty(setting.Value))
                {
                    // Validate that the theme still exists
                    if (ThemeExists(setting.Value))
                    {
                        return setting.Value;
                    }
                }
            }
            catch
            {
                // If database access fails, return default
            }

            // Default theme
            return "framtidsdesign";
        }

        public void SetActiveTheme(string themeId)
        {
            if (!ThemeExists(themeId))
            {
                throw new ArgumentException($"Theme '{themeId}' does not exist");
            }

            try
            {
                var setting = _dbContext.Settings.FirstOrDefault(s => s.Key == "ActiveTheme");
                
                if (setting == null)
                {
                    setting = new CmsSetting { Key = "ActiveTheme", Value = themeId, ActiveTheme = themeId };
                    _dbContext.Settings.Add(setting);
                }
                else
                {
                    setting.Value = themeId;
                    setting.ActiveTheme = themeId;
                }
                
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to save active theme to database", ex);
            }
        }

        public bool ThemeExists(string themeId)
        {
            var themePath = Path.Combine(_environment.WebRootPath, "themes", themeId);
            return Directory.Exists(themePath);
        }

        private ThemeInfo? GetThemeInfoFromJson(string jsonPath)
        {
            try
            {
                var json = File.ReadAllText(jsonPath);
                var options = new JsonSerializerOptions 
                { 
                    PropertyNameCaseInsensitive = true 
                };
                return JsonSerializer.Deserialize<ThemeInfo>(json, options);
            }
            catch
            {
                return null;
            }
        }
    }
}
