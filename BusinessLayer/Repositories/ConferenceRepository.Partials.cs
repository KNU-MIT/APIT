using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BusinessLayer.DataServices;
using BusinessLayer.Models;
using DatabaseLayer.Entities;
using Microsoft.AspNetCore.Http;

namespace BusinessLayer.Repositories
{
    public partial class ConferenceRepository
    {
        public IEnumerable<ConferenceParticipant> GetConfParticipants(Conference conference) =>
            _ctx.ConfParticipants.Where(a => a.Conference == conference);

        public IEnumerable<ArticleViewModel> GetConfArticles(Conference conference) =>
            _ctx.Articles.Where(a => a.Conference == conference).Select(a => _articles.GetById(a.Id));

        public IEnumerable<ConferenceImage> GetConfImages(Conference conference) =>
            _ctx.ConfImages.Where(a => a.Conference == conference);

        public IEnumerable<Topic> GetConfTopics(Conference conference) =>
            _ctx.Topics.Where(a => a.ConferenceId == conference.Id).OrderBy(a => a.Name);

        private IEnumerable<ConferenceDate> GetConfDates(Conference conference) =>
            _ctx.ConfDates.Where(a => a.Conference == conference).OrderBy(a => a.Date);


        public void AddArticle(Conference conference, Article article)
        {
            if (conference.Articles.Contains(article))
                Console.WriteLine($"Conference {conference.Id} already contains article {article.Id}");
            else conference.Articles.Add(article);
        }

        public void AddImage(Conference conference, IFormFile imageFile)
        {
            var extension = "." + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(_contentConfig.DataPath.ConferenceImagesDir, Guid.NewGuid() + extension);
            DataUtil.SaveFile(imageFile, filePath);

            var image = new ConferenceImage
            {
                Id = Guid.NewGuid(),
                ImagePath = filePath,
                Conference = conference,
            };

            conference.Images.Add(image);
            _ctx.ConfImages.Add(image);
        }

        public void AddDate(Conference conference, DateTime date, string description)
        {
            var newConfDate = new ConferenceDate
            {
                Id = Guid.NewGuid(),
                Date = date,
                Description = description,
                Conference = conference
            };

            conference.Dates.Add(newConfDate);
            _ctx.ConfDates.Add(newConfDate);
        }

        public void AddParticipant(Conference conference, ConferenceParticipant participant)
        {
            if (conference.Participants.Any(a => a.UserId == participant.UserId))
            {
                Console.WriteLine($"ERROR: Conference {conference.Id} already " +
                                  $"contains participant {participant.User.FullName}");
                return;
            }

            participant.Conference = conference;
            conference.Participants.Add(participant);
            participant.User.OwnParticipation.Add(participant);
            _ctx.ConfParticipants.Add(participant);
        }


        public void RemoveParticipant(Conference conference, User user)
        {
            var participant = _ctx.ConfParticipants.FirstOrDefault(a => a.UserId == user.Id);
            if (participant == null) return;

            conference.Participants.Remove(participant);
            _ctx.ConfParticipants.Remove(participant);
        }

        public void RemoveArticle(Conference conference, Article article)
        {
            conference.Articles.Add(article);
            _ctx.Articles.Add(article);
        }

        public void RemoveImage(Conference conference, string path)
        {
            var image = conference.Images.FirstOrDefault(a => a.ImagePath == path);
            if (image == null) throw new KeyNotFoundException(nameof(path));

            conference.Images.Add(image);
            _ctx.ConfImages.Add(image);
        }

        private void RemoveDate(Conference conf)
        {
            // TODO: Remove Conference Date Method in Repo
        }
    }
}