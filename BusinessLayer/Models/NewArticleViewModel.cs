using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BusinessLayer.DataServices;
using DatabaseLayer.ConfigModels;
using DatabaseLayer.Entities;
using Microsoft.AspNetCore.Http;

namespace BusinessLayer.Models
{
    // Why don't we use the ArticleViewModel for represent the new instance of the Article?
    // ANSWER: because there are a lot of unnecessary redundant fields

    // TODO: It is better to use the Localization and the Resource files.

    public class NewArticleViewModel
    {
        private static readonly Regex keyWordsAvailableRegex =
            new Regex(@"^[а-яА-Яa-zA-Z0-9- ,;`'іїєґ!]+$", RegexOptions.Compiled);

        private static readonly Regex keyWordsSeparatorRegex =
            new Regex(@"[,;]+", RegexOptions.Compiled);


        [Required(ErrorMessage = "Прикрепіть файл з матеріалом")] // Display(Name = "Файл із матеріалами")
        public virtual IFormFile ArticleFile { get; set; }

        [Required(ErrorMessage = "Оберіть тему Вашої роботи"), Display(Name = "Тематика")]
        public string TopicId { get; set; }


        private string _keyWords;

        [Required(ErrorMessage = "Вкажіть (через «,» або «;») ключові слова"), Display(Name = "Ключові слова")]
        public string KeyWords
        {
            get => _keyWords;
            set
            {
                // "hello, wor/*+#ld1!" != "hello, world1!"
                KeyWordsAreCorrect = value != null && string.IsNullOrWhiteSpace(
                    keyWordsAvailableRegex.Replace(value, ""));

                if (KeyWordsAreCorrect)
                    _keyWords = string.Join("; ", keyWordsSeparatorRegex
                        .Replace(value, ";").Split(';')
                        .Select(s => s.Trim()).Distinct().ToArray());
                else _keyWords = "";
            }
        }

        [Required(ErrorMessage = "Матеріал повинен мати заголовок"), Display(Name = "Заголовок")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Введіть короткий опис"), Display(Name = "Короткий опис")]
        public string ShortDescription { get; set; }

        public string[] Authors { get; set; }


        public bool KeyWordsAreCorrect { get; private set; }


        public async Task<(string extension, string uniqueAddress, string errorMessage)>
            GetUploadedFile(DataManager dataManager, ProjectConfig.DataPathConfig dataPathConfig)
        {
            string extension = "", uniqueAddress = "", errorMessage = null;
            if (ArticleFile != null && ArticleFile.Length > 0)
            {
                extension = Path.GetExtension(ArticleFile.FileName);
                uniqueAddress = dataManager.Articles.GenerateUniqueAddress();

                if (Regex.IsMatch(extension ?? "", @"\.docx?$"))
                {
                    string err = await DataUtil.TrySaveDocFile(ArticleFile,
                        uniqueAddress, extension, dataPathConfig);

                    if (!string.IsNullOrEmpty(err))
                    {
                        errorMessage =
                            "даний файл не може бути збереженим, оскільки може нести у собі загрозу для " +
                            "сервісу. Якщо це не так, будь ласка, зверніться до адміністрації сайту\n" + err;
                    }
                }
                else errorMessage = "невірний формат файлу (доступно лише .doc і .docx)";
            }
            else errorMessage = "будь ласка, прикрепіть файл з матеріалом";

            return (extension, uniqueAddress, errorMessage);
        }


        public IList<UserOwnArticlesLinking> GenerateAuthorsLinking(DataManager dataManager,
            ProjectConfig.UniqueAddressConfig addressConfig, User user, Article article)
        {
            var authors = new List<UserOwnArticlesLinking>();

            foreach (string author in Authors)
            {
                if (string.IsNullOrEmpty(author)) continue;
                // is author already exist in dest cellection
                if (authors.Any(a => a.NameString == author || a.UserId == author)) continue;

                if (author.Length == addressConfig.UserAddressSize)
                {
                    var addressUser = dataManager.Users.GetByUniqueAddress(author);
                    if (addressUser != null)
                    {
                        authors.Add(new UserOwnArticlesLinking
                        {
                            Id = Guid.NewGuid(),
                            IsCreator = false,
                            IsRegisteredUser = true,
                            NameString = addressUser.FullName,
                            UserId = addressUser.Id,
                            ArticleId = article.Id
                        });
                        continue;
                    }
                }

                authors.Add(new UserOwnArticlesLinking
                {
                    Id = Guid.NewGuid(),
                    IsCreator = false,
                    IsRegisteredUser = false,
                    NameString = author,
                    UserId = user.Id,
                    ArticleId = article.Id
                });
            }

            return authors;
        }
    }
}