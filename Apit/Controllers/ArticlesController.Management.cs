using System;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Models;
using DatabaseLayer;
using DatabaseLayer.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Apit.Controllers
{
    public partial class ArticlesController
    {
        /// <summary>
        /// Goto article edit view page
        /// </summary>
        /// <param name="x">Unique article route address</param>
        /// <param name="status">Non required param to specify action type</param>
        [Authorize]
        public async Task<IActionResult> Edit(string x, string status = null)
        {
            var model = _dataManager.Articles.GetByUniqueAddress(x);
            if (model == null)
            {
                ViewData["ErrorTitle"] = 404;
                ViewData["ErrorMessage"] = "Нічого не знайдено :(";
                return View("error");
            }

            var user = await _userManager.GetUserAsync(User);
            bool isAdmin = await _userManager.IsInRoleAsync(user, RoleNames.ADMIN);
            var current = _dataManager.Conferences.Current;
            var conference = _dataManager.Conferences.GetById(model.Topic.ConferenceId);

            if (!Enum.TryParse<ArticleStatus>(status, out var statusOption))
            {
                if (isAdmin || (conference.Id == current.Id && model.Creator == user))
                    return View(model);
            }
            else if (statusOption == ArticleStatus.Published || statusOption == ArticleStatus.Banned)
            {
                model.Status = statusOption;
                return RedirectToAction("index", "articles", new {x});
            }

            ViewData["ErrorTitle"] = 403;
            ViewData["ErrorMessage"] = "Доступ до даної опції заблоковано";
            return View("error");
        }

        /// <summary>
        /// Applying article editing from form by POST action method
        /// Available option for the article creator and root admin 
        /// </summary>
        [HttpPost, Authorize]
        public async Task<IActionResult> Edit(ArticleViewModel model, string returnUrl)
        {
            var dbModel = _dataManager.Articles.GetByUniqueAddress(model.UniqueAddress);
            if (dbModel == null)
            {
                ViewData["ErrorTitle"] = 404;
                ViewData["ErrorMessage"] = "Нічого не знайдено :(";
                return View("error");
            }

            var user = await _userManager.GetUserAsync(User);
            bool isAdmin = await _userManager.IsInRoleAsync(user, RoleNames.ADMIN);

            if (model.Creator != user || !isAdmin)
            {
                ModelState.AddModelError(string.Empty,
                    "Ви не маєте доступу до цієї опції");
                return View(model);
            }

            if (!string.IsNullOrWhiteSpace(model.NewTopicId)
                && model.NewTopicId != model.Topic.Id.ToString())
            {
                var topic = _dataManager.Conferences.Current.Topics
                    .FirstOrDefault(a => a.Id.ToString() == model.NewTopicId);
                if (topic != null) model.Topic = topic;
            }
            else
                ModelState.AddModelError(nameof(ArticleViewModel.NewTopicId),
                    "Дана тема не може бути використана");

            return LocalRedirect(returnUrl ?? "/articles/list");
        }

        /// <summary>
        /// Delete one article
        /// Available option for the article creator and root admin 
        /// </summary>
        /// <param name="x">Unique article route address</param>
        /// <param name="returnUrl">Route to redirect after processing request</param>
        [Authorize]
        public async Task<IActionResult> Delete(string x, string returnUrl = null)
        {
            try
            {
                var article = _dataManager.Articles.GetByUniqueAddress(x);
                var user = await _userManager.GetUserAsync(User);

                if (article.IsAuthor(user) || await _userManager.IsInRoleAsync(user, RoleNames.ADMIN))
                {
                    _dataManager.Articles.Delete(article.Id);
                    return LocalRedirect(returnUrl ?? "/");
                }

                ViewData["ErrorTitle"] = 403;
                ViewData["ErrorMessage"] = "Доступ заблоковано :(";
                return View("error");
            }
            catch (Exception e)
            {
                _logger.LogError("Article not deleted with exception: " + e);
                ViewData["ErrorTitle"] = 500;
                ViewData["ErrorMessage"] = "Щось пішло не так...";
                return View("error");
            }
        }
    }
}