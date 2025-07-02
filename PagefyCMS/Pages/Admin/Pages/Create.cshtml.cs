using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagefyCMS.Data;
using PagefyCMS.Models;
using System.Text.RegularExpressions;

namespace PagefyCMS.Pages.Admin.Pages
{
    public class CreateModel : PageModel
    {
        private readonly PagefyDbContext _context;

        [BindProperty]
        public ContentPage NewPage { get; set; } = new ContentPage();

        public CreateModel(PagefyDbContext context)
        {
            _context = context;
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Console.WriteLine($"- {state.Key}: {error.ErrorMessage}");
                    }
                }
                return Page();
            }

            if (string.IsNullOrWhiteSpace(NewPage.Slug) && !string.IsNullOrWhiteSpace(NewPage.Title))
            {
                NewPage.Slug = GenerateSlug(NewPage.Title);
            }

            NewPage.CreatedAt = DateTime.Now;

            if (string.IsNullOrWhiteSpace(NewPage.GalleryGroup))
            {
                NewPage.GalleryGroup = ""; // Eller "default" eller vad du vill
            }

            _context.Pages.Add(NewPage);
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
