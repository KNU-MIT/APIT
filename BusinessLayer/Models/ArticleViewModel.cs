using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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


        public string Title { get; set; }
        public ArticleStatus Status { get; set; }
        public IEnumerable<string> KeyWords { get; set; }


        [DataType(DataType.DateTime)] public DateTime DateCreated { get; set; }
        [DataType(DataType.DateTime)] public DateTime DateLastModified { get; set; }


        public bool IsAuthor(User user) => Authors.Any(a => a == user);

        public string GetFormatCreatingDate() => DateCreated.ToString("dd/MM/yyy");
    }
}