using System;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Models;
using DatabaseLayer;
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
        [Authorize]
        public async Task<IActionResult> Edit(string x)
        {
            var model = _dataManager.Articles.GetByUniqueAddress(x);
            
            var user = await _userManager.GetUserAsync(User);
            bool isAdmin = await _userManager.IsInRoleAsync(user, RoleNames.ADMIN);

            if (model.Creator == user || isAdmin) return View(model);
            
            ViewData["ErrorTitle"] = 403;
            ViewData["ErrorMessage"] = "Доступ заблоковано";
            return View("error");

        }

        /// <summary>
        /// Applying article editing from form by POST action method
        /// Available option for the article creator and root admin 
        /// </summary>
        [HttpPost, Authorize]
        public async Task<IActionResult> Edit(ArticleViewModel model)
        {
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
                    "Дана тема не може буди використана");

            return View(model);
        }

        /// <summary>
        /// Delete one article
        /// Available option for the article creator and root admin 
        /// </summary>
        /// <param name="x">Unique article route address</param>
        /// <param name="returnUrl">Route to redirect after processing request</param>
        [HttpPost, Authorize]
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
                ViewData["ErrorMessage"] = "Доступ заблоковано";
                return View("error");
            }
            catch (Exception e)
            {
                _logger.LogError("Article not deleted with exception: " + e);
                ViewData["ErrorTitle"] = 500;
                ViewData["ErrorMessage"] = "На превеликий жаль, такої людини серед нас немає :(";
                return View("error");
            }
        }
    }
}