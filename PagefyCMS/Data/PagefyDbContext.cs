using Microsoft.EntityFrameworkCore;
using PagefyCMS.Models;

namespace PagefyCMS.Data
{
    public class PagefyDbContext : DbContext
    {
        public PagefyDbContext(DbContextOptions<PagefyDbContext> options) : base(options) { }

        public DbSet<ContentPage> Pages { get; set; }
        public DbSet<ArticlePage> Articles { get; set; }
        public DbSet<CmsSetting> Settings { get; set; }
        public DbSet<MediaItem> MediaLibrary { get; set; }
        public DbSet<AssetUsage> AssetUsages { get; set; }

        public DbSet<StartpageSettings> StartpageSettings { get; set; }


    }
}
