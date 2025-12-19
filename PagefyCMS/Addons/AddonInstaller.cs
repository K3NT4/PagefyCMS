namespace PagefyCMS.Addons
{
    /// <summary>
    /// Installerar och hanterar addon-paket (ZIP-filer)
    /// </summary>
    public class AddonInstaller
    {
        private readonly ILogger<AddonInstaller> _logger;
        private readonly string _addonsPath;
        private readonly string _uploadsPath;
        private const long MaxUploadSize = 10 * 1024 * 1024; // 10 MB
        private static readonly string[] AllowedExtensions = { ".zip" };

        public AddonInstaller(ILogger<AddonInstaller> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _addonsPath = Path.Combine(env.ContentRootPath, "Addons");
            _uploadsPath = Path.Combine(env.ContentRootPath, "Uploads", "Addons");
        }

        /// <summary>
        /// Laddar upp och installerar en addon från ZIP-fil
        /// </summary>
        public async Task<AddonInstallResult> InstallAddonAsync(IFormFile file)
        {
            try
            {
                // Validera fil
                var validation = ValidateFile(file);
                if (!validation.IsSuccess)
                    return validation;

                // Skapa upload-mapp om den inte finns
                Directory.CreateDirectory(_uploadsPath);

                // Spara temporär fil
                var tempFileName = $"{Guid.NewGuid()}.zip";
                var tempPath = Path.Combine(_uploadsPath, tempFileName);

                using (var stream = new FileStream(tempPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                _logger.LogInformation("📦 Uppladdat addon-paket: {FileName}", file.FileName);

                // Packa upp och validera
                var extractResult = await ExtractAndValidateAsync(tempPath);
                if (!extractResult.IsSuccess)
                {
                    try { System.IO.File.Delete(tempPath); } catch { }
                    return extractResult;
                }

                // Installera addonen
                var installResult = await FinalizeInstallationAsync(extractResult.TempAddonPath ?? "", extractResult.AddonId ?? "");
                
                // Rensa temporär fil
                try { System.IO.File.Delete(tempPath); } catch { }

                return installResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Fel vid installation av addon");
                return new AddonInstallResult
                {
                    IsSuccess = false,
                    ErrorMessage = $"Installation misslyckades: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// Validerar uppladdad fil
        /// </summary>
        private AddonInstallResult ValidateFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return new AddonInstallResult
                {
                    IsSuccess = false,
                    ErrorMessage = "Ingen fil vald"
                };
            }

            if (file.Length > MaxUploadSize)
            {
                return new AddonInstallResult
                {
                    IsSuccess = false,
                    ErrorMessage = $"Filen är för stor. Max storlek är {MaxUploadSize / 1024 / 1024} MB"
                };
            }

            var ext = Path.GetExtension(file.FileName).ToLower();
            if (!AllowedExtensions.Contains(ext))
            {
                return new AddonInstallResult
                {
                    IsSuccess = false,
                    ErrorMessage = "Endast ZIP-filer är tillåtna"
                };
            }

            return new AddonInstallResult { IsSuccess = true };
        }

        /// <summary>
        /// Packar upp ZIP-fil och validerar addon.json
        /// </summary>
        private async Task<AddonInstallResult> ExtractAndValidateAsync(string zipPath)
        {
            try
            {
                var tempExtractPath = Path.Combine(_uploadsPath, Guid.NewGuid().ToString());
                Directory.CreateDirectory(tempExtractPath);

                // Packa upp ZIP
                System.IO.Compression.ZipFile.ExtractToDirectory(zipPath, tempExtractPath);

                _logger.LogInformation("📂 Packade upp addon från: {ZipPath}", zipPath);

                // Hitta addon-mapp (kan vara på olika nivåer)
                var addonJsonPath = FindAddonJson(tempExtractPath);
                if (string.IsNullOrEmpty(addonJsonPath))
                {
                    Directory.Delete(tempExtractPath, true);
                    return new AddonInstallResult
                    {
                        IsSuccess = false,
                        ErrorMessage = "addon.json hittades inte. Kontrollera att ZIP-filen innehåller en giltig addon"
                    };
                }

                // Validera addon.json
                var validation = await ValidateAddonJsonAsync(addonJsonPath);
                if (!validation.IsSuccess)
                {
                    Directory.Delete(tempExtractPath, true);
                    return validation;
                }

                var addonDir = Path.GetDirectoryName(addonJsonPath);
                return new AddonInstallResult
                {
                    IsSuccess = true,
                    TempAddonPath = addonDir,
                    AddonId = validation.AddonId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Fel vid uppackning av addon");
                return new AddonInstallResult
                {
                    IsSuccess = false,
                    ErrorMessage = $"Kunde inte packa upp filen: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// Hittar addon.json i mappstrukturen
        /// </summary>
        private string? FindAddonJson(string searchPath)
        {
            try
            {
                // Sök i root
                var rootJson = Path.Combine(searchPath, "addon.json");
                if (File.Exists(rootJson))
                    return rootJson;

                // Sök i undermappar (max 2 nivåer)
                foreach (var dir in Directory.GetDirectories(searchPath))
                {
                    var json = Path.Combine(dir, "addon.json");
                    if (File.Exists(json))
                        return json;

                    // Sök en nivå djupare
                    foreach (var subdir in Directory.GetDirectories(dir))
                    {
                        var subjson = Path.Combine(subdir, "addon.json");
                        if (File.Exists(subjson))
                            return subjson;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "⚠️ Fel vid sökning efter addon.json");
            }

            return null;
        }

        /// <summary>
        /// Validerar addon.json-fil
        /// </summary>
        private async Task<AddonInstallResult> ValidateAddonJsonAsync(string jsonPath)
        {
            try
            {
                var json = await File.ReadAllTextAsync(jsonPath);
                var config = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(json);

                if (config == null)
                {
                    return new AddonInstallResult
                    {
                        IsSuccess = false,
                        ErrorMessage = "addon.json är inte giltig JSON"
                    };
                }

                // Validera obligatoriska fält
                var requiredFields = new[] { "id", "name", "version", "author" };
                foreach (var field in requiredFields)
                {
                    if (!config.ContainsKey(field) || string.IsNullOrEmpty(config[field]?.ToString()))
                    {
                        return new AddonInstallResult
                        {
                            IsSuccess = false,
                            ErrorMessage = $"addon.json saknar obligatoriskt fält: {field}"
                        };
                    }
                }

                var addonId = config["id"].ToString() ?? "";

                // Kontrollera om addonen redan är installerad
                var installedPath = Path.Combine(_addonsPath, addonId);
                if (Directory.Exists(installedPath))
                {
                    return new AddonInstallResult
                    {
                        IsSuccess = false,
                        ErrorMessage = $"Addonen '{addonId}' är redan installerad",
                        AddonId = addonId
                    };
                }

                _logger.LogInformation("✅ addon.json validerad för: {AddonId}", addonId);

                return new AddonInstallResult
                {
                    IsSuccess = true,
                    AddonId = addonId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Fel vid validering av addon.json");
                return new AddonInstallResult
                {
                    IsSuccess = false,
                    ErrorMessage = $"Kunde inte validera addon.json: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// Slutför installationen genom att flytta addon till rätt plats
        /// </summary>
        private async Task<AddonInstallResult> FinalizeInstallationAsync(string tempAddonPath, string addonId)
        {
            try
            {
                var targetPath = Path.Combine(_addonsPath, addonId);

                // Skapa Addons-mapp om den inte finns
                Directory.CreateDirectory(_addonsPath);

                // Flytta addon-mapp
                if (Directory.Exists(targetPath))
                {
                    Directory.Delete(targetPath, true);
                }

                Directory.Move(tempAddonPath, targetPath);

                _logger.LogInformation("✅ Addon installerad: {AddonId} -> {TargetPath}", addonId, targetPath);

                return new AddonInstallResult
                {
                    IsSuccess = true,
                    AddonId = addonId,
                    InstallationPath = targetPath,
                    Message = $"Addon '{addonId}' installerades framgångsrikt"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Fel vid slutförande av installation");
                return new AddonInstallResult
                {
                    IsSuccess = false,
                    ErrorMessage = $"Kunde inte slutföra installationen: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// Avinstallerar en addon
        /// </summary>
        public async Task<bool> UninstallAddonAsync(string addonId)
        {
            try
            {
                var addonPath = Path.Combine(_addonsPath, addonId);
                if (!Directory.Exists(addonPath))
                {
                    _logger.LogWarning("⚠️ Addon-mapp hittades inte: {AddonId}", addonId);
                    return false;
                }

                Directory.Delete(addonPath, true);
                _logger.LogInformation("🗑️ Addon avinstallerad: {AddonId}", addonId);

                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Fel vid avinstallation av addon: {AddonId}", addonId);
                return false;
            }
        }

        /// <summary>
        /// Uppdaterar en befintlig addon
        /// </summary>
        public async Task<AddonInstallResult> UpdateAddonAsync(string addonId, IFormFile file)
        {
            try
            {
                var addonPath = Path.Combine(_addonsPath, addonId);
                if (!Directory.Exists(addonPath))
                {
                    return new AddonInstallResult
                    {
                        IsSuccess = false,
                        ErrorMessage = $"Addon '{addonId}' är inte installerad"
                    };
                }

                // Skapa backup
                var backupPath = Path.Combine(_uploadsPath, $"{addonId}_backup_{DateTime.UtcNow:yyyyMMdd_HHmmss}");
                Directory.CreateDirectory(backupPath);
                CopyDirectory(addonPath, backupPath);

                _logger.LogInformation("💾 Backup skapat för addon: {AddonId}", addonId);

                // Installera ny version
                var installResult = await InstallAddonAsync(file);
                if (!installResult.IsSuccess)
                {
                    // Återställ från backup om installation misslyckades
                    if (Directory.Exists(addonPath))
                        Directory.Delete(addonPath, true);
                    Directory.Move(backupPath, addonPath);
                    _logger.LogInformation("♻️ Återställd backup för addon: {AddonId}", addonId);
                }
                else
                {
                    // Ta bort backup om installation lyckas
                    try { Directory.Delete(backupPath, true); } catch { }
                }

                return installResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Fel vid uppdatering av addon: {AddonId}", addonId);
                return new AddonInstallResult
                {
                    IsSuccess = false,
                    ErrorMessage = $"Kunde inte uppdatera addon: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// Kopierar en mapp rekursivt
        /// </summary>
        private void CopyDirectory(string sourceDir, string targetDir)
        {
            var dir = new DirectoryInfo(sourceDir);
            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Källmappen existerar inte: {sourceDir}");

            Directory.CreateDirectory(targetDir);

            foreach (var file in dir.GetFiles())
            {
                var targetFilePath = Path.Combine(targetDir, file.Name);
                file.CopyTo(targetFilePath);
            }

            foreach (var subDir in dir.GetDirectories())
            {
                var newDestinationDir = Path.Combine(targetDir, subDir.Name);
                CopyDirectory(subDir.FullName, newDestinationDir);
            }
        }

        /// <summary>
        /// Hantera addon enabled/disabled status
        /// </summary>
        public async Task<bool> EnableAddonAsync(string addonId)
        {
            try
            {
                var statusFile = Path.Combine(_addonsPath, addonId, ".addon-enabled");
                if (!System.IO.File.Exists(statusFile))
                {
                    System.IO.File.WriteAllText(statusFile, "enabled");
                    _logger.LogInformation("✅ Addon aktiverad: {AddonId}", addonId);
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Fel vid aktivering av addon");
                return false;
            }
        }

        public async Task<bool> DisableAddonAsync(string addonId)
        {
            try
            {
                var statusFile = Path.Combine(_addonsPath, addonId, ".addon-enabled");
                if (System.IO.File.Exists(statusFile))
                {
                    System.IO.File.Delete(statusFile);
                    _logger.LogInformation("🚫 Addon inaktiverad: {AddonId}", addonId);
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Fel vid inaktivering av addon");
                return false;
            }
        }

        public bool IsAddonEnabled(string addonId)
        {
            var statusFile = Path.Combine(_addonsPath, addonId, ".addon-enabled");
            return System.IO.File.Exists(statusFile);
        }
    }

    /// <summary>
    /// Resultat från addon-installation
    /// </summary>
    public class AddonInstallResult
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public string? ErrorMessage { get; set; }
        public string? AddonId { get; set; }
        public string? InstallationPath { get; set; }
        public string? TempAddonPath { get; set; }
    }
}
