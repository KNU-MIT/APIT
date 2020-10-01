﻿using System.Threading.Tasks;
using BusinessLayer;
using BusinessLayer.Models;
using DatabaseLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Apit.Controllers
{
    public partial class AccountController
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly DataManager _dataManager;

        public AccountController(SignInManager<User> signInManager,
            UserManager<User> userManager, DataManager dataManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _dataManager = dataManager;
        }


        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(new LoginViewModel {ReturnUrl = returnUrl});
        }

        [AllowAnonymous, HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _signInManager.PasswordSignInAsync
                (model.Email, model.Password, model.RememberMe, false);

            if (result.Succeeded) return Redirect(model.ReturnUrl ?? "/");

            ModelState.AddModelError(nameof(LoginViewModel.Email), "User don't registered");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}