using Microsoft.AspNetCore.Mvc;
using PagefyCMS.Data;
using System.Text;
using System.Xml;

namespace PagefyCMS.Controllers
{
    [ApiController]
    public class SeoController : ControllerBase
    {
        private readonly PagefyDbContext _context;

        public SeoController(PagefyDbContext context)
        {
            _context = context;
        }

        [Route("sitemap.xml")]
        [HttpGet]
        public IActionResult Sitemap()
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            // Include all pages for indexing, selecting only needed fields for performance
            var pages = _context.Pages.Select(p => new { p.Slug, p.UpdatedAt }).ToList();
            var articles = _context.Articles.Select(a => new { a.Slug, a.UpdatedAt }).ToList();

            var sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sb.AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");

            // Home page
            sb.AppendLine("  <url>");
            sb.AppendLine($"    <loc>{baseUrl}/</loc>");
            sb.AppendLine("    <changefreq>daily</changefreq>");
            sb.AppendLine("    <priority>1.0</priority>");
            sb.AppendLine("  </url>");

            foreach (var page in pages)
            {
                if (string.IsNullOrEmpty(page.Slug)) continue;

                sb.AppendLine("  <url>");
                sb.AppendLine($"    <loc>{baseUrl}/{page.Slug}</loc>");
                sb.AppendLine($"    <lastmod>{page.UpdatedAt:yyyy-MM-dd}</lastmod>");
                sb.AppendLine("    <changefreq>weekly</changefreq>");
                sb.AppendLine("    <priority>0.8</priority>");
                sb.AppendLine("  </url>");
            }

            foreach (var article in articles)
            {
                if (string.IsNullOrEmpty(article.Slug)) continue;

                sb.AppendLine("  <url>");
                sb.AppendLine($"    <loc>{baseUrl}/Article/{article.Slug}</loc>");
                sb.AppendLine($"    <lastmod>{article.UpdatedAt:yyyy-MM-dd}</lastmod>");
                sb.AppendLine("    <changefreq>monthly</changefreq>");
                sb.AppendLine("    <priority>0.6</priority>");
                sb.AppendLine("  </url>");
            }

            sb.AppendLine("</urlset>");

            return Content(sb.ToString(), "application/xml", Encoding.UTF8);
        }

        [Route("robots.txt")]
        [HttpGet]
        public IActionResult Robots()
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var sb = new StringBuilder();
            sb.AppendLine("User-agent: *");
            sb.AppendLine("Allow: /");
            sb.AppendLine($"Sitemap: {baseUrl}/sitemap.xml");

            return Content(sb.ToString(), "text/plain", Encoding.UTF8);
        }
    }
}
