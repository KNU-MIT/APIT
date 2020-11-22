using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.DataServices;
using DatabaseLayer.ConfigModels;
using BusinessLayer.Interfaces;
using BusinessLayer.Models;
using DatabaseLayer;
using DatabaseLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Repositories
{
    public class ArticlesRepository : IArticlesRepository
    {
        private readonly AppDbContext _ctx;
        private readonly ProjectConfig.ContentConfig _contentConfig;

        public ArticlesRepository(AppDbContext context, ProjectConfig config)
        {
            _ctx = context;
            _contentConfig = config.Content;
        }

        public string GenerateUniqueAddress() => DataUtil.GenerateUniqueAddress(this, 8);

        public IEnumerable<ArticleViewModel> GetAll() => ConvertToViewModel(_ctx.Articles);

        public ArticleViewModel GetById(Guid id) =>
            ConvertToViewModel(_ctx.Articles.FirstOrDefault(a => a.Id == id)).Result;

        public bool IsExist(Guid id) => _ctx.Articles.Any(a => a.Id == id);

        public ArticleViewModel GetByUniqueAddress(string address) =>
            ConvertToViewModel(GetByUniqueAddressAsDbObject(address)).Result;

        public Article GetByUniqueAddressAsDbObject(string address) =>
            _ctx.Articles.FirstOrDefault(a => a.UniqueAddress == address);

        public IEnumerable<ArticleViewModel> GetByKeyWord(string word)
        {
            bool SelectorFunction(Article a)
            {
                var keys = a.KeyWords.Split(' ');
                return keys.Any(kw => kw == word);
            }

            return ConvertToViewModel(_ctx.Articles.Where(SelectorFunction));
        }


        public IEnumerable<ArticleViewModel> GetLatest(ushort count) =>
            ConvertToViewModel(_ctx.Articles.OrderByDescending(a => a.DateCreated).Take(count));

        public IEnumerable<ArticleViewModel> GetByAuthor(string userId) =>
            ConvertToViewModel(_ctx.Articles.Where(a => a.Authors.Any(b => b.UserId == userId)));

        public IEnumerable<Article> GetByConference(Conference conf) =>
            _ctx.Articles.Where(a => a.Conference == conf);

        public void SaveChanges() => _ctx.SaveChanges();


        public void Create(Article entity)
        {
            if (entity == null) throw new ArgumentNullException();

            _ctx.Articles.Add(entity);

            SaveChanges();
        }

        public void Update(Article entity)
        {
            foreach (var author in entity.Authors)
                _ctx.UserArticles.Update(author);

            _ctx.Entry(entity).State = EntityState.Modified;
            _ctx.SaveChanges();
        }


        public void Delete(Article entity)
        {
            _ctx.Articles.Remove(entity);
            var linkingUsers = GetLinkedUsers(entity.Id);
            _ctx.UserArticles.RemoveRange(linkingUsers);
            SaveChanges();
        }


        public void CreateLinkedUsers(IEnumerable<UserOwnArticlesLinking> linking)
        {
            _ctx.UserArticles.AddRange(linking);
            SaveChanges();
        }

        public IEnumerable<UserOwnArticlesLinking> GetLinkedUsers(Guid articleId) =>
            _ctx.UserArticles.Where(a => a.ArticleId == articleId);

        public void DeleteLinkedUsers(IEnumerable<UserOwnArticlesLinking> linking)
        {
            _ctx.UserArticles.RemoveRange(linking);
            SaveChanges();
        }


        // It will be more convenient to do this using specific constructor
        private async Task<ArticleViewModel> ConvertToViewModel(Article article)
        {
            if (article == null) return null;


            var authors = _ctx.UserArticles.Where(a => a.ArticleId == article.Id).ToArray();
            string creatorId = authors.FirstOrDefault(a => a.IsCreator)?.UserId;

            return new ArticleViewModel
            {
                Id = article.Id,
                UniqueAddress = article.UniqueAddress,
                Topic = _ctx.Topics.FirstOrDefault(t => t.Id == article.TopicId),

                Creator = _ctx.Users.FirstOrDefault(u => u.Id == creatorId),
                Authors = authors.Select(a => a.NameString).ToArray(),
                AuthorUsers = authors.Where(a => a.IsCreator).Select(a =>
                    _ctx.Users.FirstOrDefault(u => u.Id == a.UserId)),
                NonLinkedAuthors = authors.Where(a =>
                    !string.IsNullOrEmpty(a.NameString)).Select(a => a.NameString),

                DocFileAddress = article.DocxFilePath,
                HTMLContent = await DataUtil.LoadHtmlFile(article.HtmlFilePath,
                    _contentConfig.DataPath) ?? "Такої статті не існує",

                Title = article.Title,
                ShortDescription = article.ShortDescription,
                Status = article.Status,
                KeyWords = article.KeyWords,

                DateCreated = article.DateCreated,
                DateLastModified = article.DateLastModified
            };
        }

        private IEnumerable<ArticleViewModel> ConvertToViewModel(IEnumerable<Article> articles) =>
            articles.Select(article => ConvertToViewModel(article).Result);
    }
}