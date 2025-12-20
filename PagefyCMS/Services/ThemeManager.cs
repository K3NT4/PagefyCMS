using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Linq;
using PagefyCMS.Data;
using PagefyCMS.Models;

namespace PagefyCMS.Services
{
    /// <summary>
    /// Interface for theme management operations.
    /// </summary>
    public interface IThemeManager
    {
        List<ThemeInfo> GetAvailableThemes();
        ThemeInfo? GetThemeInfo(string themeId);
        string? GetActiveThemeId();
        void SetActiveTheme(string themeId);
        bool ThemeExists(string themeId);
    }

    /// <summary>
    /// Theme information metadata.
    /// </summary>
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

    /// <summary>
    /// Theme color palette information.
    /// </summary>
    public class ThemeColors
    {
        public string? Primary { get; set; }
        public string? Secondary { get; set; }
        public string? Accent { get; set; }
    }

    /// <summary>
    /// Theme feature flags.
    /// </summary>
    public class ThemeFeatures
    {
        public bool Animations { get; set; }
        public bool DarkMode { get; set; }
        public bool GlowEffects { get; set; }
        public bool ResponsiveDesign { get; set; }
        public bool CustomFont { get; set; }
    }

    /// <summary>
    /// Theme manager - handles theme loading, switching and caching.
    /// </summary>
    public class ThemeManager : IThemeManager
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly PagefyDbContext _dbContext;
        private List<ThemeInfo>? _themesCache;
        private const string DefaultTheme = "framtidsdesign";

        public ThemeManager(IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor, PagefyDbContext dbContext)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        /// <summary>
        /// Get all available themes, using cache when possible.
        /// </summary>
        public List<ThemeInfo> GetAvailableThemes()
        {
            // Return cached themes if available
            if (_themesCache != null)
                return _themesCache;

            try
            {
                var themesPath = Path.Combine(_environment.WebRootPath, "themes");
                var themes = new List<ThemeInfo>();

                if (!Directory.Exists(themesPath))
                {
                    _themesCache = themes;
                    return themes;
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

                _themesCache = themes.OrderBy(t => t.Name).ToList();
                return _themesCache;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading themes: {ex.Message}");
                return new List<ThemeInfo>();
            }
        }

        /// <summary>
        /// Get theme information by ID.
        /// </summary>
        public ThemeInfo? GetThemeInfo(string themeId)
        {
            if (string.IsNullOrWhiteSpace(themeId))
                return null;

            var themePath = Path.Combine(_environment.WebRootPath, "themes", themeId);
            var themeJsonPath = Path.Combine(themePath, "theme.json");

            if (File.Exists(themeJsonPath))
            {
                return GetThemeInfoFromJson(themeJsonPath);
            }

            return null;
        }

        /// <summary>
        /// Get the currently active theme ID.
        /// </summary>
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
            catch (Exception ex)
            {
                // If database access fails, return default
                System.Diagnostics.Debug.WriteLine($"Error getting active theme: {ex.Message}");
            }

            // Default theme fallback
            return DefaultTheme;
        }

        /// <summary>
        /// Set the active theme. Validates that the theme exists before saving.
        /// </summary>
        public void SetActiveTheme(string themeId)
        {
            if (string.IsNullOrWhiteSpace(themeId))
                throw new ArgumentException("Theme ID cannot be empty", nameof(themeId));

            if (!ThemeExists(themeId))
            {
                throw new ArgumentException($"Theme '{themeId}' does not exist", nameof(themeId));
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
                
                // Clear cache after theme change
                _themesCache = null;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to save active theme to database: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Check if a theme exists in the file system.
        /// </summary>
        public bool ThemeExists(string themeId)
        {
            if (string.IsNullOrWhiteSpace(themeId))
                return false;

            var themePath = Path.Combine(_environment.WebRootPath, "themes", themeId);
            return Directory.Exists(themePath);
        }

        /// <summary>
        /// Load theme metadata from theme.json file.
        /// </summary>
        private ThemeInfo? GetThemeInfoFromJson(string jsonPath)
        {
            try
            {
                if (!File.Exists(jsonPath))
                    return null;

                var json = File.ReadAllText(jsonPath);
                var options = new JsonSerializerOptions 
                { 
                    PropertyNameCaseInsensitive = true 
                };
                return JsonSerializer.Deserialize<ThemeInfo>(json, options);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error reading theme.json {jsonPath}: {ex.Message}");
                return null;
            }
        }
    }
}
