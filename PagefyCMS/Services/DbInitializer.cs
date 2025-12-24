using Microsoft.EntityFrameworkCore;
using PagefyCMS.Data;

namespace PagefyCMS.Services
{
    public static class DbInitializer
    {
        public static void EnsureAssetUsageTable(PagefyDbContext context)
        {
            // Simple SQL to create the table if it doesn't exist.
            // SQLite specific syntax.
            var sql = @"
                CREATE TABLE IF NOT EXISTS AssetUsages (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    AssetId TEXT NOT NULL,
                    ContentId INTEGER NOT NULL,
                    ContentType TEXT NOT NULL,
                    ContentTitle TEXT,
                    ReferenceKind TEXT,
                    LastSeenAt TEXT NOT NULL
                );
                CREATE INDEX IF NOT EXISTS IX_AssetUsages_AssetId ON AssetUsages(AssetId);
                CREATE INDEX IF NOT EXISTS IX_AssetUsages_ContentId ON AssetUsages(ContentId);
            ";

            context.Database.ExecuteSqlRaw(sql);
        }
    }
}
