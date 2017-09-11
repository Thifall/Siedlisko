using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Siedlisko.Controllers.Interfaces;
using Siedlisko.Models;
using Siedlisko.Models.Interfaces;
using Siedlisko.ViewModels;
using Siedlisko.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siedlisko.Controllers
{
    public class ReservationController : Controller, IReservationController
    {
        #region Fields and Properties
        private UserManager<SiedliskoUser> _userManager;
        private IRepository _repository;
        private static bool _operationSucces;
        private static string _operationResultDescription;
        #endregion

        #region ctor

        public ReservationController( UserManager<SiedliskoUser> userManager, IRepository repository)
        {
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
                reservationViewModel = new ReservationIndexViewModel(_repository.GetAllReservations().Where(x => x.ReserverUserName == User.Identity.Name), _repository);
            }
            else
            {
                reservationViewModel = new ReservationIndexViewModel(_repository.GetAllReservations(), _repository);
            }
            SetupLastOperationsResults(reservationViewModel);
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
                SetupLastOperationsResults(editViewModel);
                return View(editViewModel);
            }
            return RedirectToAction("Index");
        }

        //potwierdzenie i powrót do widoku edycji
        [Authorize]
        public async Task<IActionResult> Confirm(int Id)//TODO zrobić view modela i w widoku dodać diva na powiadomienia... lol
        {
            var reservation = _repository.GetReservationById(Id);
            if (reservation != null)
            {
                if (await _repository.UpdateReservation(x => x.Status = Models.Enums.ReservationStatus.Confirmed, Id))
                {
                    _operationResultDescription = "Zmiany zapisane!";
                    _operationSucces = true;
                }
                else
                {
                    _operationResultDescription = "Operacja nie powiodła się";
                }

                return RedirectToAction("Edit", "Reservation", new { Id = reservation.Id });
            }
            _operationResultDescription = "Rezerwacja nie istnieje...";
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

            await TryMakeReservation(createVM);
            SetupLastOperationsResults(createVM);

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
                        if (await _repository.RemoveReservation(reservation))
                        {
                            _operationResultDescription = "Twoja rezerwacja została pomyślnie anulowana.";
                            _operationSucces = true;
                        }
                        else
                        {
                            _operationResultDescription = "Nie udało się anulować rezerwacji";
                            _operationSucces = false;
                        }
                    }
                }
                else
                {
                    if (await _repository.RemoveReservation(reservation))
                    {
                        _operationResultDescription = "Rezerwacja została pomyślnie anulowana.";
                        _operationSucces = true;
                    }
                    else
                    {
                        _operationResultDescription = "Nie udało się anulować rezerwacji";
                        _operationSucces = false;
                    }
                }
            }
            return RedirectToAction("Index", "Reservation");
        }
        #endregion

        #region Private Methods
        private async Task TryMakeReservation(CreateReservationViewModel createVM)
        {
            var room = _repository.GetRoom(createVM.RoomId);

            //ToDo :
            //1. Check if can be reserved

            if (room == null || !CheckIfReservationCanBeMade(room, createVM))
            {
                _operationResultDescription = "Wybrany domek jest zarezerwowany w podanym terminie";
                _operationSucces = false;
                return;
            }

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            //2. add reservation 
            Reservation res = new Reservation()
            {
                StartDate = createVM.StartDate,
                EndDate = createVM.EndDate,
                ReservedOn = DateTime.Now,
                ReserverLastName = user.Nazwisko,
                ReserverUserName = user.UserName,
                Status = Models.Enums.ReservationStatus.WaitingForConfirmation,
                Adults = createVM.Adults,
                Children = createVM.Children,
            };
            res.TotalCost = CountTotalCost(res);

            var addResult = await _repository.AddReservation(res, room);

            //3. save changes and display succes monit
            if (addResult)
            {
                _operationSucces = true;
                _operationResultDescription = "Rezerwacja przebiegła pomyślnie";
            }
            else
            {
                _operationSucces = false;
                _operationResultDescription = "Rezerwacja nie powiodła się";
            }
        }

        private void SetupLastOperationsResults(IViewModel viewModel)
        {
            if (!string.IsNullOrWhiteSpace(_operationResultDescription))
            {
                viewModel.OperationResultsDescription = _operationResultDescription;
                viewModel.OperationSucces = _operationSucces;

                _operationSucces = false;
                _operationResultDescription = null;
            }
        }
        #endregion

        #region Public API
        public decimal CountTotalCost(Reservation res)
        {
            decimal dailyCost = 140;
            if (res.Adults > 4)
            {
                dailyCost += (res.Adults - 4) * 10;
            }

            return (res.NumberOfAccomodations * dailyCost);
        }

        public bool CheckIfReservationCanBeMade(IRoom room, CreateReservationViewModel createViewModel)
        {
            if (room.Reservations.Any(r => createViewModel.StartDate.Date == r.StartDate.Date))
            {
                return false;
            }

            if (room.Reservations.Any(r => createViewModel.EndDate.Date == r.EndDate.Date))
            {
                return false;
            }

            if (room.Reservations.Any(r => (createViewModel.StartDate.Date > r.StartDate.Date) && (createViewModel.StartDate.Date < r.EndDate.Date)))
            {
                return false;
            }

            if (room.Reservations.Any(r => (createViewModel.EndDate.Date > r.StartDate.Date) && (createViewModel.EndDate.Date < r.EndDate.Date)))
            {
                return false;
            }

            if (room.Reservations.Any(r => (createViewModel.EndDate.Date > r.EndDate.Date) && (createViewModel.StartDate.Date < r.StartDate.Date)))
            {
                return false;
            }
            return true;
        } 
        #endregion
    }
}
