using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagefyCMS.Data;
using PagefyCMS.Models;
using System.Collections.Generic;
using System.Linq;

namespace PagefyCMS.Pages.Admin.Settings
{
    public class StartpageDesignModel : PageModel
    {
        private readonly PagefyDbContext _context;

        public StartpageDesignModel(PagefyDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public StartpageSettings Settings { get; set; }

        public List<string> Sections { get; set; } = new List<string>
        {
            "Hero", "FeaturedArticles", "LatestReviews", "NewsList", "TrailerSection"
        };

        public void OnGet()
        {
            Settings = _context.StartpageSettings.FirstOrDefault() ?? new StartpageSettings();
        }

        public IActionResult OnPost()
        {
            var existing = _context.StartpageSettings.FirstOrDefault();
            if (existing == null)
            {
                existing = new StartpageSettings();
                _context.StartpageSettings.Add(existing);
            }

            existing.Layout = Request.Form["Layout"];
            existing.Sections = Request.Form["Sections"].ToList();
            existing.Colors = new StartpageColors
            {
                Background = Request.Form["BackgroundColor"],
                Primary = Request.Form["PrimaryColor"],
                Text = Request.Form["TextColor"]
            };
            existing.HeroBackground = Request.Form["HeroBackground"];

            _context.SaveChanges();
            TempData["Message"] = "Startsidans design är sparad!";
            return RedirectToPage();
        }
    }
}
