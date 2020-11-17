using System.Linq;
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

        [Route("/manage/conference/statistics")]
        public IActionResult ConferenceStatistics(string x)
        {
            return View(x == null
                ? _dataManager.Conferences.Current
                : _dataManager.Conferences.GetByUniqueAddress(x));
        }


        public JsonResult GetParticipantsData(string conf, string field)
        {
            _logger.LogInformation("Attempt to get participants data detected");
            var conference = _dataManager.Conferences.GetByUniqueAddress(conf);
            if (conference == null) return new JsonResult(null);
            var users = conference.Participants.Select(u => _dataManager.Users.GetById(u.UserId));

            return field.ToLower() switch
            {
                "email" => new JsonResult(users.Select(u => u.Email)),
                "name" => new JsonResult(users.Select(u => u.FullName)),
                "address" => new JsonResult(users.Select(u => u.ProfileAddress)),
                _ => new JsonResult(null)
            };
        }
    }
}