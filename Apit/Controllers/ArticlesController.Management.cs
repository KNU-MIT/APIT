using System;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Models;
using DatabaseLayer;
using DatabaseLayer.Entities;
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
            var articleViewModel = _dataManager.Articles.GetByUniqueAddress(x);
            if (articleViewModel == null)
            {
                ViewData["ErrorTitle"] = 404;
                ViewData["ErrorMessage"] = "Нічого не знайдено :(";
                return View("error");
            }

            var article = _dataManager.Articles.GetByUniqueAddressAsDbObject(x);
            var user = await _userManager.GetUserAsync(User);
            bool isAdmin = await _userManager.IsInRoleAsync(user, RoleNames.ADMIN);
            var current = _dataManager.Conferences.Current;
            var conference = _dataManager.Conferences.GetById(articleViewModel.Topic.ConferenceId);

            if (!Enum.TryParse<ArticleStatus>(status, out var statusOption))
            {
                if (isAdmin || (conference.Id == current.Id && articleViewModel.Creator == user))
                    return View(articleViewModel);
            }
            else
            {
                article.Status = statusOption;
                _dataManager.Articles.SaveChanges();
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
        public async Task<IActionResult> Edit(ArticleViewModel model, string x, string returnUrl)
        {
            if (!ModelState.IsValid) return View(model);

            var article = _dataManager.Articles.GetByUniqueAddressAsDbObject(x);
            if (article == null)
            {
                ViewData["ErrorTitle"] = 404;
                ViewData["ErrorMessage"] = "Стаття не знайдена, або її не існує :(";
                return View("error");
            }

            returnUrl ??= "/articles/list";
            var user = await _userManager.GetUserAsync(User);
            bool isAdmin = await _userManager.IsInRoleAsync(user, RoleNames.ADMIN);

            if (model.Creator != user && !isAdmin)
            {
                ModelState.AddModelError(string.Empty,
                    "Ви не маєте доступу до цієї опції");
                return View(model);
            }

            bool hasChanges = false;

            // Set new article topic
            if (!string.IsNullOrWhiteSpace(model.TopicId))
            {
                var topic = _dataManager.Conferences.Current.Topics
                    .FirstOrDefault(a => a.Id.ToString() == model.TopicId);

                if (topic != null)
                {
                    article.TopicId = topic.Id;
                    hasChanges = true;
                }
            }
            else
                ModelState.AddModelError(nameof(ArticleViewModel.TopicId),
                    "Дана тема не може бути використана");

            // Set new article title
            if (!string.IsNullOrEmpty(model.Title) && model.Title != article.Title)
            {
                article.Title = model.Title;
                hasChanges = true;
            }

            // Set new article key words
            if (model.KeyWordsAreCorrect && model.KeyWords != article.KeyWords)
            {
                article.KeyWords = model.KeyWords;
                hasChanges = true;
            }

            // Set new article short description
            if (!string.IsNullOrEmpty(model.ShortDescription) &&
                model.ShortDescription != article.ShortDescription)
            {
                article.ShortDescription = model.ShortDescription;
                hasChanges = true;
            }

            // Set new article document file
            (string extension, string uniqueAddress, string errorMessage) =
                await model.GetUploadedFile(_dataManager, _config.Content.DataPath);

            if (errorMessage == null)
            {
                _logger.LogInformation($"{user.FullName} changed file {uniqueAddress}.{extension}");
                hasChanges = true;
            }
            else ModelState.AddModelError(nameof(model.ArticleFile), errorMessage);


            // Set new article authors
            var oldAuthorsInDb = article.Authors.Where(aa =>
                !model.Authors.Contains(aa.NameString) && !aa.IsCreator);

            model.Authors = model.Authors.Where(ma => article.Authors
                    .FirstOrDefault(aa =>
                        aa.NameString != null && ma == aa.NameString) == null)
                .ToArray();

            var newInModelList = model.GenerateAuthorsLinking(_dataManager, _config.Content.UniqueAddress, user);
            foreach (var newAuthor in newInModelList) article.Authors.Add(newAuthor);

            _dataManager.Articles.DeleteLinkedUser(oldAuthorsInDb);


            // Update database content
            if (hasChanges)
            {
                article.Status = article.Status == ArticleStatus.Published ||
                                 article.Status == ArticleStatus.PublishedEdited
                    ? ArticleStatus.PublishedEdited
                    : ArticleStatus.UploadedEdited;
                _dataManager.Articles.SaveChanges();
            }

            return LocalRedirect(returnUrl);
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

                bool isAdmin = await _userManager.IsInRoleAsync(user, RoleNames.ADMIN);
                if (article.IsAuthor(user) || isAdmin)
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