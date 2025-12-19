using PagefyCMS.Addons;

namespace PagefyCMS.ExampleAddons
{
    /// <summary>
    /// Exempel-addon som kan installeras via ZIP-fil
    /// Denna visar hur man skapar en installerbara addon
    /// </summary>
    public class ExamplePackagedAddon : HookableAddon
    {
        public override string Id => "com.pagefy.example";
        public override string Name => "Example Addon";
        public override string Description => "Ett exempel-addon";
        public override string Version => "1.0.0";
        public override string Author => "PagefyCMS Team";

        public override IEnumerable<AddonHook> SupportedHooks => new[]
        {
            AddonHook.SystemInitialize
        };

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();
            System.Diagnostics.Debug.WriteLine($"✨ {Name} initialiserad från installerad package");
        }

        public override async Task ExecuteHookAsync(HookContext context)
        {
            if (context.Hook == AddonHook.SystemInitialize)
            {
                System.Diagnostics.Debug.WriteLine($"🚀 Example addon är aktiv och körs!");
            }
            await Task.CompletedTask;
        }
    }
}
