using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DatabaseLayer.Enums;

namespace DatabaseLayer.Entities
{
    public class Article
    {
        public Guid Id { get; set; }
        [Required] public string UniqueAddress { get; set; }

        [Required] public Topic Topic { get; set; }
        
        public ICollection<UserOwnArticlesLinking> Authors { get; set; }
        [Required] public string Title { get; set; }


        public ArticleStatus Status { get; set; }
        public string KeyWords { get; set; }


        [Required] public string HtmlFilePath { get; set; }
        [Required] public string DocxFilePath { get; set; }

        public Conference Conference { get; set; }


        [DataType(DataType.DateTime)] public DateTime DateCreated { get; set; }
        [DataType(DataType.DateTime)] public DateTime DateLastModified { get; set; }


        public Article()
        {
            Authors = new HashSet<UserOwnArticlesLinking>();
        }
    }
}