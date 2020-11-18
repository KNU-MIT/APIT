using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLayer.DataServices;
using BusinessLayer.DataServices.ConfigModels;
using BusinessLayer.Interfaces;
using BusinessLayer.Models;
using DatabaseLayer;
using DatabaseLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Repositories
{
    public partial class ConferenceRepository : IConferencesRepository
    {
        private readonly AppDbContext _ctx;
        private readonly IUsersRepository _users;
        private readonly IArticlesRepository _articles;
        private readonly ProjectConfig.UniqueAddressConfig _addressConfig;

        public ConferenceRepository(AppDbContext context,
            IUsersRepository usersRepo, IArticlesRepository articlesRepo, ProjectConfig config)
        {
            _ctx = context;
            _users = usersRepo;
            _articles = articlesRepo;
            _addressConfig = config.Content.UniqueAddress;
        }

        public string GenerateUniqueAddress() =>
            DataUtil.GenerateUniqueAddress(this, _addressConfig.ArticleAddressSize);


        public ConferenceViewModel Current => ConvertToViewModel(GetCurrentAsDbModel());
        public Conference GetCurrentAsDbModel() => _ctx.Conferences.FirstOrDefault(a => a.IsActual);


        public ConferenceViewModel GetById(Guid id) =>
            ConvertToViewModel(_ctx.Conferences.FirstOrDefault(conf => conf.Id == id));

        public IEnumerable<ConferenceViewModel> GetAll() => ConvertToViewModel(_ctx.Conferences);

        public void SaveChanges() => _ctx.SaveChanges();

        public bool IsExist(Guid id) => _ctx.Conferences.Any(conf => conf.Id == id);


        public ConferenceViewModel GetByUniqueAddress(string address) =>
            ConvertToViewModel(_ctx.Conferences.FirstOrDefault(a => a.UniqueAddress == address));

        public IEnumerable<ConferenceViewModel> GetLatest(ushort count) =>
            ConvertToViewModel(_ctx.Conferences.OrderByDescending(a => a.DateCreated).Take(count));


        public void Create(NewConferenceViewModel entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var current = _ctx.Conferences.FirstOrDefault(a => a.IsActual);
            if (current != null) current.IsActual = false;
            else current = null;

            var dateNow = DateTime.Now;

            var topics = entity.Topics
                .Where(topic => !string.IsNullOrWhiteSpace(topic))
                .Select(topic => new Topic
                {
                    Id = Guid.NewGuid(),
                    Name = topic,
                }).ToList();
            
            var newConference = new Conference()
            {
                Id = Guid.NewGuid(),
                UniqueAddress = entity.UniqueAddress,
                IsActual = true,
                Title = entity.Title,

                ShortDescription = entity.ShortDescription,
                Description = entity.Description,
                Topics = topics,
                Dates = entity.Events.OrderBy(a => a.Date).ToArray(),

                DateCreated = dateNow,
                DateLastModified = dateNow
            };

            foreach (var topic in newConference.Topics)
            {
                topic.Conference = newConference;
                _ctx.Topics.Add(topic);
            }

            _ctx.Conferences.Add(newConference);
            SaveChanges();
        }

        public void Update(ConferenceViewModel model)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            var instance = _ctx.Conferences.FirstOrDefault(conf => conf.Id == id);
            if (instance == null) return;

            foreach (var topic in _ctx.Topics.Where(a => a.Conference == instance))
                _ctx.Entry(topic).State = EntityState.Deleted;
            _ctx.Entry(instance).State = EntityState.Deleted;
            SaveChanges();
        }


        private ConferenceViewModel ConvertToViewModel(Conference conf)
        {
            if (conf == null) return null;

            var dates = GetConfDates(conf).ToArray();

            return new ConferenceViewModel
            {
                IsActual = conf.IsActual,

                Id = conf.Id,
                Title = conf.Title,
                ShortDescription = conf.ShortDescription,
                Description = conf.Description,

                Topics = GetConfTopics(conf),
                Participants = GetConfParticipants(conf),
                Articles = GetConfArticles(conf),
                Images = GetConfImages(conf),
                Dates = dates,

                DateStart = dates.Min(d => d.Date).GetValueOrDefault(DateTime.Now),
                DateFinish = dates.Max(d => d.Date).GetValueOrDefault(DateTime.Now),

                DateCreated = conf.DateCreated,
                DateLastModified = conf.DateLastModified,
            };
        }

        private IEnumerable<ConferenceViewModel> ConvertToViewModel
            (IEnumerable<Conference> conf) => conf.Select(ConvertToViewModel);
    }
}