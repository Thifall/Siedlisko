﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Siedlisko.Controllers.Interfaces;
using Siedlisko.Models;
using Siedlisko.Models.Interfaces;
using Siedlisko.ViewModels;
using Siedlisko.ViewModels.Interfaces;
using SiedliskoCommon.Models;
using SiedliskoCommon.Models.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Siedlisko.Reservations;

namespace Siedlisko.Controllers
{
    public class ReservationController : Controller
    {
        #region Fields and Properties
        private IConfigurationRoot _config;
        private readonly Reserver _reserver;
        private UserManager<SiedliskoUser> _userManager;
        private IRepository _repository;
        #endregion

        #region ctor

        public ReservationController(UserManager<SiedliskoUser> userManager,
            IRepository repository,
            IConfigurationRoot configuration,
            Reserver reserver)
        {
            _config = configuration;
            _reserver = reserver;
            _userManager = userManager;
            _repository = repository;
        }
        #endregion

        #region Actions
        //wyświetlenie danych rezerwacji
        [Authorize]
        public IActionResult Index()
        {
            ReservationIndexViewModel reservationViewModel;
            if (User.IsInRole("User"))
            {
                reservationViewModel = new ReservationIndexViewModel(_repository
                                                                        .GetUsersReservations(User.Identity.Name)
                                                                        .OrderBy(x => x.StartDate)
                                                                        .ThenBy(x => x.EndDate), _repository);
            }
            else
            {
                reservationViewModel = new ReservationIndexViewModel(_repository.GetAllReservations()
                                                                        .OrderBy(x => x.StartDate)
                                                                        .ThenBy(x => x.EndDate), _repository);
            }
            _reserver.RetriveLastOperationResults(reservationViewModel);
            return View(reservationViewModel);
        }

        //edycja rezerwacji (w zasadzie jedynie kontakt jest możliwy do edycji admina)
        [Authorize]
        public IActionResult Edit(int Id)
        {
            if (User.IsInRole("User"))
            {
                return RedirectToAction("Index");
            }

            var reservation = _repository.GetReservationById(Id);
            if (reservation != null)
            {
                var editViewModel = new EditViewModel() { Reservation = reservation };
                _reserver.RetriveLastOperationResults(editViewModel);
                return View(editViewModel);
            }
            return RedirectToAction("Index");
        }

        //potwierdzenie i powrót do widoku edycji
        [Authorize]
        public async Task<IActionResult> Confirm(int Id)
        {
            await _reserver.ConfirmReservation(Id);
            if (_reserver.LastOperationResult)
            {
                return RedirectToAction("Edit", "Reservation", new { Id });
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateReservationViewModel createVM)
        {
            if (!ModelState.IsValid)
            {
                return View(createVM);
            }

            await _reserver.TryMakeReservation(createVM, await _userManager.FindByNameAsync(User.Identity.Name));
            _reserver.RetriveLastOperationResults(createVM);

            return View(createVM);
        }

        [Authorize]
        public async Task<IActionResult> CancelReservation(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var reservation = _repository.GetReservationById(id);
                if (User.IsInRole("User"))
                {
                    if (reservation.ReserverUserName == User.Identity.Name)
                    {
                        await _reserver.RemoveReservation(reservation);
                    }
                }
                else if (User.IsInRole("Dev") || User.IsInRole("Admin"))
                {
                    await _reserver.RemoveReservation(reservation);
                }
            }
            return RedirectToAction("Index", "Reservation");
        }
        #endregion
    }
}
