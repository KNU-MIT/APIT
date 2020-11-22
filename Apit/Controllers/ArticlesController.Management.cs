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

            returnUrl ??= "/articles?x=" + x;
            var user = await _userManager.GetUserAsync(User);
            bool isAdmin = await _userManager.IsInRoleAsync(user, RoleNames.ADMIN);

            if (model.Creator != user && !isAdmin)
            {
                ModelState.AddModelError(string.Empty,
                    "Ви не маєте доступу до цієї опції");
                return View(model);
            }

            var article = _dataManager.Articles.GetByUniqueAddressAsDbObject(x);
            if (article == null)
            {
                ViewData["ErrorTitle"] = 404;
                ViewData["ErrorMessage"] = "Стаття не знайдена, або її не існує :(";
                return View("error");
            }

            var articleAuthorsList = _dataManager.Articles.GetLinkedUsers(article.Id).ToList();


            bool hasChanges = false;


            // Set new article topic
            if (!string.IsNullOrWhiteSpace(model.TopicId))
            {
                var topic = _dataManager.Conferences.Current.Topics
                    .FirstOrDefault(a => a.Id.ToString() == model.TopicId);

                if (topic != null && topic.Id != article.TopicId)
                {
                    article.TopicId = topic.Id;
                    hasChanges = true;
                    // Console.WriteLine("New Topic: " + article.TopicId);
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
                // Console.WriteLine("New Title: " + article.Title);
            }

            // Set new article key words
            if (model.KeyWordsAreCorrect && model.KeyWords != article.KeyWords)
            {
                article.KeyWords = model.KeyWords;
                hasChanges = true;
                // Console.WriteLine("New KeyWords: " + article.KeyWords);
            }

            // Set new article short description
            model.ShortDescription = model.ShortDescription.Trim();
            if (!string.IsNullOrEmpty(model.ShortDescription) &&
                model.ShortDescription != article.ShortDescription)
            {
                article.ShortDescription = model.ShortDescription;
                hasChanges = true;
                // Console.WriteLine("New ShortDescription: " + article.ShortDescription);
            }

            // Set new article document file
            (string extension, string uniqueAddress, string errorMessage) =
                await model.GetUploadedFile(_dataManager, _config.Content.DataPath);

            if (errorMessage == null)
            {
                _logger.LogInformation($"{user.FullName} changed file {uniqueAddress}.{extension}");
                article.HtmlFilePath = uniqueAddress + ".htm";
                article.DocxFilePath = uniqueAddress + extension;

                hasChanges = true;
                Console.WriteLine("New DocxFilePath: " + article.DocxFilePath);
            }
            else
            {
                ModelState.AddModelError(nameof(model.ArticleFile), errorMessage);
                _logger.LogError(errorMessage);
            }


            // Set new article authors
            var oldAuthorsInDbList = articleAuthorsList.Where(aa =>
                !model.Authors.Contains(aa.NameString) && !aa.IsCreator).ToList();

            model.Authors = model.Authors.Where(ma => ma != null && articleAuthorsList
                .FirstOrDefault(aa => ma == aa.NameString) == null).ToArray();

            var newInModelList = model.GenerateAuthorsLinking(
                _dataManager, _config.Content.UniqueAddress, user, article);

            if (newInModelList.Count > 0)
            {
                _dataManager.Articles.CreateLinkedUsers(newInModelList);
                hasChanges = true;
                // Console.WriteLine("New CreateLinkedUsers: " + string.Join(", ",
                // newInModelList.Select(a => a.NameString)));
            }

            if (oldAuthorsInDbList.Count > 0)
            {
                _dataManager.Articles.DeleteLinkedUsers(oldAuthorsInDbList);
                hasChanges = true;
                // Console.WriteLine("New DeleteLinkedUser: " + string.Join(", ",
                // oldAuthorsInDbList.Select(a => a.NameString)));
            }


            // Update database content
            if (hasChanges)
            {
                article.Status = article.Status == ArticleStatus.Published ||
                                 article.Status == ArticleStatus.PublishedEdited
                    ? ArticleStatus.PublishedEdited
                    : ArticleStatus.UploadedEdited;
                // Console.WriteLine("New Status: " + article.Status);
                article.DateLastModified = DateTime.Now;

                _dataManager.Articles.Update(article);
                _logger.LogInformation($"Article {article.UniqueAddress} edited via user {user.FullName}");

                article.Options = new Article.DisplayOptions
                {
                    Topic = _dataManager.Topics.GetById(article.TopicId),
                    PageAbsoluteUrl = Url.ActionLink("index", "articles", new {x = uniqueAddress}),
                    DocumentAbsoluteUrl = Url.ActionLink("document", "resources", new {id = article.DocxFilePath})
                };

                foreach (string email in _config.EmailsForGetInfo)
                    _mailService.SendArticleInfoEmail(email, article, _dataManager,
                        _config.MailboxDefaults.MailSubjects.ArticleEditedSubject);
            }

            return LocalRedirect(returnUrl);
        }

        /// <summary>
        /// Delete one article
        /// Available option for the article creator and root admin 
        /// </summary>
        /// <param name="x">Unique article route address</param>
        /// <param name="returnUrl">Route to redirect after processing request</param>
        [Authorize, HttpPost]
        public async Task<IActionResult> Delete(string x, string returnUrl = null)
        {
            try
            {
                var article = _dataManager.Articles.GetByUniqueAddressAsDbObject(x);

                if (article == null)
                {
                    ViewData["ErrorTitle"] = 404;
                    ViewData["ErrorMessage"] = "Статтю не знайдено, або її не існує";
                    return View("error");
                }

                var authors = _dataManager.Articles.GetByUniqueAddress(x).AuthorUsers;
                var user = await _userManager.GetUserAsync(User);

                bool isAdmin = await _userManager.IsInRoleAsync(user, RoleNames.ADMIN);
                if (Article.IsAuthor(user, authors) || isAdmin)
                {
                    _dataManager.Articles.Delete(article);
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