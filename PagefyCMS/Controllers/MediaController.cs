using Microsoft.AspNetCore.Mvc;
using PagefyCMS.Data;
using PagefyCMS.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Threading.Tasks;

namespace PagefyCMS.Controllers
{
    /// <summary>
    /// API controller for media management - handles file uploads and deletions.
    /// </summary>
    [Route("admin/media")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly PagefyDbContext _context;
        private const long MaxFileSize = 50 * 1024 * 1024; // 50 MB
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

        public MediaController(IWebHostEnvironment env, PagefyDbContext context)
        {
            _env = env ?? throw new ArgumentNullException(nameof(env));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Upload a new media file and generate WebP variants.
        /// </summary>
        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            // Validation
            if (file == null || file.Length == 0)
                return BadRequest(new { error = "Ingen fil vald" });

            if (file.Length > MaxFileSize)
                return BadRequest(new { error = $"Filen är för stor (max {MaxFileSize / 1024 / 1024}MB)" });

            var ext = Path.GetExtension(file.FileName).ToLower();
            if (!AllowedExtensions.Contains(ext))
                return BadRequest(new { error = "Filtypen stöds inte. Tillåtna: jpg, png, gif, webp" });

            try
            {
                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var id = Guid.NewGuid();
                var slug = $"{fileName}-{id}{ext}";
                var uploadsPath = Path.Combine(_env.WebRootPath, "uploads");
                
                // Ensure directories exist
                EnsureDirectoriesExist(uploadsPath);

                var originalsPath = Path.Combine(uploadsPath, "originals", slug);

                // Save original file
                using (var stream = new FileStream(originalsPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Generate WebP variants
                await GenerateWebPVariants(originalsPath, uploadsPath, fileName, id);

                // Save to database
                var media = new MediaItem
                {
                    Id = id,
                    Filename = file.FileName,
                    Slug = slug,
                    OriginalPath = $"/uploads/originals/{slug}",
                    WebpSmall = $"/uploads/webp/small/{fileName}-{id}.webp",
                    WebpMedium = $"/uploads/webp/medium/{fileName}-{id}.webp",
                    WebpLarge = $"/uploads/webp/large/{fileName}-{id}.webp",
                    UploadedAt = DateTime.UtcNow
                };

                _context.MediaLibrary.Add(media);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, data = media });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Upload error: {ex.Message}");
                return StatusCode(500, new { error = "Fel vid filuppladdning. Försök igen senare." });
            }
        }

        /// <summary>
        /// Delete a media file and its variants.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var media = await _context.MediaLibrary.FindAsync(id);
                if (media == null) 
                    return NotFound(new { error = "Fil ej hittad" });

                // Delete files with error handling
                var filesToDelete = new[] { 
                    media.WebpSmall, 
                    media.WebpMedium, 
                    media.WebpLarge, 
                    media.OriginalPath 
                };

                foreach (var filePath in filesToDelete)
                {
                    DeleteFileIfExists(filePath);
                }

                _context.MediaLibrary.Remove(media);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Filen har tagits bort" });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Delete error: {ex.Message}");
                return StatusCode(500, new { error = "Fel vid borttagning av fil." });
            }
        }

        /// <summary>
        /// Ensure all required directories exist for file operations.
        /// </summary>
        private void EnsureDirectoriesExist(string uploadsPath)
        {
            Directory.CreateDirectory(Path.Combine(uploadsPath, "originals"));
            Directory.CreateDirectory(Path.Combine(uploadsPath, "webp", "small"));
            Directory.CreateDirectory(Path.Combine(uploadsPath, "webp", "medium"));
            Directory.CreateDirectory(Path.Combine(uploadsPath, "webp", "large"));
        }

        /// <summary>
        /// Generate WebP variants for responsive image delivery.
        /// </summary>
        private async Task GenerateWebPVariants(string originalsPath, string uploadsPath, string fileName, Guid id)
        {
            var smallPath = Path.Combine(uploadsPath, "webp", "small", $"{fileName}-{id}.webp");
            var mediumPath = Path.Combine(uploadsPath, "webp", "medium", $"{fileName}-{id}.webp");
            var largePath = Path.Combine(uploadsPath, "webp", "large", $"{fileName}-{id}.webp");

            using (var image = Image.Load(originalsPath))
            {
                // Small variant
                image.Mutate(x => x.Resize(400, 0));
                await image.SaveAsWebpAsync(smallPath);

                // Medium variant
                image.Mutate(x => x.Resize(800, 0));
                await image.SaveAsWebpAsync(mediumPath);

                // Large variant
                image.Mutate(x => x.Resize(1600, 0));
                await image.SaveAsWebpAsync(largePath);
            }
        }

        /// <summary>
        /// Safely delete a file if it exists.
        /// </summary>
        private void DeleteFileIfExists(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return;

            try
            {
                var fullPath = Path.Combine(_env.WebRootPath, filePath.TrimStart('/'));
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }
            catch (Exception ex)
            {
                // Log but don't throw - deletion of variant is not critical
                System.Diagnostics.Debug.WriteLine($"Failed to delete file {filePath}: {ex.Message}");
            }
        }
    }
}

