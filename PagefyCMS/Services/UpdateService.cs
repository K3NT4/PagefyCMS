namespace PagefyCMS.Services
{
    public class UpdateInfo
    {
        public string LatestVersion { get; set; } = "";
        public string ReleaseNotes { get; set; } = "";
        public bool IsUpdateAvailable { get; set; }
        public DateTime ReleaseDate { get; set; }
    }

    public interface IUpdateService
    {
        Task<UpdateInfo> CheckForUpdatesAsync();
        Task<bool> TriggerDeploymentAsync();
    }

    public class MockUpdateService : IUpdateService
    {
        private readonly IVersionService _versionService;
        private readonly ILogger<MockUpdateService> _logger;

        public MockUpdateService(IVersionService versionService, ILogger<MockUpdateService> logger)
        {
            _versionService = versionService;
            _logger = logger;
        }

        public async Task<UpdateInfo> CheckForUpdatesAsync()
        {
            // In a real app, this would call GitHub API or a custom endpoint.
            // For now, we simulate a newer version availability.
            await Task.Delay(500); // Simulate network latency

            var current = _versionService.GetCurrentVersion();

            // Mock logic: If current is "1.0.0-dev", pretend "1.1.0" is out.
            return new UpdateInfo
            {
                LatestVersion = "1.1.0",
                ReleaseDate = DateTime.UtcNow.AddDays(-2),
                IsUpdateAvailable = true,
                ReleaseNotes = "<ul><li>New Asset Usage Map</li><li>Performance improvements</li><li>Security fixes</li></ul>"
            };
        }

        public async Task<bool> TriggerDeploymentAsync()
        {
            // In a real app, this sends a POST to a webhook (e.g., GitHub Actions dispatch).
            // For now, we just log it.
            await Task.Delay(1000);
            _logger.LogInformation("Deployment webhook triggered by Admin.");

            return true;
        }
    }
}
