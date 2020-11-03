using System.Diagnostics;
using System.Threading.Tasks;
using BusinessLayer;
using BusinessLayer.Models;
using DatabaseLayer;
using DatabaseLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Apit.Controllers
{
    public partial class ConferenceController : Controller
    {
        private readonly ILogger<ConferenceController> _logger;
        private readonly DataManager _dataManager;

        private readonly UserManager<User> _userManager;
        // private readonly ProjectConfig.ContentDataConfig _config;

        public ConferenceController(ILogger<ConferenceController> logger,
            DataManager dataManager, UserManager<User> userManager)
        {
            _logger = logger;
            _dataManager = dataManager;
            _userManager = userManager;
            // _config = config.Content.Conference;
        }


        [AllowAnonymous]
        public IActionResult Index(string id)
        {
            var current = string.IsNullOrWhiteSpace(id)
                ? _dataManager.Conferences.Current
                : _dataManager.Conferences.GetByUniqueAddress(id);

            return View(current);
        }


        [Route("join-now"), AllowAnonymous]
        public async Task<IActionResult> JoinNow()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("register", "account");

            var user = await _userManager.GetUserAsync(User);
            var conference = _dataManager.Conferences.GetCurrentAsDbModel();

            _dataManager.Conferences.AddParticipant(conference, user);
            _dataManager.Conferences.SaveChanges();

            ViewData["ResultMessage"] = "<span>Добро пожаловать!</span>";
            return RedirectToAction("index", "conference");
        }

        [Authorize]
        public async Task<IActionResult> Unsubscribe()
        {
            var user = await _userManager.GetUserAsync(User);
            var conference = _dataManager.Conferences.GetCurrentAsDbModel();

            _logger.LogDebug("Id" + user.Id);

            _dataManager.Conferences.RemoveParticipant(conference, user);
            _dataManager.Conferences.SaveChanges();

            ViewData["ResultMessage"] = "<span>Ви больше не с нами</span>";
            return RedirectToAction("index", "conference");
        }

        [Authorize]
        public IActionResult Archive()
        {
            var conferences = _dataManager.Conferences.GetAll();
            return View(conferences);
        }

        [Route("move-to-archive"), Authorize(Roles = RoleNames.SEMPAI)]
        public IActionResult MoveToArchive()
        {
            var current = _dataManager.Conferences.Current;
            current.IsActual = false;
            _dataManager.Conferences.SaveChanges();

            _logger.LogInformation($"Conference {current.UniqueAddress} archived");

            return RedirectToAction("archive", "conference");
        }

        [AllowAnonymous, ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}