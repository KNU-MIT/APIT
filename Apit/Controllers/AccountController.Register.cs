using System;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Models;
using DatabaseLayer;
using DatabaseLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Apit.Controllers
{
    // Maybe it is better to use the integrated Account ASP.NET functionality (Areas/Identity/Pages/Account(/Manage))
    public partial class AccountController
    {
        public IActionResult Register(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(new RegisterViewModel {ReturnUrl = returnUrl});
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            if (!string.IsNullOrWhiteSpace(model.MailboxIndex)
                && !int.TryParse(model.MailboxIndex, out int mailboxIndex))
            {
                ModelState.AddModelError(nameof(RegisterViewModel.MailboxIndex),
                    "Невірно введено поштовий індекс");
                return View(model);
            }

            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                ProfileAddress = _dataManager.Users.GenerateUniqueAddress(),

                FirstName = model.FirstName,
                LastName = model.LastName,
                MiddleName = model.MiddleName,

                WorkingFor = model.WorkingFor,
                ScienceDegree = model.ScienceDegree,
                AcademicTitle = model.AcademicTitle,

                PhoneNumber = model.PhoneNumber,
                MailboxIndex = model.MailboxIndex,
                InfoSourceName = model.InfoSourceName,

                Email = model.Email,
                UserName = model.Email
            };

            bool isFirst = !_userManager.Users.Any();

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                if (isFirst) // TODO: not secure part of code
                {
                    var roleResult1 = await _userManager.AddToRoleAsync(user, RoleNames.ADMIN);
                    var roleResult2 = await _userManager.AddToRoleAsync(user, RoleNames.MANAGER);

                    if (roleResult1.Succeeded && roleResult2.Succeeded)
                        _logger.LogInformation("First user authorized as admin with full access");

                    else ModelState.AddModelError(string.Empty, "You are not admin!");
                }


                string confirmationToken = _userManager.GenerateEmailConfirmationTokenAsync(user).Result;

                string confirmationLink = Url.Action
                ("ConfirmEmail", "account", new
                {
                    id = user.Id,
                    token = confirmationToken
                }, protocol: HttpContext.Request.Scheme);

                _mailService.SendConfirmationEmail(user.Email, confirmationLink);


                await _signInManager.SignInAsync(user, false);
                _logger.LogInformation($"User {user.ProfileAddress} has successfully registered");

                // Send confirmation email before redirect via return url 
                return RedirectToAction("SendConfirm", "account", new {returnUrl = model.ReturnUrl});
            }

            // if something goes wrong

            ModelState.AddModelError(model.Email, "Неможливо створити користувача");
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(model);
        }


        [Route("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string id, string token)
        {
            if (!string.IsNullOrEmpty(id) && !string.IsNullOrWhiteSpace(token))
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user != null)
                {
                    var result = await _userManager.ConfirmEmailAsync(user, token);

                    if (result.Succeeded)
                    {
                        _logger.LogInformation($"User {user.ProfileAddress} confirmed mail");
                        ViewData["Message"] = "Ви успішно підтвердили Вашу пошту!";
                        return View("success");
                    }

                    _logger.LogWarning($"User {user.FullName} NOT confirmed mail");
                }
            }

            ViewData["ErrorTitle"] = 403;
            ViewData["ErrorMessage"] = "Упс, виникла помилка...";
            return View("error");
        }
    }
}