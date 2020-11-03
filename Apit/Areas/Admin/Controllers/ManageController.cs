using DatabaseLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Apit.Areas.Admin.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        [Authorize(Roles = RoleNames.ADMIN)]
        public IActionResult Permissions()
        {
            return View();
        }
    }
}