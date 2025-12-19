using PagefyCMS.Addons;

namespace PagefyCMS.ExampleAddons
{
    /// <summary>
    /// Exempel-addon: SEO-optimering
    /// Denna addon lägger till meta-tags och optimering för sökmotorer
    /// </summary>
    public class SeoAddon : HookableAddon
    {
        public override string Id => "com.pagefy.seo";
        public override string Name => "SEO Optimizer";
        public override string Description => "Lägger till automatisk SEO-optimering för sidor och artiklar";
        public override string Version => "1.0.0";
        public override string Author => "PagefyCMS Team";

        public override IEnumerable<AddonHook> SupportedHooks => new[]
        {
            AddonHook.BeforePageRender,
            AddonHook.BeforeArticleRender,
            AddonHook.BeforeArticleSave,
            AddonHook.BeforePageSave
        };

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();
            // Lägg till din initialiserings-logik här
            System.Diagnostics.Debug.WriteLine($"✓ {Name} aktiverad");
        }

        public override async Task ExecuteHookAsync(HookContext context)
        {
            switch (context.Hook)
            {
                case AddonHook.BeforePageSave:
                case AddonHook.BeforeArticleSave:
                    await OptimizeForSaveAsync(context);
                    break;

                case AddonHook.BeforePageRender:
                case AddonHook.BeforeArticleRender:
                    await OptimizeForRenderAsync(context);
                    break;
            }
        }

        private Task OptimizeForSaveAsync(HookContext context)
        {
            // Här kan man validera SEO-data före sparning
            System.Diagnostics.Debug.WriteLine($"SEO: Optimera före sparning för {context.Hook}");
            return Task.CompletedTask;
        }

        private Task OptimizeForRenderAsync(HookContext context)
        {
            // Här kan man lägga till meta-tags och andra SEO-saker
            System.Diagnostics.Debug.WriteLine($"SEO: Optimera rendering för {context.Hook}");
            return Task.CompletedTask;
        }
    }
}
