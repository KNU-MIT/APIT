using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer;
using BusinessLayer.DataServices.ConfigModels;
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
        private readonly ProjectConfig.ContentConfig _config;

        public ConferenceController(ILogger<ConferenceController> logger,
            DataManager dataManager, UserManager<User> userManager, ProjectConfig config)
        {
            _logger = logger;
            _dataManager = dataManager;
            _userManager = userManager;
            _config = config.Content;
        }


        [Authorize]
        public async Task<IActionResult> Index(string id)
        {
            var current = string.IsNullOrWhiteSpace(id)
                ? _dataManager.Conferences.Current
                : _dataManager.Conferences.GetByUniqueAddress(id);

            if (current == null) return RedirectToAction("index", "home");

            var user = await _userManager.GetUserAsync(User);
            current.ParticipantChan = current.Participants.FirstOrDefault(a => a.User == user);

            return View(current);
        }


        [Route("/conference/join-now"), Authorize, HttpPost]
        public async Task<IActionResult> JoinNow(ConferenceViewModel model)
        {
            var newParticipant = model.ParticipantChan;

            var user = await _userManager.GetUserAsync(User);
            var conference = _dataManager.Conferences.GetCurrentAsDbModel();

            _dataManager.Conferences.AddParticipant(conference, new ConferenceParticipant
            {
                Id = Guid.NewGuid(),
                ParticipationForm = newParticipant.ParticipationForm,
                AdditionalConditions = newParticipant.AdditionalConditions,
                User = user,
                UserId = user.Id
            });
            _dataManager.Conferences.SaveChanges();

            return RedirectToAction("index", "conference");
        }

        [Authorize]
        public async Task<IActionResult> Unsubscribe()
        {
            var user = await _userManager.GetUserAsync(User);
            var conference = _dataManager.Conferences.GetCurrentAsDbModel();

            _dataManager.Conferences.RemoveParticipant(conference, user);
            _dataManager.Conferences.SaveChanges();

            return RedirectToAction("index", "conference");
        }

        [Authorize]
        public IActionResult Archive()
        {
            var conferences = _dataManager.Conferences.GetAll().Where(conf => !conf.IsActual);
            return View(conferences);
        }

        [Route("/conference/move-to-archive"), Authorize(Roles = RoleNames.ADMIN)]
        public IActionResult MoveToArchive()
        {
            var current = _dataManager.Conferences.GetCurrentAsDbModel();
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