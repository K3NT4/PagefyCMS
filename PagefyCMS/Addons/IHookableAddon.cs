namespace PagefyCMS.Addons
{
    /// <summary>
    /// Hook-system för addons att kunna koppla in sitt eget funktionalitet
    /// </summary>
    public enum AddonHook
    {
        /// <summary>
        /// Körs före rendering av startsida
        /// </summary>
        BeforeHomepageRender,

        /// <summary>
        /// Körs efter rendering av startsida
        /// </summary>
        AfterHomepageRender,

        /// <summary>
        /// Körs före rendering av sida
        /// </summary>
        BeforePageRender,

        /// <summary>
        /// Körs efter rendering av sida
        /// </summary>
        AfterPageRender,

        /// <summary>
        /// Körs före rendering av artikel
        /// </summary>
        BeforeArticleRender,

        /// <summary>
        /// Körs efter rendering av artikel
        /// </summary>
        AfterArticleRender,

        /// <summary>
        /// Körs innan artikel sparas
        /// </summary>
        BeforeArticleSave,

        /// <summary>
        /// Körs innan sida sparas
        /// </summary>
        BeforePageSave,

        /// <summary>
        /// Körs innan media sparas
        /// </summary>
        BeforeMediaSave,

        /// <summary>
        /// Körs innan media tas bort
        /// </summary>
        BeforeMediaDelete,

        /// <summary>
        /// Tillåter addons att lägga till eget innehål i admin-menyn
        /// </summary>
        AdminMenuItems,

        /// <summary>
        /// Körs vid system-initialisering
        /// </summary>
        SystemInitialize
    }

    /// <summary>
    /// Hook-kontext som passeras till addons
    /// </summary>
    public class HookContext
    {
        public AddonHook Hook { get; set; }
        public Dictionary<string, object> Data { get; set; } = new();
        public string? UserId { get; set; }
        public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;
    }

    /// <summary>
    /// Interface för addon som har hook-funktionalitet
    /// </summary>
    public interface IHookableAddon : IAddon
    {
        /// <summary>
        /// Returnerar lista av hooks som addonen support
        /// </summary>
        IEnumerable<AddonHook> SupportedHooks { get; }

        /// <summary>
        /// Körs när en hook utlöses
        /// </summary>
        Task ExecuteHookAsync(HookContext context);
    }
}
