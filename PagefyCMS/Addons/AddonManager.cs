namespace PagefyCMS.Addons
{
    /// <summary>
    /// Hanterar inladdning och hantering av addons
    /// </summary>
    public class AddonManager
    {
        private readonly Dictionary<string, IAddon> _loadedAddons = new();
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<AddonManager> _logger;
        private readonly string _addonsPath;

        public AddonManager(IServiceProvider serviceProvider, ILogger<AddonManager> logger, IWebHostEnvironment env)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _addonsPath = Path.Combine(env.ContentRootPath, "Addons");
        }

        /// <summary>
        /// Laddar alla addons från addons-mappen
        /// </summary>
        public async Task LoadAddonsAsync()
        {
            try
            {
                if (!Directory.Exists(_addonsPath))
                {
                    Directory.CreateDirectory(_addonsPath);
                    _logger.LogInformation("Skapade Addons-mapp: {AddonPath}", _addonsPath);
                    return;
                }

            // Leta efter addon-mappar
            var addonDirs = Directory.GetDirectories(_addonsPath);
            foreach (var addonDir in addonDirs)
            {
                await LoadAddonFromDirectoryAsync(addonDir);
            }

            _logger.LogInformation("Laddade {Count} addons", _loadedAddons.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fel vid laddning av addons");
        }
    }

    /// <summary>
    /// Laddar en addon från en specifik mapp
    /// </summary>
    private async Task LoadAddonFromDirectoryAsync(string addonDirectory)
    {
        try
        {
            var configPath = Path.Combine(addonDirectory, "addon.json");
            if (!File.Exists(configPath))
                return;

            var configJson = await File.ReadAllTextAsync(configPath);
            var config = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(configJson);

            if (config == null)
                return;

            var addonId = config?["id"]?.ToString() ?? Path.GetFileName(addonDirectory);
            _logger.LogInformation("Hittade addon: {AddonId} i {AddonDir}", addonId, addonDirectory);

            var addon = new DynamicAddon(
                addonId ?? "unknown",
                config?["name"]?.ToString() ?? addonId ?? "Unknown",
                config?["description"]?.ToString() ?? "No description",
                config?["version"]?.ToString() ?? "1.0.0",
                config?["author"]?.ToString() ?? "PagefyCMS"
            );

            if (_loadedAddons.ContainsKey(addon.Id))
            {
                _logger.LogWarning("Addon {AddonId} är redan registrerad", addon.Id);
                return;
            }

            _loadedAddons[addon.Id] = addon;
            _logger.LogInformation("Registrerad addon: {AddonId} v{Version}", addon.Id, addon.Version);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fel vid laddning av addon från {AddonDir}", addonDirectory);
        }
    }

        /// <summary>
        /// Hämtar en registrerad addon
        /// </summary>
        public IAddon? GetAddon(string addonId)
        {
            _loadedAddons.TryGetValue(addonId, out var addon);
            return addon;
        }

        /// <summary>
        /// Hämtar alla registrerade addons
        /// </summary>
        public IEnumerable<IAddon> GetAllAddons()
        {
            return _loadedAddons.Values;
        }

        /// <summary>
        /// Aktiverar en addon
        /// </summary>
        public async Task<bool> EnableAddonAsync(string addonId)
        {
            var addon = GetAddon(addonId);
            if (addon == null)
            {
                _logger.LogWarning("Addon {AddonId} hittades inte", addonId);
                return false;
            }

            try
            {
                await addon.InitializeAsync();
                _logger.LogInformation("Aktiverade addon: {AddonId}", addonId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fel vid aktivering av addon {AddonId}", addonId);
                return false;
            }
        }

        /// <summary>
        /// Deaktiverar en addon
        /// </summary>
        public async Task<bool> DisableAddonAsync(string addonId)
        {
            var addon = GetAddon(addonId);
            if (addon == null)
            {
                _logger.LogWarning("Addon {AddonId} hittades inte", addonId);
                return false;
            }

            try
            {
                await addon.ShutdownAsync();
                _logger.LogInformation("Deaktiverade addon: {AddonId}", addonId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fel vid deaktivering av addon {AddonId}", addonId);
                return false;
            }
        }

        /// <summary>
        /// Utför en hook för alla registrerade addons som support den
        /// </summary>
        public async Task ExecuteHookAsync(HookContext context)
        {
            var hookableAddons = _loadedAddons.Values
                .OfType<IHookableAddon>()
                .Where(a => a.IsEnabled && a.SupportedHooks.Contains(context.Hook));

            foreach (var addon in hookableAddons)
            {
                try
                {
                    await addon.ExecuteHookAsync(context);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Fel vid körning av hook {Hook} i addon {AddonId}", context.Hook, addon.Id);
                }
            }
        }

        /// <summary>
        /// Initializeringskontroll för alla addons
        /// </summary>
        public async Task InitializeAllAddonsAsync()
        {
            foreach (var addon in _loadedAddons.Values.Where(a => a.IsEnabled))
            {
                try
                {
                    await addon.InitializeAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Fel vid initialisering av addon {AddonId}", addon.Id);
                }
            }
        }

        /// <summary>
        /// Rensar upp alla addons vid avslutning
        /// </summary>
        public async Task ShutdownAllAddonsAsync()
        {
            foreach (var addon in _loadedAddons.Values)
            {
                try
                {
                    await addon.ShutdownAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Fel vid avslutning av addon {AddonId}", addon.Id);
                }
            }
        }
    }
}
