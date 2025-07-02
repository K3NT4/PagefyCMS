using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagefyCMS.Data;
using PagefyCMS.Models;
using System.Text.RegularExpressions;

namespace PagefyCMS.Pages.Admin.Articles
{
    public class EditModel : PageModel
    {
        private readonly PagefyDbContext _context;

        [BindProperty]
        public ArticlePage EditArticle { get; set; } = new ArticlePage();

        public EditModel(PagefyDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(int id)
        {
            var article = _context.Articles.FirstOrDefault(a => a.Id == id);
            if (article == null)
                return RedirectToPage("./Index");

            EditArticle = article;
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            if (string.IsNullOrWhiteSpace(EditArticle.Slug) && !string.IsNullOrWhiteSpace(EditArticle.Headline))
            {
                EditArticle.Slug = GenerateSlug(EditArticle.Headline);
            }

            _context.Articles.Update(EditArticle);
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
