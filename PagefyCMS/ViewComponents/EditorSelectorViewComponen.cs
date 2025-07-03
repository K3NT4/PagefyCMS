using Microsoft.AspNetCore.Mvc;
using PagefyCMS.Data;

namespace PagefyCMS.ViewComponents
{
    public class EditorSelectorViewComponent : ViewComponent
    {
        private readonly PagefyDbContext _context;

        public EditorSelectorViewComponent(PagefyDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            // Exempel: Läs från en inställningstabell i databasen
            var editorType = _context.Settings.FirstOrDefault(s => s.Key == "EditorType")?.Value ?? "tinymce";

            if (editorType == "toastui")
                return View("ToastUi");

            return View("TinyMce"); // Standardval
        }
    }
}
