using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BusinessLayer.Models;
using Microsoft.AspNetCore.Authorization;

namespace Apit.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Administration()
        {
            return View();
        }

        [Route("/manage/developers"), AllowAnonymous]
        public IActionResult DevelopersHiddenPage()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}