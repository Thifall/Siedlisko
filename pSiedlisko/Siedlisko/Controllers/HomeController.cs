using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Siedlisko.ViewModels;
using Siedlisko.Models;
using Microsoft.Extensions.Configuration;
using Siedlisko.Models.Interfaces;

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
