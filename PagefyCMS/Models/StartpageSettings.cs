using Microsoft.EntityFrameworkCore;
namespace PagefyCMS.Models
{
    public class StartpageSettings
    {
        public int Id { get; set; }
        public string Layout { get; set; }
        public string SectionsJson { get; set; } // Vi kan lagra listan som JSON-sträng
        public List<string> Sections { get; set; } = new List<string>();
        public StartpageColors Colors { get; set; } = new StartpageColors();
        public string BackgroundColor { get; set; }
        public string PrimaryColor { get; set; }
        public string TextColor { get; set; }
        public string HeroBackground { get; set; }
    }
    [Owned]
    public class StartpageColors
    {
        public string Background { get; set; }
        public string Primary { get; set; }
        public string Text { get; set; }
    }
}