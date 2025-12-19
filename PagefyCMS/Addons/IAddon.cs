namespace PagefyCMS.Addons
{
    /// <summary>
    /// Basinterface för alla addons i PagefyCMS
    /// </summary>
    public interface IAddon
    {
        /// <summary>
        /// Addons unika ID (t.ex. "com.example.myaddon")
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Visningsnamn för addonen
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Beskrivning av vad addonen gör
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Versionsnummer för addonen
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Författare av addonen
        /// </summary>
        string Author { get; }

        /// <summary>
        /// Initialiserar addonen när den laddas
        /// </summary>
        Task InitializeAsync();

        /// <summary>
        /// Avslutar addonen på ett rent sätt
        /// </summary>
        Task ShutdownAsync();

        /// <summary>
        /// Returnerar true om addonen är aktiverad
        /// </summary>
        bool IsEnabled { get; }
    }
}
