using System;
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

            // Check event dates and description content
            if (model.Topics.All(item => item == null))
            {
                ModelState.AddModelError(nameof(NewConferenceViewModel.Topics),
                    "задайте як мінімум одну тему");
                hasIncorrectData = true;
            }

            var dateNow = DateTime.Now;
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
                    "дату задано у невірному форматі");
                hasIncorrectData = true;
            }

            #endregion

            if (hasIncorrectData) return View(model);


            model.Events = model.EventDates
                .Where(t => t != null)
                .Select((t, i) => new ConferenceDate
                {
                    Id = Guid.NewGuid(),
                    Date = t.Value,
                    Description = model.EventDescriptions[i]
                }).ToList();


            _dataManager.Conferences.Create(model);
            _logger.LogInformation($"New conference {model.UniqueAddress} created");

            return RedirectToAction("index", "conference");
        }

        [Authorize(Roles = RoleNames.SEMPAI)]
        public IActionResult Edit()
        {
            // TODO: to do it... 


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
    }
}