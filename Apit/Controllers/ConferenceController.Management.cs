using System;
using System.Collections.Generic;
using System.Linq;
using Apit.Service;
using Apit.Utils;
using BusinessLayer.Models;
using DatabaseLayer;
using DatabaseLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Apit.Controllers
{
    public partial class ConferenceController
    {
        [Authorize(Roles = RoleNames.SEMPAI)]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, Authorize(Roles = RoleNames.SEMPAI)]
        public IActionResult Create(NewConferenceViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            // Combine all errors and return them back if this variable is set to true
            bool hasIncorrectData = false;

            #region ================ Long form review ================

            // Check unique address field value
            model.UniqueAddress?.NormalizeAddress();
            if (string.IsNullOrWhiteSpace(model.UniqueAddress))
                model.UniqueAddress = _dataManager.Conferences.GenerateUniqueAddress();
            else
            {
                string rangeMessage = $"{_config.UniqueAddress.MinSize}-{_config.UniqueAddress.MaxSize}";
                if (model.UniqueAddress.Length < _config.UniqueAddress.MinSize)
                {
                    ModelState.AddModelError(nameof(NewConferenceViewModel.UniqueAddress),
                        $"адреса закоротка (потрібно {rangeMessage} символів)");
                    hasIncorrectData = true;
                }
                else if (model.UniqueAddress.Length > _config.UniqueAddress.MaxSize)
                {
                    ModelState.AddModelError(nameof(NewConferenceViewModel.UniqueAddress),
                        $"адреса задовга (потрібно {rangeMessage} символів)");
                    hasIncorrectData = true;
                }
                else if (_dataManager.Articles.GetByUniqueAddress(model.UniqueAddress) != null)
                {
                    ModelState.AddModelError(nameof(NewConferenceViewModel.UniqueAddress),
                        "ця адреса вже використовується, оберіть іншу");
                    hasIncorrectData = true;
                }
            }

            // Check topics
            if (model.Topics.All(item => item == null))
            {
                ModelState.AddModelError(nameof(NewConferenceViewModel.Topics),
                    "задайте як мінімум одну тему");
                hasIncorrectData = true;
            }

            var dateNow = DateTime.Now;


            // Check event dates and description content
            // ^ => XOR (when a and b have difference values)
            if (model.EventDates.PairAny(model.EventDescriptions,
                (date, desc) => (date != null) ^ string.IsNullOrWhiteSpace(desc)))
            {
                ModelState.AddModelError(nameof(NewConferenceViewModel.Topics),
                    "задайте як мінімум одну дату");
                hasIncorrectData = true;
            }

            if (model.EventDates.Any(date => date < dateNow))
            {
                ModelState.AddModelError(nameof(NewConferenceViewModel.Topics),
                    "одна або декілька дат поза доступним діапазоном");
                hasIncorrectData = true;
            }

            #endregion

            if (hasIncorrectData) return View(model);


            model.Events = model.EventDates
                .Where((date, i) => date != null).Select((date, i) =>
                    new DateDescPair(date, model.EventDescriptions[i]))
                .Distinct(new DatesEqualityComparer())
                .Select(a => new ConferenceDate
                {
                    Id = Guid.NewGuid(),
                    Date = a.Date,
                    Description = a.Description,
                }).OrderBy(date => date.Date).ToList();


            _dataManager.Conferences.Create(model);
            _logger.LogInformation($"New conference {model.UniqueAddress} created");

            return RedirectToAction("index", "conference");
        }

        [Authorize(Roles = RoleNames.SEMPAI)]
        public IActionResult Edit()
        {
            // TODO: to do it... 
            
            ViewData["ErrorTitle"] = 501;
            ViewData["ErrorMessage"] = "У процесі реалізації";
            return View("error");
        }

        [Authorize(Roles = RoleNames.SEMPAI)]
        public IActionResult Delete()
        {
            var conference = _dataManager.Conferences.Current;
            if (conference.Participants.Any())
                return RedirectToAction("index", "conference");
            _dataManager.Conferences.Delete(conference.Id);
            return RedirectToAction("index", "account");
        }


        private class DateDescPair
        {
            public DateTime? Date { get; set; }
            public string Description { get; set; }

            public DateDescPair(DateTime? date, string description)
            {
                Date = date;
                Description = description;
            }
        }

        private class DatesEqualityComparer : EqualityComparer<DateDescPair>
        {
            public override bool Equals(DateDescPair x, DateDescPair y)
            {
                if (x == y) return true; // null == null
                if (x == null ^ y == null) return false;
                return x.Date == y.Date && x.Description == y.Description;
            }

            public override int GetHashCode(DateDescPair obj) =>
                obj.Date.GetHashCode() + obj.Description.GetHashCode();
        }
    }
}