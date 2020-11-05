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
                (model.Email, model.Password, model.RememberMe, false);

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


        [Route("send-reset"), HttpPost]
        public async Task<IActionResult> SendReset(string email)
        {
            ModelState.AddModelError(nameof(LoginViewModel.Password),
                "Вам на пошту вудпрвлено лист для зміни пароля");

            var user = await _userManager.GetUserAsync(User);
            string confirmationToken = _userManager.GeneratePasswordResetTokenAsync(user).Result;

            string confirmationLink = Url.Action
            ("ConfirmEmail", "account", new
            {
                id = user.Id,
                token = confirmationToken
            }, protocol: HttpContext.Request.Scheme);

            _mailService.SendActionEmail(user.Email,
                "Зміна пароля на сайті конференції",
                MailService.Presets.ResetPassword, confirmationLink);
            _logger.LogDebug("Password reset email was sent to: " + user.Email);

            return View("login");
        }

        [Route("change-password"), Authorize]
        public async Task<IActionResult> ChangePassword(string id, string token)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user.Id != id) return View("error");
            return View(new ResetPasswordViewModel
            {
                User = id,
                Token = token
            });
        }

        [Route("change-password"), Authorize, HttpPost]
        public async Task<IActionResult> ChangePassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user.Id != model.User) return View("error");

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.PasswordConfirm);
            if (result.Succeeded)
            {
                _logger.LogError($"User {user.ProfileAddress} password changed");
                ViewBag.Message = "Ви успішно змінили пароль!";
                return View("success");
            }

            _logger.LogError($"User {user.ProfileAddress} NOT changed password");
            ViewBag.Message = "Упс, виникла помилка...";
            return View("error");
        }
    }
}