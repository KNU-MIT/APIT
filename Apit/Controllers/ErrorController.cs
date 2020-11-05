using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Apit.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        [Route("error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            string message;
            switch (statusCode)
            {
                case 404:
                    message = "Такої сторінки не існує :(";
                    break;
                default:
                    if (statusCode >= 500)
                        message = "На сервері виникла якась помилка... <br>" +
                                  "Ми найближчим часом її обов'язково вирішино";
                    else message = "Виникла невідома помилка";
                    break;
            }

            ViewBag.ErrorMessage = $"<h1><{statusCode}/h1>{message}";

            if (statusCode != 404) _logger.LogError($"error handled [code:{statusCode}]");
            return View("error");
        }
    }
}