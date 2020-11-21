using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DatabaseLayer.Enums;

namespace DatabaseLayer.Entities
{
    public class Article
    {
        public Guid Id { get; set; }
        [Required] public string UniqueAddress { get; set; }

        [Required] public Guid TopicId { get; set; }


        public ICollection<UserOwnArticlesLinking> Authors { get; set; }
        [Required] public string Title { get; set; }

        [Required] public string ShortDescription { get; set; }

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


        [NotMapped] public DisplayOptions Options { get; set; }


        [NotMapped]
        public class DisplayOptions
        {
            public Topic Topic { get; set; }
            public string PageAbsoluteUrl { get; set; }
            public string DocumentAbsoluteUrl { get; set; }
        }
    }
}