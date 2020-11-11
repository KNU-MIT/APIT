using System;
using System.Linq;
using Apit.Service;
using BusinessLayer.Models;
using DatabaseLayer;
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
            var dateNow = DateTime.Now;
            var model = new NewConferenceViewModel
            {
                UniqueAddress = _dataManager.Conferences.GenerateUniqueAddress(),
                DateStart = dateNow,
                DateFinish = dateNow
            };

            return View(model);
        }

        [HttpPost, Authorize(Roles = RoleNames.SEMPAI)]
        public IActionResult Create(NewConferenceViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            // Combine all errors and return them back if this variable is set to true
            bool hasIncorrectData = false;

            #region ================ Long form review ================

            // Check unique address field value
            model.UniqueAddress.NormalizeAddress();
            if (model.UniqueAddress.Length < 5)
            {
                ModelState.AddModelError(nameof(NewConferenceViewModel.UniqueAddress),
                    "адреса закоротка (потрібна довжина 5-25 символів)");
                hasIncorrectData = true;
            }
            else if (model.UniqueAddress.Length > 25)
            {
                ModelState.AddModelError(nameof(NewConferenceViewModel.UniqueAddress),
                    "адреса задовга (потрібна довжина 5-20 символів)");
                hasIncorrectData = true;
            }
            else if (_dataManager.Articles.GetByUniqueAddress(model.UniqueAddress) != null)
            {
                ModelState.AddModelError(nameof(NewConferenceViewModel.UniqueAddress),
                    "ця адреса вже використовується, оберіть іншу");
                hasIncorrectData = true;
            }

            // apply topics
            if (model.Topics.All(item => item == null))
            {
                ModelState.AddModelError(nameof(NewConferenceViewModel.Topics),
                    "задайте як мінімум одну тему");
                hasIncorrectData = true;
            }

            // disable past dates for DateStart
            if (model.DateStart < DateTime.Today)
            {
                ModelState.AddModelError(nameof(NewConferenceViewModel.DateStart),
                    "невірно задано дату");
                hasIncorrectData = true;
            }

            // disable past dates for DateFinish
            if (model.DateFinish < DateTime.Today)
            {
                ModelState.AddModelError(nameof(NewConferenceViewModel.DateFinish),
                    "невірно задано дату");
                hasIncorrectData = true;
            }

            // end date is always later than start date
            if (model.DateStart > model.DateFinish)
            {
                ModelState.AddModelError(nameof(NewConferenceViewModel.DateFinish),
                    "невірно задано дати");
                hasIncorrectData = true;
            }

            #endregion

            if (hasIncorrectData) return View(model);

            _dataManager.Conferences.Create(model);
            _logger.LogInformation($"New conference {model.UniqueAddress} created");

            return RedirectToAction("index", "conference");
        }

        [Authorize(Roles = RoleNames.SEMPAI)]
        public IActionResult Edit()
        {
            
            
            
            // TODO: to do it... 
            // TODO: to do it... 
            // TODO: to do it... 
            // TODO: to do it... 
            // TODO: to do it... 
            // TODO: to do it...
            // TODO: to do it... 
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