using System;
using System.Text;
using System.Threading.Tasks;
using Apit.Service;
using BusinessLayer;
using DatabaseLayer.ConfigModels;
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
        private readonly SecurityConfig _security;

        private readonly string _passwordRules;


        public AccountController(ILogger<AccountController> logger, SignInManager<User> signInManager,
            UserManager<User> userManager, DataManager dataManager, MailService mailService,
            ProjectConfig projectConfig, SecurityConfig securityConfig)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _dataManager = dataManager;
            _mailService = mailService;
            _config = projectConfig;
            _security = securityConfig;

            _passwordRules = GeneratePasswordRules();
        }

        private string GeneratePasswordRules()
        {
            var str = new StringBuilder();
            var pass = _security.Password;

            str.Append("<b>Пароль повинен відповідати наступним критеріям:</b><br>");

            str.Append($"Мінімальна довжина: {pass.RequiredLength} символів<br>");
            str.Append($"Унікальних символів: {pass.RequiredUniqueChars}<br>");

            if (pass.RequireNonAlphanumeric)
                str.Append("Як мінімум 1 спецсимвол (@#$%)<br>");

            if (pass.RequireNonAlphanumeric)
                str.Append("Як мінімум 1 цифра (1234)<br>");

            if (pass.RequireLowercase && pass.RequireUppercase)
                str.Append(
                    "Як мінімум 1 символ латинського алфавіту у нижньому (abcd) і 1 у верхньому регістрі (ABCD)<br>");
            else if (pass.RequireLowercase)
                str.Append("Як мінімум 1 символ латинського алфавіту у нижньому регістрі (abcd)<br>");
            else if (pass.RequireUppercase)
                str.Append("Як мінімум 1 символ латинського алфавіту у верхньому регістрі (ABCD)<br>");

            return str.ToString();
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


        [HttpPost]
        public async Task<IActionResult> SetManager(string x, string newState)
        {
            var targetUser = _dataManager.Users.GetByUniqueAddress(x);
            if (targetUser == null)
            {
                ViewData["ErrorTitle"] = 404;
                ViewData["ErrorMessage"] = "На превеликий жаль, такої людини серед нас немає :(";
                return View("error");
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null || !await _userManager.IsInRoleAsync(user, RoleNames.ADMIN))
            {
                ViewData["ErrorTitle"] = 403;
                ViewData["ErrorMessage"] = "У вас немає доступу до цієї опції.";
                return View("error");
            }

            var result = newState == "manager"
                ? await _userManager.AddToRoleAsync(targetUser, RoleNames.MANAGER)
                : await _userManager.RemoveFromRoleAsync(targetUser, RoleNames.MANAGER);

            if (result.Succeeded)
            {
                _logger.LogInformation($"User {targetUser} role changed to {newState}");
                return RedirectToAction("index", "account", new {x});
            }

            ViewData["ErrorTitle"] = 403;
            ViewData["ErrorMessage"] = "Щось пішло не так...";
            return View("error");
        }


        [Authorize, HttpPost]
        public async Task<IActionResult> Edit(User model)
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


            model.MailboxIndex = model.MailboxIndex.Trim();
            if (!string.IsNullOrWhiteSpace(model.MailboxIndex))
                user.MailboxIndex = model.MailboxIndex;

            model.PhoneNumber = model.PhoneNumber.Trim();
            if (!string.IsNullOrWhiteSpace(model.PhoneNumber))
                user.PhoneNumber = model.PhoneNumber;

            _dataManager.Users.SaveChanges();

            _logger.LogInformation($"User {user.ProfileAddress} has changed his data");

            ModelState.AddModelError(string.Empty,
                "Дані змінено успішно");
            return RedirectToAction("index", "account");
        }


        [Route("/account/send-confirm")]
        public async Task<IActionResult> SendConfirm(string returnUrl)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("index", "home");

            string confirmationToken = _userManager.GenerateEmailConfirmationTokenAsync(user).Result;

            string confirmationLink = Url.Action
            ("ConfirmEmail", "account", new
            {
                id = user.Id,
                token = confirmationToken
            }, protocol: HttpContext.Request.Scheme);

            _mailService.SendConfirmationEmail(user.Email, confirmationLink);

            if (returnUrl != null) return LocalRedirect(returnUrl);
            return RedirectToAction("index", "account", new {x = user.ProfileAddress});
        }
    }
}