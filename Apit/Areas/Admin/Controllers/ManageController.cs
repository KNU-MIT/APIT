using BusinessLayer;
using DatabaseLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Apit.Areas.Admin.Controllers
{
    [Authorize(Roles = RoleNames.ADMIN)]
    public class ManageController : Controller
    {
        private readonly ILogger<ManageController> _logger;
        private readonly DataManager _dataManager;

        public ManageController(DataManager dataManager)
        {
            _dataManager = dataManager;
        }

        public IActionResult ConferenceStatistics(string x)
        {
            return View(x == null
            ? _dataManager.Conferences.Current
            : _dataManager.Conferences.GetByUniqueAddress(x));
        }
    }
}