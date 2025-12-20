using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagefyCMS.Services;
using System.Text.Json;

namespace PagefyCMS.Pages.Admin.Settings
{
    public class LanguagesModel : PageModel
    {
        private readonly ILanguageService _languageService;
        private readonly IWebHostEnvironment _environment;

        public List<string> AvailableLanguages { get; set; } = new();
        public string? Message { get; set; }
        public string? Error { get; set; }

        public LanguagesModel(ILanguageService languageService, IWebHostEnvironment environment)
        {
            _languageService = languageService;
            _environment = environment;
        }

        public void OnGet()
        {
            AvailableLanguages = _languageService.GetAvailableLanguages();
        }

        public IActionResult OnPostUpload(string languageName, IFormFile languageFile)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(languageName))
                {
                    Error = "Language code is required.";
                    AvailableLanguages = _languageService.GetAvailableLanguages();
                    return Page();
                }

                if (languageFile == null || languageFile.Length == 0)
                {
                    Error = "No file uploaded.";
                    AvailableLanguages = _languageService.GetAvailableLanguages();
                    return Page();
                }

                if (!languageFile.FileName.EndsWith(".json"))
                {
                    Error = "Only JSON files are allowed.";
                    AvailableLanguages = _languageService.GetAvailableLanguages();
                    return Page();
                }

                // Read file content
                using (var reader = new StreamReader(languageFile.OpenReadStream()))
                {
                    string jsonContent = reader.ReadToEnd();

                    // Upload language
                    if (_languageService.UploadLanguage(languageName, jsonContent))
                    {
                        Message = $"Language '{languageName}' uploaded successfully!";
                    }
                    else
                    {
                        Error = "Failed to upload language file. Invalid JSON format.";
                    }
                }

                AvailableLanguages = _languageService.GetAvailableLanguages();
                return Page();
            }
            catch (Exception ex)
            {
                Error = $"Error uploading file: {ex.Message}";
                AvailableLanguages = _languageService.GetAvailableLanguages();
                return Page();
            }
        }

        public IActionResult OnPostDownload()
        {
            try
            {
                string template = _languageService.GetLanguageTemplate();
                byte[] fileBytes = System.Text.Encoding.UTF8.GetBytes(template);
                return File(fileBytes, "application/json", "en.json");
            }
            catch (Exception ex)
            {
                Error = $"Error downloading template: {ex.Message}";
                AvailableLanguages = _languageService.GetAvailableLanguages();
                return Page();
            }
        }

        public IActionResult OnPostDelete(string languageName)
        {
            try
            {
                if (languageName == "en")
                {
                    Error = "Cannot delete the default English language file.";
                }
                else
                {
                    string filePath = Path.Combine(_environment.WebRootPath, "languages", $"{languageName}.json");
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                        Message = $"Language '{languageName}' deleted successfully!";
                    }
                    else
                    {
                        Error = "Language file not found.";
                    }
                }

                AvailableLanguages = _languageService.GetAvailableLanguages();
                return Page();
            }
            catch (Exception ex)
            {
                Error = $"Error deleting language: {ex.Message}";
                AvailableLanguages = _languageService.GetAvailableLanguages();
                return Page();
            }
        }
    }
}
