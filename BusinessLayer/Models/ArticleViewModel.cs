using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using BusinessLayer.DataServices;
using DatabaseLayer.ConfigModels;
using DatabaseLayer.Entities;
using DatabaseLayer.Enums;
using Microsoft.AspNetCore.Http;

namespace BusinessLayer.Models
{
    public class ArticleViewModel : NewArticleViewModel
    {
        public Guid Id { get; set; }

        public string UniqueAddress { get; set; }
        public string DocFileAddress { get; set; }

        public string HTMLContent { get; set; }

        public Topic Topic { get; set; }


        /// <summary>
        /// User with full access to the current article controls
        /// </summary>
        public User Creator { get; set; }

        /// <summary>
        /// Users without permissions but appear as authors
        /// </summary>
        public IEnumerable<User> AuthorUsers { get; set; }
        
        
        public IEnumerable<string> NonLinkedAuthors { get; set; }

        public ArticleStatus Status { get; set; }


        public IEnumerable<string> KeyWordsArray => KeyWords?.Split(';').Select(kw => kw.Trim());

        // override without Required attribute for use it to Update  
        public override IFormFile ArticleFile { get; set; }


        [DataType(DataType.DateTime)] public DateTime DateCreated { get; set; }
        [DataType(DataType.DateTime)] public DateTime DateLastModified { get; set; }

        
        public string GetFormatCreatingDate() => DateCreated.ToString("dd/MM/yyy");

        public string GetPlaceholderImagePath(ProjectConfig.DataPathConfig config)
        {
            string defaultDir = Path.Combine(config.ArticleImagesDir, UniqueAddress + Extension.Htm);
            const string altDir = "../Apit/wwwroot/img/articles_placeholder";

            var rand = new Random();
            var images = Directory.Exists(defaultDir)
                ? Directory.GetFiles(defaultDir)
                : Directory.GetFiles(altDir);
            if (images.Length == 0) Directory.GetFiles(altDir);

            return images[rand.Next(0, images.Length - 1)].Split("/wwwroot/").Last();
        }
    }
}