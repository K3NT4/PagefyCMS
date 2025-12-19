using PagefyCMS.Addons;

namespace PagefyCMS.ExampleAddons
{
    /// <summary>
    /// Exempel-addon: Aktivitets-loggning
    /// Spårar alla ändringar i CMS
    /// </summary>
    public class ActivityLogAddon : HookableAddon
    {
        public override string Id => "com.pagefy.activitylog";
        public override string Name => "Activity Logger";
        public override string Description => "Loggar alla ändringar och aktiviteter i CMS";
        public override string Version => "1.0.0";
        public override string Author => "PagefyCMS Team";

        public override IEnumerable<AddonHook> SupportedHooks => new[]
        {
            AddonHook.BeforePageSave,
            AddonHook.BeforeArticleSave,
            AddonHook.BeforeMediaSave,
            AddonHook.BeforeMediaDelete,
            AddonHook.SystemInitialize
        };

        private List<string> _activityLog = new();

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();
            _activityLog.Clear();
            System.Diagnostics.Debug.WriteLine($"✓ {Name} aktiverad - Börjar logga aktiviteter");
        }

        public override async Task ExecuteHookAsync(HookContext context)
        {
            var logEntry = FormatLogEntry(context);
            _activityLog.Add(logEntry);
            
            // Här kan man spara till databas, fil, etc.
            System.Diagnostics.Debug.WriteLine($"📝 LOG: {logEntry}");
            
            await Task.CompletedTask;
        }

        private string FormatLogEntry(HookContext context)
        {
            var timestamp = context.ExecutedAt.ToString("yyyy-MM-dd HH:mm:ss");
            var userId = context.UserId ?? "Unknown";
            return $"[{timestamp}] {context.Hook} by {userId}";
        }

        public List<string> GetActivityLog() => new(_activityLog);

        public override async Task ShutdownAsync()
        {
            System.Diagnostics.Debug.WriteLine($"🛑 {Name} stängs ner. Sparade {_activityLog.Count} loggar");
            await base.ShutdownAsync();
        }
    }
}
