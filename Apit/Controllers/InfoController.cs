using Microsoft.AspNetCore.Mvc;

namespace Apit.Controllers
{
    public class InfoController : Controller
    {
        public IActionResult Dates()
        {
            return View();
        }
        
        public IActionResult Requirements()
        {
            return View();
        }
        
        public IActionResult Secretariat()
        {
            return View();
        }
    }
}