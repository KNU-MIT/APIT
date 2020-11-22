using System;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Models;
using DatabaseLayer.Entities;
using DatabaseLayer.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Apit.Controllers
{
    public partial class ArticlesController
    {
        [Authorize]
        public IActionResult Create()
        {
            var conference = _dataManager.Conferences.Current;
            if (conference == null || (!conference.Topics?.Any() ?? true))
                return RedirectToAction("index", "account");
            return View();
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> Create(NewArticleViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var user = await _userManager.GetUserAsync(User); // included [Authorize]!

            // Combine all errors and return them back if this variable is set to true
            bool hasIncorrectData = false;

            #region ================ Long form review ================

            // Check selected topic existing
            var topic = model.TopicId == null ? null : _dataManager.Topics.GetById(Guid.Parse(model.TopicId));
            if (topic == null)
            {
                ModelState.AddModelError(nameof(model.TopicId),
                    "дана тема не може бути використана");
                hasIncorrectData = true;
            }

            // "hello, wor/*+#ld1!" != "hello, world1!"
            if (!model.KeyWordsAreCorrect)
            {
                ModelState.AddModelError(nameof(model.KeyWords),
                    "У ключових словах Ви можете використовувати лише " +
                    "цифри, літери українського та англійського алфавітів");
                hasIncorrectData = true;
            }

            // Check uploaded file
            (string extension, string uniqueAddress, string errorMessage) =
                await model.GetUploadedFile(_dataManager, _config.Content.DataPath);

            if (errorMessage != null) ModelState.AddModelError(nameof(model.ArticleFile), errorMessage);
            else _logger.LogInformation($"{user.FullName} upload file {uniqueAddress}.{extension}");

            #endregion

            if (hasIncorrectData) return View(model);

            var dateNow = DateTime.Now;


            var authorsList = model.GenerateAuthorsLinking(
                _dataManager, _config.Content.UniqueAddress, user, null);
            authorsList.Add(new UserOwnArticlesLinking
                {
                    Id = Guid.NewGuid(),
                    IsCreator = true,
                    IsRegisteredUser = true,
                    NameString = user.FullName,
                    UserId = user.Id
                }
            );

            foreach (string author in model.Authors)
            {
                if (string.IsNullOrWhiteSpace(author)) continue;
                if (authorsList.Any(a => a.NameString == author || a.UserId == author)) continue;

                if (author.Length == _config.Content.UniqueAddress.UserAddressSize)
                {
                    var addressUser = _dataManager.Users.GetByUniqueAddress(author);
                    if (addressUser != null && addressUser != user)
                    {
                        authorsList.Add(new UserOwnArticlesLinking
                        {
                            Id = Guid.NewGuid(),
                            IsCreator = false,
                            IsRegisteredUser = true,
                            NameString = addressUser.FullName,
                            UserId = addressUser.Id
                        });
                        continue;
                    }
                }

                authorsList.Add(new UserOwnArticlesLinking
                {
                    Id = Guid.NewGuid(),
                    IsCreator = false,
                    IsRegisteredUser = false,
                    NameString = author,
                    UserId = user.Id
                });
            }

            string docxFilePath = uniqueAddress + extension;

            var article = new Article
            {
                Id = Guid.NewGuid(),
                TopicId = topic.Id,
                Authors = authorsList,
                UniqueAddress = uniqueAddress,

                Title = model.Title,
                ShortDescription = model.ShortDescription.Trim(),
                Status = ArticleStatus.Uploaded,
                KeyWords = model.KeyWords,

                HtmlFilePath = uniqueAddress + ".htm",
                DocxFilePath = docxFilePath,

                Conference = _dataManager.Conferences.GetCurrentAsDbModel(),

                DateCreated = dateNow,
                DateLastModified = dateNow,

                Options = new Article.DisplayOptions
                {
                    Topic = topic,
                    PageAbsoluteUrl = Url.ActionLink("index", "articles", new {x = uniqueAddress}),
                    DocumentAbsoluteUrl = Url.ActionLink("document", "resources", new {id = docxFilePath})
                }
            };

            foreach (var author in article.Authors)
                author.ArticleId = article.Id;

            var currentConference = _dataManager.Conferences.GetCurrentAsDbModel();
            _dataManager.Conferences.AddArticle(currentConference, article);
            _dataManager.Articles.Create(article);

            foreach (string email in _config.EmailsForGetInfo)
                _mailService.SendArticleInfoEmail(email, article, _dataManager,
                    _config.MailboxDefaults.MailSubjects.ArticleEditedSubject);

            return RedirectToAction("index", "articles", new {x = uniqueAddress});
        }
    }
}