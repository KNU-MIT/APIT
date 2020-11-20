using BusinessLayer.DataServices;
using BusinessLayer.DataServices.ConfigModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Apit.Controllers
{
    public class ResourcesController : Controller
    {
        private readonly ILogger<ResourcesController> _logger;
        private readonly ProjectConfig.DataPathConfig _config;

        public ResourcesController(ILogger<ResourcesController> logger, ProjectConfig config)
        {
            _logger = logger;
            _config = config.Content.DataPath;
        }


        public IActionResult Document(string id)
        {
            ViewData["ErrorTitle"] = 404;
            ViewData["ErrorMessage"] = "Файл не знайдено";
            
            if (string.IsNullOrWhiteSpace(id)) return View("error");
            var data = DataUtil.GetLoadDocFileOptions(id, _config);
            if (data == null) return View("error");

            _logger.LogDebug($"send file: {data.FilePath}, {data.MimeType}, {data.FileName}");
            return PhysicalFile(data.FilePath, data.MimeType, data.FileName);
        }
    }
}