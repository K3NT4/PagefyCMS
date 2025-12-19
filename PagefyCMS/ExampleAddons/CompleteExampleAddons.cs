using PagefyCMS.Addons;

namespace PagefyCMS.ExampleAddons
{
    /// <summary>
    /// KOMPLETT EXEMPEL-ADDON: Watermark på alla bilder
    /// 
    /// Denna addon visar:
    /// - Grundläggande addon-struktur
    /// - Hook-användning
    /// - Konfiguration
    /// - Error handling
    /// - Logging
    /// </summary>
    public class WatermarkAddon : HookableAddon
    {
        private readonly ILogger<WatermarkAddon> _logger;
        private string _watermarkText = "© PagefyCMS";
        private bool _enabled = false;

        public WatermarkAddon(ILogger<WatermarkAddon> logger)
        {
            _logger = logger;
        }

        // Addon-metadata
        public override string Id => "com.pagefy.watermark";
        public override string Name => "Watermark";
        public override string Description => "Lägger till vattenmärke på alla bilder";
        public override string Version => "1.0.0";
        public override string Author => "PagefyCMS Team";

        // Vilka hooks denna addon vill lyssna på
        public override IEnumerable<AddonHook> SupportedHooks => new[]
        {
            AddonHook.BeforeMediaSave,    // Innan bild sparas
            AddonHook.SystemInitialize    // Vid systemstart
        };

        // Körs när addonen aktiveras
        public override async Task InitializeAsync()
        {
            try
            {
                _logger.LogInformation("🖼️ Watermark Addon initialiseras...");

                // Läs konfiguration (om den finns)
                LoadConfiguration();

                _enabled = true;
                await base.InitializeAsync();

                _logger.LogInformation("✅ Watermark Addon aktiverad. Text: '{Text}'", _watermarkText);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Fel vid initialisering av Watermark Addon");
                _enabled = false;
            }
        }

        // Körs när addonen deaktiveras
        public override async Task ShutdownAsync()
        {
            _logger.LogInformation("🛑 Watermark Addon stängs ner");
            await base.ShutdownAsync();
        }

        // Denna körs när någon av hooksen utlöses
        public override async Task ExecuteHookAsync(HookContext context)
        {
            if (!_enabled)
            {
                _logger.LogWarning("⚠️ Watermark Addon är disabled");
                return;
            }

            try
            {
                switch (context.Hook)
                {
                    case AddonHook.BeforeMediaSave:
                        await HandleMediaSaveAsync(context);
                        break;

                    case AddonHook.SystemInitialize:
                        await HandleSystemInitializeAsync(context);
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Fel vid körning av hook {Hook}", context.Hook);
            }
        }

        // Hook: BeforeMediaSave - Lägg till vattenmärke
        private async Task HandleMediaSaveAsync(HookContext context)
        {
            _logger.LogInformation("📝 BeforeMediaSave hook aktiverad");

            if (context.Data.TryGetValue("MediaItem", out var mediaObj))
            {
                if (mediaObj is not null)
                {
                    var filename = mediaObj?.GetType().GetProperty("Filename")?.GetValue(mediaObj)?.ToString() ?? "";
                    
                    // Kontrollera att det är en bildfil
                    if (IsImageFile(filename))
                    {
                        _logger.LogInformation("🖼️ Lägger till vattenmärke på: {Filename}", filename);
                        
                        // Lägg till vattenmärke-metadata
                        context.Data["Watermarked"] = true;
                        context.Data["WatermarkText"] = _watermarkText;
                    }
                }
            }

            await Task.CompletedTask;
        }

        // Hook: SystemInitialize - Visa status
        private async Task HandleSystemInitializeAsync(HookContext context)
        {
            _logger.LogInformation("🚀 SystemInitialize hook aktiverad");
            _logger.LogInformation("   Status: {Status}", _enabled ? "Aktiv" : "Inaktiv");
            _logger.LogInformation("   Vattenmärke: {Text}", _watermarkText);

            await Task.CompletedTask;
        }

        // Ladda konfiguration (simulerat exempel)
        private void LoadConfiguration()
        {
            // I verklig kod skulle du läsa från appsettings.json:
            // var config = _configuration.GetSection("Addons:Watermark");
            // _watermarkText = config["Text"] ?? "© PagefyCMS";

            _logger.LogInformation("📋 Läser konfiguration...");
        }

        // Hjälpmetod: Kontrollera om fil är bild
        private bool IsImageFile(string filename)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var ext = Path.GetExtension(filename).ToLower();
            return allowedExtensions.Contains(ext);
        }

        // Offentlig API för andra addons
        public void SetWatermarkText(string text)
        {
            _watermarkText = text;
            _logger.LogInformation("🔄 Vattenmärke-text uppdaterad: {Text}", text);
        }

        public string GetWatermarkText() => _watermarkText;
        public bool IsAddonEnabled() => _enabled;

        // Statistik-metod
        public Dictionary<string, object> GetAddonStats()
        {
            return new Dictionary<string, object>
            {
                ["Enabled"] = _enabled,
                ["Version"] = Version,
                ["WatermarkText"] = _watermarkText,
                ["LoadedAt"] = DateTime.UtcNow
            };
        }
    }

    /// <summary>
    /// AVANCERAT EXEMPEL: Addon som använder databas
    /// </summary>
    public class AnalyticsAddon : HookableAddon
    {
        private readonly ILogger<AnalyticsAddon> _logger;
        private List<AnalyticsEvent> _events = new();

        public AnalyticsAddon(ILogger<AnalyticsAddon> logger)
        {
            _logger = logger;
        }

        public override string Id => "com.pagefy.analytics";
        public override string Name => "Analytics";
        public override string Description => "Samlar statistik över CMS-användning";
        public override string Version => "1.0.0";
        public override string Author => "PagefyCMS Team";

        public override IEnumerable<AddonHook> SupportedHooks => new[]
        {
            AddonHook.BeforePageSave,
            AddonHook.BeforeArticleSave,
            AddonHook.BeforeMediaSave,
            AddonHook.BeforeMediaDelete
        };

        public override async Task InitializeAsync()
        {
            _logger.LogInformation("📊 Analytics Addon startar...");
            _events.Clear();
            await base.InitializeAsync();
        }

        public override async Task ExecuteHookAsync(HookContext context)
        {
            var evt = new AnalyticsEvent
            {
                Hook = context.Hook,
                UserId = context.UserId ?? "Unknown",
                Timestamp = context.ExecutedAt,
                DataCount = context.Data.Count
            };

            _events.Add(evt);

            _logger.LogInformation(
                "📊 EVENT: {Hook} | User: {User} | Data Points: {Count}",
                context.Hook,
                evt.UserId,
                evt.DataCount
            );

            await Task.CompletedTask;
        }

        // Hämta statistik
        public IEnumerable<AnalyticsEvent> GetEvents() => _events.AsReadOnly();

        public Dictionary<string, int> GetEventCounts()
        {
            return _events
                .GroupBy(e => e.Hook)
                .ToDictionary(g => g.Key.ToString(), g => g.Count());
        }

        public override async Task ShutdownAsync()
        {
            _logger.LogInformation("📊 Analytics: Samlade {Count} events", _events.Count);
            await base.ShutdownAsync();
        }
    }

    // Hjälpklasser
    public class AnalyticsEvent
    {
        public AddonHook Hook { get; set; }
        public string UserId { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public int DataCount { get; set; }
    }
}
