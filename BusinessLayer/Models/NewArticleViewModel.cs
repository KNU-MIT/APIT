using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace BusinessLayer.Models
{
    // Why don't we use the ArticleViewModel for represent the new instance of the Article?
    // ANSWER: because there are a lot of unnecessary redundant fields

    // TODO: It is better to use the Localization and the Resource files.
    
    public class NewArticleViewModel
    {
        [Required, Display(Name = "Файл із матеріалами")] 
        public IFormFile DocFile { get; set; }

        [Required, Display(Name = "Тематика")]
        public string TopicId { get; set; }

        [Required, Display(Name = "Ключові слова")]
        public string KeyWords { get; set; }

        [Required, Display(Name = "Заголовок")]
        public string Title { get; set; }
        
        [Required, Display(Name = "Короткий опис")]
        public string ShortDescription { get; set; }
    }
}