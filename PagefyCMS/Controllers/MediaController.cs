using Microsoft.AspNetCore.Mvc;
using PagefyCMS.Data;
using PagefyCMS.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

[Route("admin/media")]
public class MediaController : Controller
{
    private readonly IWebHostEnvironment _env;
    private readonly PagefyDbContext _context;

    public MediaController(IWebHostEnvironment env, PagefyDbContext context)
    {
        _env = env;
        _context = context;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Ingen fil vald");

        try
        {
            var fileName = Path.GetFileNameWithoutExtension(file.FileName);
            var ext = Path.GetExtension(file.FileName).ToLower();

            var id = Guid.NewGuid();
            var slug = $"{fileName}-{id}{ext}";
            var uploadsPath = Path.Combine(_env.WebRootPath, "uploads");
            
            // Ensure directories exist
            Directory.CreateDirectory(Path.Combine(uploadsPath, "originals"));
            Directory.CreateDirectory(Path.Combine(uploadsPath, "webp", "small"));
            Directory.CreateDirectory(Path.Combine(uploadsPath, "webp", "medium"));
            Directory.CreateDirectory(Path.Combine(uploadsPath, "webp", "large"));

            var originalsPath = Path.Combine(uploadsPath, "originals", slug);

            // Spara original
            using (var stream = new FileStream(originalsPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Konvertera till WebP
            var smallPath = Path.Combine(uploadsPath, "webp", "small", $"{fileName}-{id}.webp");
            var mediumPath = Path.Combine(uploadsPath, "webp", "medium", $"{fileName}-{id}.webp");
            var largePath = Path.Combine(uploadsPath, "webp", "large", $"{fileName}-{id}.webp");

            using (var image = SixLabors.ImageSharp.Image.Load(originalsPath))
            {
                image.Mutate(x => x.Resize(400, 0));
                await image.SaveAsWebpAsync(smallPath);

                image.Mutate(x => x.Resize(800, 0));
                await image.SaveAsWebpAsync(mediumPath);

                image.Mutate(x => x.Resize(1600, 0));
                await image.SaveAsWebpAsync(largePath);
            }

            // Spara i databasen
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

            return Ok(media);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Fel vid filuppladdning: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var media = await _context.MediaLibrary.FindAsync(id);
            if (media == null) 
                return NotFound();

            // Delete files with error handling
            var filesToDelete = new[] { 
                media.WebpSmall, 
                media.WebpMedium, 
                media.WebpLarge, 
                media.OriginalPath 
            };

            foreach (var filePath in filesToDelete)
            {
                try
                {
                    var fullPath = Path.Combine(_env.WebRootPath, filePath.TrimStart('/'));
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }
                catch (Exception)
                {
                    // Log error but continue with deletion
                }
            }

            _context.MediaLibrary.Remove(media);
            await _context.SaveChangesAsync();

            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Fel vid borttagning: {ex.Message}");
        }
    }
}

