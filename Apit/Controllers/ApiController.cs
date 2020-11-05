using System.Threading.Tasks;
using Apit.Areas.Admin.Models;
using BusinessLayer;
using BusinessLayer.DataServices;
using DatabaseLayer;
using DatabaseLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Apit.Controllers
{
    public class ApiController : Controller
    {
        private readonly ILogger<ConferenceController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly DataManager _dataManager;

        public ApiController(ILogger<ConferenceController> logger, 
            UserManager<User> userManager, DataManager dataManager)
        {
            _logger = logger;
            _userManager = userManager;
            _dataManager = dataManager;
        }

        [HttpGet, Route("try-get-user")]
        public async Task<JsonResult> TryGetUser(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Json(new SetPermissionsResponse
                {
                    Status = 400, // bad request
                    User = null
                });

            var user = await _userManager.GetUserAsync(User);

            if (!await _userManager.IsInRoleAsync(user, RoleNames.ADMIN))
                return Json(new SetPermissionsResponse
                {
                    Status = 403, // forbidden
                    User = null
                });

            User resultUser = null;
            // value could be an unique profile address
            if (value.Length == UniqueAddressSizes.USERS)
                resultUser = _dataManager.Users.GetByUniqueAddress(value);

            if (resultUser == null)
                return Json(new SetPermissionsResponse
                {
                    Status = 404, // not found
                    User = null
                });
            
            return Json(new SetPermissionsResponse
            {
                Status = 200, // ok
                User = new SetPermissionsResponse.UserData
                {
                    ProfileAddress = resultUser.ProfileAddress,
                    FullName = resultUser.FullName
                }
            });
        }
    }
}