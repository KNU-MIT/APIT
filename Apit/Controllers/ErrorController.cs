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

        [Route("/error")]
        public IActionResult HttpStatusCodeHandler()
        {
            if (ViewData["ErrorTitle"] == default) ViewData["ErrorTitle"] = 500;
            if (ViewData["ErrorMessage"] == default) ViewData["ErrorMessage"] = "Щось пішло не так...";

            // string message;
            // switch (statusCode)
            // {
            //     case 404:
            //         message = "Такої сторінки не існує :(";
            //         break;
            //     default:
            //         if (statusCode >= 500)
            //             message = "На сервері виникла якась помилка... <br>" +
            //                       "Ми найближчим часом її обов'язково вирішино";
            //         else message = "Виникла невідома помилка";
            //         break;
            // }

            // ViewData["ErrorTitle"] =  statusCode;
            // ViewData["ErrorMessage"] = message;

            if ((int) ViewData["ErrorTitle"] != 404)
                _logger.LogError($"error handled [code:{ViewData["ErrorTitle"]}]");
            return View("error");
        }
    }
}