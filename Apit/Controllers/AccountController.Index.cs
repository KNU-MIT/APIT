using System.Threading.Tasks;
using Apit.Service;
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
    // TODO: Maybe it is better to use the integrated Account ASP.NET functionality (Areas/Identity/Pages/Account(/Manage))
    public partial class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly DataManager _dataManager;
        private readonly MailService _mailService;
        private readonly ProjectConfig _config;

        public AccountController(ILogger<AccountController> logger, SignInManager<User> signInManager,
            UserManager<User> userManager, DataManager dataManager, MailService mailService, ProjectConfig config)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _dataManager = dataManager;
            _mailService = mailService;
            _config = config;
        }


        [Authorize]
        public async Task<IActionResult> Index(string x)
        {
            var user = x == null
                ? await _userManager.GetUserAsync(User)
                : _dataManager.Users.GetByUniqueAddress(x);

            ViewData["ErrorTitle"] = 404;
            ViewData["ErrorMessage"] = "На превеликий жаль, такого користувача серед нас немає :(";
            return user == null ? View("error") : View(user);
        }

        [Route("/account/access-denied")]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [Authorize(Roles = RoleNames.ADMIN)]
        public async Task<IActionResult> SetManager(string x, string newState)
        {
            var user = _dataManager.Users.GetByUniqueAddress(x);
            if (user == null)
            {
                ViewData["ErrorTitle"] = 404;
                ViewData["ErrorMessage"] = "На превеликий жаль, такої людини серед нас немає :(";
                return View("error");
            }

            var result = newState == "manager"
                ? await _userManager.AddToRoleAsync(user, RoleNames.MANAGER)
                : await _userManager.RemoveFromRoleAsync(user, RoleNames.MANAGER);

            ViewData["ErrorTitle"] = 403;
            ViewData["ErrorMessage"] = "Щось пішло не так...";

            return result.Succeeded
                ? RedirectToAction("index", "account", new {x})
                : (IActionResult) ViewBag("error");
        }

        [Authorize, HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            // TODO: check why this commented code block not works
            // if (!ModelState.IsValid)
            // {
            // ModelState.AddModelError(string.Empty,
            // "Не вдалось дані змінити");
            // return RedirectToAction("index", "account");
            // }

            var user = await _userManager.GetUserAsync(User);

            model.FirstName = model.FirstName?.Trim();
            if (!string.IsNullOrWhiteSpace(model.FirstName))
                user.FirstName = model.FirstName;

            model.LastName = model.LastName?.Trim();
            if (!string.IsNullOrWhiteSpace(model.LastName))
                user.LastName = model.LastName;

            model.MiddleName = model.MiddleName?.Trim();
            if (!string.IsNullOrWhiteSpace(model.MiddleName))
                user.MiddleName = model.MiddleName;

            model.WorkingFor = model.WorkingFor?.Trim();
            if (!string.IsNullOrWhiteSpace(model.WorkingFor))
                user.WorkingFor = model.WorkingFor;


            // if (!string.IsNullOrWhiteSpace(model.AltScienceDegree))
            // {
            //     var str = model.ScienceDegree.ToString();
            //     if (user.ScienceDegree != str) user.ScienceDegree = str;
            // }
            // else if (user.ScienceDegree != model.AltScienceDegree)
            //     user.ScienceDegree = model.AltScienceDegree;

            if (user.ScienceDegree != model.ScienceDegree)
                user.ScienceDegree = model.ScienceDegree;

            // if (!string.IsNullOrWhiteSpace(model.AltAcademicTitle))
            // {
            //     var strAcad = model.AcademicTitle.ToString();
            //     if (user.AcademicTitle != strAcad) user.AcademicTitle = strAcad;
            // }
            // else if (user.AcademicTitle != model.AltAcademicTitle)
            //     user.AcademicTitle = model.AltAcademicTitle;

            if (user.AcademicTitle != model.AcademicTitle)
                user.AcademicTitle = model.AcademicTitle;

            _dataManager.Users.SaveChanges();

            _logger.LogInformation($"User {user.ProfileAddress} has changed his data");

            ModelState.AddModelError(string.Empty,
                "Дані змінено успішно");
            return RedirectToAction("index", "account");
        }
    }
}