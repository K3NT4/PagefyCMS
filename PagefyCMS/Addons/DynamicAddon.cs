namespace PagefyCMS.Addons
{
    /// <summary>
    /// En dynamisk addon-klass för addons som laddas från konfigurationsfiler
    /// </summary>
    public class DynamicAddon : BaseAddon
    {
        private readonly string _id;
        private readonly string _name;
        private readonly string _description;
        private readonly string _version;
        private readonly string _author;

        public override string Id => _id;
        public override string Name => _name;
        public override string Description => _description;
        public override string Version => _version;
        public override string Author => _author;

        public DynamicAddon(string id, string name, string description, string version, string author = "PagefyCMS")
        {
            _id = id ?? "dynamic";
            _name = name ?? "Dynamic Addon";
            _description = description ?? "En dynamisk addon";
            _version = version ?? "1.0.0";
            _author = author;
        }

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();
        }

        public override async Task ShutdownAsync()
        {
            await base.ShutdownAsync();
        }
    }
}

