using System;
using System.Collections.Generic;
using BusinessLayer.Models;
using DatabaseLayer.Entities;

namespace BusinessLayer.Interfaces
{
    public interface IArticlesRepository :
        ICollectedData<Guid, ArticleViewModel, Article>,
        IAddressedData<ArticleViewModel>
    {
        IEnumerable<ArticleViewModel> GetByKeyWord(string word);
        IEnumerable<ArticleViewModel> GetByAuthor(string userId);

        Article GetByUniqueAddressAsDbObject(string address);


        void DeleteLinkedUser(IEnumerable<UserOwnArticlesLinking> linking);
        IEnumerable<Article> GetByConference(Conference conf);
    }
}