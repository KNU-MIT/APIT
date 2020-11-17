using System;

namespace DatabaseLayer.Entities
{
    public class UserOwnArticlesLinking
    {
        public Guid Id { get; set; }
        public bool IsCreator { get; set; }

        public string NameString { get; set; }
        
        public string UserId { get; set; }
        public User User { get; set; }

        public Guid ArticleId { get; set; }
        public Article Article { get; set; }
    }
}