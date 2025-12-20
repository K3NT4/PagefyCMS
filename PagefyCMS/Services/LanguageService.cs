using System.Text.Json;
using System.IO;

namespace PagefyCMS.Services
{
    public interface ILanguageService
    {
        string Get(string key, string defaultValue = "");
        Dictionary<string, string> LoadLanguage(string languageName);
        List<string> GetAvailableLanguages();
        bool UploadLanguage(string languageName, string jsonContent);
        string GetLanguageTemplate();
    }

    public class LanguageService : ILanguageService
    {
        private readonly IWebHostEnvironment _environment;
        private Dictionary<string, string> _currentLanguage;
        private readonly string _languagesPath;

        public LanguageService(IWebHostEnvironment environment)
        {
            _environment = environment;
            _languagesPath = Path.Combine(_environment.WebRootPath, "languages");
            
            // Create languages directory if it doesn't exist
            if (!Directory.Exists(_languagesPath))
            {
                Directory.CreateDirectory(_languagesPath);
            }

            // Load default English language
            _currentLanguage = LoadLanguage("en") ?? new Dictionary<string, string>();
        }

        public string Get(string key, string defaultValue = "")
        {
            return _currentLanguage.ContainsKey(key) ? _currentLanguage[key] : defaultValue;
        }

        public Dictionary<string, string> LoadLanguage(string languageName)
        {
            try
            {
                string filePath = Path.Combine(_languagesPath, $"{languageName}.json");
                if (File.Exists(filePath))
                {
                    string json = File.ReadAllText(filePath);
                    return JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();
                }
            }
            catch
            {
                // If loading fails, return empty dictionary
            }
            return new Dictionary<string, string>();
        }

        public List<string> GetAvailableLanguages()
        {
            try
            {
                if (!Directory.Exists(_languagesPath))
                    return new List<string> { "en" };

                return Directory.GetFiles(_languagesPath, "*.json")
                    .Select(f => Path.GetFileNameWithoutExtension(f))
                    .ToList();
            }
            catch
            {
                return new List<string> { "en" };
            }
        }

        public bool UploadLanguage(string languageName, string jsonContent)
        {
            try
            {
                // Validate JSON
                var options = new JsonSerializerOptions();
                JsonSerializer.Deserialize<Dictionary<string, string>>(jsonContent, options);

                // Save file
                string filePath = Path.Combine(_languagesPath, $"{languageName}.json");
                File.WriteAllText(filePath, jsonContent);
                
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string GetLanguageTemplate()
        {
            var template = new Dictionary<string, string>
            {
                // Frontend
                { "welcome", "Welcome" },
                { "discover_articles", "Discover our latest articles and news" },
                { "read_more", "Read more" },
                { "no_articles", "No articles yet" },
                { "no_articles_message", "There are no published articles at the moment" },
                { "important_pages", "Important Pages" },
                { "menu", "Menu" },
                { "hide_sidebar", "Hide sidebar" },
                { "home", "Home" },
                { "modern_cms", "Modern content management system built for performance and flexibility" },
                { "quick_links", "Quick Links" },
                { "follow_us", "Follow us" },

                // Admin
                { "dashboard", "Dashboard" },
                { "pages", "Pages" },
                { "articles", "Articles" },
                { "media", "Media" },
                { "addons", "Addons" },
                { "themes", "Themes" },
                { "settings", "Settings" },
                { "logout", "Logout" },
                { "back_to_website", "← Back to Website" },
                
                // Settings page
                { "site_settings", "Site Settings" },
                { "settings_saved", "Settings saved successfully!" },
                { "basic_settings", "Basic Settings" },
                { "site_name", "Site Name" },
                { "editor_choice", "Editor Choice" },
                { "enable_editor", "Enable editor for pages and articles" },
                { "theme_select", "Theme" },
                { "choose_theme", "Choose theme for website" },
                { "framtidsdesign", "🎨 Future Design (Default)" },
                { "neon_cyberpunk", "⚡ Neon Cyberpunk" },
                { "professional_midnight", "💼 Professional Midnight" },
                { "news_hub", "📰 News Hub" },
                { "gaming_edge", "🎮 Gaming Edge" },
                { "minimalist_clean", "✨ Minimalist Clean" },
                { "aurora_gradient", "🌌 Aurora Gradient" },
                { "save_changes", "💾 Save Changes" },
            };

            var options = new JsonSerializerOptions { WriteIndented = true };
            return JsonSerializer.Serialize(template, options);
        }
    }
}
