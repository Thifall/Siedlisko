using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Siedlisko.ViewModels;
using Microsoft.Extensions.Configuration;
using DbAcces.Repositories.Interfaces;

namespace Siedlisko.Controllers
{
    public class HomeController : Controller
    {
        #region Fields and Properties
        private IConfigurationRoot _config;
        private IRepository _repository;
        #endregion

        #region .ctor
        public HomeController(IConfigurationRoot config, IRepository repo)
        {
            _config = config;
            _repository = repo;
        }
        #endregion

        #region Actions
        public IActionResult Index(IndexViewModel vm)
        {
            vm.PrepareDaysStatuses(_repository.GetAllReservations().Where(x => x.StartDate.Month >= vm.Date.AddMonths(-1).Month && x.EndDate.Month <= vm.Date.AddMonths(1).Month));
            return View(vm);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        } 
        #endregion
    }
}
