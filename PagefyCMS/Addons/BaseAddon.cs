namespace PagefyCMS.Addons
{
    /// <summary>
    /// Basklass för addons - erv denna för enkelt att skapa nya addons
    /// </summary>
    public abstract class BaseAddon : IAddon
    {
        public abstract string Id { get; }
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract string Version { get; }
        public abstract string Author { get; }

        protected bool _isEnabled = false;
        public bool IsEnabled => _isEnabled;

        /// <summary>
        /// Initialisering av addonen - överskrid denna metod
        /// </summary>
        public virtual async Task InitializeAsync()
        {
            _isEnabled = true;
            await Task.CompletedTask;
        }

        /// <summary>
        /// Avslutning av addonen - överskrid denna metod
        /// </summary>
        public virtual async Task ShutdownAsync()
        {
            _isEnabled = false;
            await Task.CompletedTask;
        }
    }

    /// <summary>
    /// Basklass för hookable addons
    /// </summary>
    public abstract class HookableAddon : BaseAddon, IHookableAddon
    {
        public abstract IEnumerable<AddonHook> SupportedHooks { get; }

        /// <summary>
        /// Hook-exekveringsmetod - överskrid denna
        /// </summary>
        public virtual async Task ExecuteHookAsync(HookContext context)
        {
            await Task.CompletedTask;
        }
    }
}
