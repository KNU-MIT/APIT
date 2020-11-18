using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLayer.DataServices;
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

        public ConferenceRepository(AppDbContext context,
            IUsersRepository usersRepo, IArticlesRepository articlesRepo)
        {
            _ctx = context;
            _users = usersRepo;
            _articles = articlesRepo;
        }

        public string GenerateUniqueAddress() => DataUtil.GenerateUniqueAddress(this, 8);


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

            Console.WriteLine("Topics: " + topics.Count);

            var newConference = new Conference()
            {
                Id = Guid.NewGuid(),
                UniqueAddress = entity.UniqueAddress,
                IsActual = true,
                Title = entity.Title,

                ShortDescription = entity.ShortDescription,
                Description = entity.Description,

                Topics = topics,

                DateCreated = dateNow,
                DateLastModified = dateNow,
                DateStart = entity.DateStart,
                DateFinish = entity.DateFinish
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
            // TODO ...
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

                DateCreated = conf.DateCreated,
                DateLastModified = conf.DateLastModified,
                DateStart = conf.DateStart,
                DateFinish = conf.DateFinish
            };
        }

        private IEnumerable<ConferenceViewModel> ConvertToViewModel
            (IEnumerable<Conference> conf) => conf.Select(ConvertToViewModel);
    }
}