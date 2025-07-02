using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagefyCMS.Data;
using PagefyCMS.Models;
using System.Text.RegularExpressions;

namespace PagefyCMS.Pages.Admin.Articles
{
    public class CreateModel : PageModel
    {
        private readonly PagefyDbContext _context;

        [BindProperty]
        public ArticlePage NewArticle { get; set; } = new ArticlePage();

        public CreateModel(PagefyDbContext context)
        {
            _context = context;
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            if (string.IsNullOrWhiteSpace(NewArticle.Slug) && !string.IsNullOrWhiteSpace(NewArticle.Headline))
            {
                NewArticle.Slug = GenerateSlug(NewArticle.Headline);
            }

            NewArticle.PublishedAt = DateTime.Now;
            _context.Articles.Add(NewArticle);
            _context.SaveChanges();

            return RedirectToPage("./Index");
        }

        private string GenerateSlug(string input)
        {
            string slug = input.ToLowerInvariant();
            slug = Regex.Replace(slug, @"\s+", "-");
            slug = Regex.Replace(slug, @"[^a-z0-9\-]", "");
            slug = Regex.Replace(slug, @"\-+", "-");
            slug = slug.Trim('-');
            return slug;
        }
    }
}
