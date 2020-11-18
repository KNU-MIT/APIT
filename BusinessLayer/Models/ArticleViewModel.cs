using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using BusinessLayer.DataServices;
using DatabaseLayer.Entities;
using DatabaseLayer.Enums;
using Microsoft.AspNetCore.Http;

namespace BusinessLayer.Models
{
    public class ArticleViewModel
    {
        public Guid Id { get; set; }

        public string UniqueAddress { get; set; }
        public string DocFileAddress { get; set; }

        public string HTMLContent { get; set; }

        public Topic Topic { get; set; }


        public string NewTopicId { get; set; }
        public IFormFile NewDocFile { get; set; }


        public string ShortDescription { get; set; }


        /// <summary>
        /// User with full access to the current article controls
        /// </summary>
        public User Creator { get; set; }

        /// <summary>
        /// Users without permissions but appear as authors
        /// </summary>
        public IEnumerable<User> Authors { get; set; }

        public IEnumerable<string> NonLinkedAuthors { get; set; }


        public string Title { get; set; }
        public ArticleStatus Status { get; set; }
        public IEnumerable<string> KeyWords { get; set; }


        [DataType(DataType.DateTime)] public DateTime DateCreated { get; set; }
        [DataType(DataType.DateTime)] public DateTime DateLastModified { get; set; }


        public bool IsAuthor(User user) => Authors.Any(a => a == user);

        public string GetFormatCreatingDate() => DateCreated.ToString("dd/MM/yyy");

        public string GetPlaceholderImagePath()
        {
            string defaultDir = Path.Combine(DataUtil.DEST_IMG_DIR, UniqueAddress + Extension.Htm);
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