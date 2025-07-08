using Microsoft.AspNetCore.Mvc;
using PagefyCMS.Data;
using System.Linq;

namespace PagefyCMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StartpageController : ControllerBase
    {
        private readonly PagefyDbContext _context;

        public StartpageController(PagefyDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetSettings()
        {
            var settings = _context.StartpageSettings.FirstOrDefault();
            if (settings == null)
                return NotFound();

            return Ok(settings);
        }
    }
}
