using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BusinessLayer.Models;
using BusinessLayer;
using DatabaseLayer.ConfigModels;
using DatabaseLayer;
using DatabaseLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Apit.Controllers
{
    public partial class ArticlesController : Controller
    {
        private readonly ILogger<ArticlesController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly DataManager _dataManager;
        private readonly ProjectConfig _config;

        private static readonly Regex keyWordsAvailableRegex =
            new Regex(@"^[а-яА-Яa-zA-Z0-9- ,;'іїєґ!]+$", RegexOptions.Compiled);

        private static readonly Regex keyWordsSeparatorRegex =
            new Regex(@"[,;]+", RegexOptions.Compiled);

        public ArticlesController(ILogger<ArticlesController> logger,
            UserManager<User> userManager, DataManager dataManager, ProjectConfig config)
        {
            _logger = logger;
            _userManager = userManager;
            _dataManager = dataManager;
            _config = config;
        }


        [Authorize]
        public async Task<IActionResult> Index(string x)
        {
            var user = await _userManager.GetUserAsync(User);
            var article = _dataManager.Articles.GetByUniqueAddress(x);

            if (user == article?.Creator || await _userManager.IsInRoleAsync(user, RoleNames.MANAGER))
            {
                if (string.IsNullOrWhiteSpace(x)) Error();
                return article == null ? Error() : View(article);
            }

            ViewData["ErrorTitle"] = 403;
            ViewData["ErrorMessage"] = "Лоступ заблоковано :(";
            return View("error");
        }

        [Authorize(Roles = RoleNames.MANAGER)]
        public IActionResult List(ArticlesListViewModel model)
        {
            switch (model.Filter)
            {
                case "all":
                {
                    model.Collection = _dataManager.Articles.GetAll()
                        .OrderBy(a => a.DateLastModified).Reverse();
                    break;
                }
                default:
                {
                    model.Filter = "current";
                    model.Collection = _dataManager.Conferences.Current.Articles
                        .OrderBy(a => a.DateLastModified).Reverse();
                    break;
                }
            }

            return View(model);
        }

        public IActionResult Requirements()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}