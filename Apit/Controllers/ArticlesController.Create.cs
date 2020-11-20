using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BusinessLayer.DataServices;
using BusinessLayer.Models;
using DatabaseLayer.Entities;
using DatabaseLayer.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
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
            if (keyWordsAvailableRegex.Replace(model.KeyWords, "") != "")
            {
                ModelState.AddModelError(nameof(model.KeyWords),
                    "У ключових словах Ви можете використовувати лише " +
                    "цифри, літери українського та англійського алфавітів");
                hasIncorrectData = true;
            }

            // Check uploaded file
            string extension = default, uniqueAddress = default;
            if (model.ArticleFile != null && model.ArticleFile.Length > 0)
            {
                extension = Path.GetExtension(model.ArticleFile.FileName);
                uniqueAddress = _dataManager.Articles.GenerateUniqueAddress();
                _logger.LogInformation("Upload file with extension: " + extension);

                if (!Regex.IsMatch(extension ?? "", @"\.docx?$"))
                {
                    ModelState.AddModelError(nameof(model.ArticleFile),
                        "невірний формат файлу (доступно лише .doc і .docx)");
                    hasIncorrectData = true;
                }
                else if (!hasIncorrectData)
                {
                    string err = await DataUtil.TrySaveDocFile(model.ArticleFile,
                        uniqueAddress, extension, _config.Content.DataPath);
                    if (err != null)
                    {
                        _logger.LogError("Document converter error\n" + err);
                        ModelState.AddModelError(nameof(model.ArticleFile),
                            "даний файл не може бути збереженим, оскільки може нести у собі загрозу для сервісу. " +
                            "Якщо це не так, будь ласка, зверніться до адміністрації сайту");
                        hasIncorrectData = true;
                    }
                }
            }
            else
            {
                ModelState.AddModelError(nameof(model.ArticleFile),
                    "будь ласка, прикрепіть файл з матеріалом");
                hasIncorrectData = true;
            }

            #endregion

            if (hasIncorrectData) return View(model);


            model.KeyWords = string.Join(";", keyWordsSeparatorRegex
                .Replace(model.KeyWords, ";")
                .Split(';').Select(s => s.Trim()).Distinct().ToArray());

            var dateNow = DateTime.Now;
            var user = await _userManager.GetUserAsync(User);

            var authors = new List<UserOwnArticlesLinking>
            {
                new UserOwnArticlesLinking
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    IsCreator = true
                }
            };

            foreach (string author in model.Authors)
            {
                if (authors.Any(a => a.NameString == author || a.UserId == author)) continue;

                if (author.Length == _config.Content.UniqueAddress.UserAddressSize)
                {
                    var addressUser = _dataManager.Users.GetByUniqueAddress(author);
                    if (addressUser != null)
                    {
                        authors.Add(new UserOwnArticlesLinking
                        {
                            Id = Guid.NewGuid(),
                            UserId = addressUser.Id
                        });
                        continue;
                    }
                }

                authors.Add(new UserOwnArticlesLinking
                {
                    Id = Guid.NewGuid(),
                    NameString = author,
                    UserId = user.Id
                });
            }

            var article = new Article
            {
                Id = Guid.NewGuid(),
                TopicId = topic.Id,
                Authors = authors,
                UniqueAddress = uniqueAddress,

                Title = model.Title,
                ShortDescription = model.ShortDescription,
                Status = ArticleStatus.Uploaded,
                KeyWords = model.KeyWords,

                HtmlFilePath = uniqueAddress + ".htm",
                DocxFilePath = uniqueAddress + extension,

                Conference = _dataManager.Conferences.GetCurrentAsDbModel(),

                DateCreated = dateNow,
                DateLastModified = dateNow
            };

            foreach (var author in article.Authors)
                author.ArticleId = article.Id;

            var currentConf = _dataManager.Conferences.GetCurrentAsDbModel();
            _dataManager.Conferences.AddArticle(currentConf, article);
            _dataManager.Articles.Create(article);

            return RedirectToAction("index", "articles", new {x = uniqueAddress});
        }
    }
}