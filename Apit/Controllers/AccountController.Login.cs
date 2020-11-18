using System.Threading.Tasks;
using Apit.Service;
using BusinessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Apit.Controllers
{
    // Maybe it is better to use the integrated Account ASP.NET functionality (Areas/Identity/Pages/Account(/Manage))
    public partial class AccountController
    {
        public IActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(new LoginViewModel {ReturnUrl = returnUrl});
        }

        [ValidateAntiForgeryToken, HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            // if (!ModelState.IsValid) return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);

            var result = await _signInManager.PasswordSignInAsync
                (model.Email, model.Password, true, false);

            if (result.Succeeded)
            {
                _logger.LogDebug($"User {user.ProfileAddress} has logged in");
                return LocalRedirect(model.ReturnUrl ?? "/");
            }

            ModelState.AddModelError(string.Empty, "невірно введено дані");
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            _logger.LogDebug("User logged out");
            await _signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }

        [Route("/account/send-reset")]
        public IActionResult SendReset()
        {
            return View();
        }

        [Route("/account/send-reset"), HttpPost]
        public IActionResult SendReset(LoginViewModel model)
        {
            var user = _dataManager.Users.GetByEmail(model.Email);
            if (user == null)
            {
                ModelState.AddModelError(nameof(model.Email),
                    "Користувача з такою поштою не знайдено");
                return View(model);
            }

            string confirmationToken = _userManager.GeneratePasswordResetTokenAsync(user).Result;

            string confirmationLink = Url.Action("ChangePassword", "account", new
            {
                id = user.Id,
                token = confirmationToken
            }, protocol: HttpContext.Request.Scheme);

            _mailService.SendActionEmail(user.Email,
                "APIT | Зміна пароля на сайті конференції",
                MailService.Presets.ResetPassword, confirmationLink);
            _logger.LogInformation("Password reset email was sent to: " + user.Email);

            ModelState.AddModelError(string.Empty,
                "Вам на пошту відпрвлено лист для зміни пароля");

            return RedirectToAction("login", "account");
        }

        [Route("/account/change-password")]
        public IActionResult ChangePassword(string id, string token)
        {
            var userFromEmail = _dataManager.Users.GetById(id);
            if (userFromEmail != null)
                return View(new ResetPasswordViewModel
                {
                    User = userFromEmail,
                    Token = token
                });
            
            ViewData["ErrorTitle"] = 404;
            ViewData["ErrorMessage"] = "Щось пішло не так...";
            return View("error");

        }

        [Route("/account/change-password"), HttpPost]
        public async Task<IActionResult> ChangePassword(ResetPasswordViewModel model, string id, string token)
        {
            if (!ModelState.IsValid) return View(model);

            var user = _dataManager.Users.GetById(id);
            if (user == null)
            {
                ViewData["ErrorTitle"] = 404;
                ViewData["ErrorMessage"] = "Щось пішло не так...";
                return View("error");
            }

            var result = await _userManager.ResetPasswordAsync(user, token, model.PasswordConfirm);
            if (result.Succeeded)
            {
                _logger.LogWarning($"User {user.ProfileAddress} password changed");
                ViewData["Message"] = "Ви успішно змінили пароль Вашу пошту!";
                return View("success");
            }

            _logger.LogError($"User {user.ProfileAddress} NOT changed password");
            
            
            ViewData["ErrorTitle"] = 403;
            ViewData["ErrorMessage"] = "Щось пішло не так...";
            return View("error");
        }
    }
}