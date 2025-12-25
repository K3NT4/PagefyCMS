using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using PagefyCMS.Data;
using PagefyCMS.Models;

namespace PagefyCMS.Services
{
    public class AssetUsageService
    {
        private readonly PagefyDbContext _context;

        public AssetUsageService(PagefyDbContext context)
        {
            _context = context;
        }

        public async Task IndexContentAsync(int contentId, string contentType, string content, string title)
        {
            // 1. Remove existing usages for this content
            var existing = await _context.AssetUsages
                .Where(u => u.ContentId == contentId && u.ContentType == contentType)
                .ToListAsync();

            if (existing.Any())
            {
                _context.AssetUsages.RemoveRange(existing);
            }

            if (string.IsNullOrEmpty(content))
            {
                await _context.SaveChangesAsync();
                return;
            }

            // 2. Scan content for media references
            // We look for slugs used in /uploads/originals/ or /uploads/webp/...
            // The slug format is typically filename-guid.ext
            // We can search for the guid part or the full slug.

            // Getting all media slugs to match against is safer than blind regex if practical,
            // but for performance we might extract potential slugs and verify.

            // Let's use a regex that finds image sources.
            // src="/uploads/originals/slug"
            // src="/uploads/webp/small/slug"

            // Regex to capture the slug from the URL path
            var regex = new Regex(@"/uploads/(?:originals|webp/(?:small|medium|large))/([^\""\']+)");
            var matches = regex.Matches(content);

            var potentialSlugs = matches.Select(m => m.Groups[1].Value).Distinct().ToList();

            if (!potentialSlugs.Any())
            {
                await _context.SaveChangesAsync();
                return;
            }

            // 3. Verify which slugs exist in MediaLibrary and get their IDs
            // We need to match the filename part or the full slug.
            // In our system, the URL usually contains the exact Slug stored in DB.
            // e.g. /uploads/originals/myimage-guid.jpg -> Slug = myimage-guid.jpg

            // However, the WebP version might have replaced extension.
            // e.g. /uploads/webp/small/myimage-guid.webp
            // But the DB Slug is myimage-guid.jpg.

            // So we should match based on the "base name" (filename-guid).

            var foundMediaIds = new List<Guid>();

            // Optimize: Load all Media items? No.
            // Filter DB? "Where slug in potentialSlugs" only works if exact match.

            // Strategy: For each potential slug, try to find a media item.
            // Since potentialSlugs is usually small (images per page), this is okay.

            foreach (var slugFromUrl in potentialSlugs)
            {
                // Try exact match first
                var media = await _context.MediaLibrary.FirstOrDefaultAsync(m => m.Slug == slugFromUrl);

                if (media == null)
                {
                    // Try removing extension and matching base name?
                    // Or check if it's a .webp version of a .jpg slug
                    var baseName = Path.GetFileNameWithoutExtension(slugFromUrl);
                    // Search for media where slug starts with baseName?
                    // This is risky if one is a prefix of another.
                    // But GUIDs make it unique.

                    // Actually, if we search for "Contains(baseName)" it might work.
                    // Let's rely on the fact that GUIDs are unique.
                    media = await _context.MediaLibrary.FirstOrDefaultAsync(m => m.Slug.Contains(baseName));
                }

                if (media != null)
                {
                    if (!foundMediaIds.Contains(media.Id))
                    {
                        foundMediaIds.Add(media.Id);
                    }
                }
            }

            // 4. Insert new usages
            foreach (var assetId in foundMediaIds)
            {
                _context.AssetUsages.Add(new AssetUsage
                {
                    AssetId = assetId,
                    ContentId = contentId,
                    ContentType = contentType,
                    ContentTitle = title,
                    ReferenceKind = "BodyHtml",
                    LastSeenAt = DateTime.UtcNow
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task RebuildIndexAsync()
        {
            // Clear all
            _context.Database.ExecuteSqlRaw("DELETE FROM AssetUsages");

            // Index Pages
            var pages = await _context.Pages.AsNoTracking().ToListAsync();
            foreach (var p in pages)
            {
                await IndexContentAsync(p.Id, "Page", p.Content, p.Title);
            }

            // Index Articles
            var articles = await _context.Articles.AsNoTracking().ToListAsync();
            foreach (var a in articles)
            {
                await IndexContentAsync(a.Id, "Article", a.Content, a.Headline);
            }
        }
    }
}
