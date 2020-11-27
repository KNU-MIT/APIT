using System;
using System.Collections.Generic;
using BusinessLayer.Models;
using DatabaseLayer.Entities;
using Microsoft.AspNetCore.Http;

namespace BusinessLayer.Interfaces
{
    public interface IConferencesRepository :
        ICollectedData<Guid, ConferenceViewModel, NewConferenceViewModel>,
        IAddressedData<ConferenceViewModel>
    {
        ConferenceViewModel Current { get; }

        Conference GetCurrentAsDbModel();
        void Update(ConferenceViewModel model);

        IEnumerable<ConferenceParticipant> GetConfParticipants(Conference conference);
        IEnumerable<ArticleViewModel> GetConfArticles(Conference conference);
        IEnumerable<ConferenceImage> GetConfImages(Conference conference);

        void AddParticipant(Conference conference, ConferenceParticipant participant);
        void AddArticle(Conference conference, Article article);
        void AddImage(Conference conference, IFormFile image);

        void RemoveParticipant(Conference conference, User user);
        void RemoveArticle(Conference conference, Article article);
        void RemoveImage(Conference conference, string path);

        // TODO: костыли ...
        void Delete(Conference entity);
    }
}