using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Siedlisko.Models;
using Siedlisko.Models.Interfaces;
using Siedlisko.ViewModels;
using Siedlisko.ViewModels.Interfaces;
using SiedliskoCommon.Models;
using SiedliskoCommon.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Siedlisko.Reservations
{
    public class Reserver
    {
        private readonly object _operationResultLocker = new object();

        private readonly IRepository _repository;
        private readonly IConfigurationRoot _config;
        private static ILogger Logger;
        private IEmailRepository _emailRepository;

        public string LastOperationResultsDescription { get; set; }
        public bool LastOperationResult { get; set; }


        public Reserver(IRepository repository, IEmailRepository emailRepository, IConfigurationRoot config, ILoggerFactory loggerFactory)
        {
            _repository = repository;
            _config = config;
            if (Logger == null)
            {
            Logger = loggerFactory.CreateLogger(typeof(Reserver));
            }
            _emailRepository = emailRepository;
        }

        public async Task TryMakeReservation(CreateReservationViewModel createVM, SiedliskoUser user)
        {
            var room = _repository.GetRoom(createVM.RoomId);
            var res = Mapper.Map<Reservation>(createVM);

            //1. Check if can be reserved

            if (room == null || !CheckIfReservationCanBeMade(room, res))
            {
                LastOperationResultsDescription = "Wybrany domek jest zarezerwowany w podanym terminie";
                LastOperationResult = false;
                return;
            }
            
            //2. add reservation
            res.ReservedOn = DateTime.Now;
            res.ReserverLastName = user.Nazwisko;
            res.ReserverUserName = user.UserName;
            res.Status = ReservationStatus.WaitingForConfirmation;
            res.TotalCost = CountTotalCost(res);

            var addResult = await _repository.AddReservation(res, room);

            //3. save changes and display succes monit
            if (addResult != null)
            {
                SetupLastOperationResults("Rezerwacja przebiegła pomyślnie", true);
                PrepareEmail(user, res);
                PrepareEmailForAdministration(user, res);
            }
            else
            {
                SetupLastOperationResults("Rezerwacja nie powiodła się", false);
            }
        }

        public async Task ConfirmReservation(int reservationId)
        {
            var reservation = _repository.GetReservationById(reservationId);
            if (reservation != null)
            {
                if (await _repository.UpdateReservation(x => x.Status = ReservationStatus.Confirmed, reservationId))
                {
                    SetupLastOperationResults("Zmiany zapisane!", true);
                }
                else
                {
                    SetupLastOperationResults("Operacja nie powiodła się", false);
                }
                return;
            }
            SetupLastOperationResults("Rezerwacja nie istnieje...", false);
        }

        public void RetriveLastOperationResults(IViewModel viewModel)
        {
            if (!string.IsNullOrWhiteSpace(LastOperationResultsDescription))
            {
                viewModel.OperationResultsDescription = LastOperationResultsDescription;
                viewModel.OperationSucces = LastOperationResult;

                LastOperationResult = false;
                LastOperationResultsDescription = null;

                if (Monitor.IsEntered(_operationResultLocker))
                {
                    Monitor.Exit(_operationResultLocker);
                    Logger.LogInformation("[Reserver.RetriveLastOperationResults()] Exit operation lock!");
                }
                else
                {
                    Logger.LogInformation("[Reserver.RetriveLastOperationResults()] Tried to exit operation lock, but it was not held!");
                }
            }
        }

        public async Task RemoveReservation(Reservation reservation)
        {
            Logger.LogInformation("[Reserver.RemoveReservation] Trying to remove reservation with ID: {0}", reservation.Id);
            if (await _repository.RemoveReservation(reservation))
            {
                LastOperationResultsDescription = "Rezerwacja została pomyślnie anulowana.";
                LastOperationResult = true;
                Logger.LogInformation("[Reserver.RemoveReservation] Successfully removed reservation with ID: {0}", reservation.Id);
            }
            else
            {
                LastOperationResultsDescription = "Operacja nie powiodła się!";
                LastOperationResult = false;
                Logger.LogInformation("[Reserver.RemoveReservation] Failed to remove reservation with ID: {0}", reservation.Id);
            }
        }

        public bool CheckIfReservationCanBeMade(IRoom room, Reservation res)
        {
            if (room.Reservations.Any(r => res.StartDate.Date == r.StartDate.Date))
            {
                return false;
            }

            if (room.Reservations.Any(r => res.EndDate.Date == r.EndDate.Date))
            {
                return false;
            }

            if (room.Reservations.Any(r => (res.StartDate.Date > r.StartDate.Date) && (res.StartDate.Date < r.EndDate.Date)))
            {
                return false;
            }

            if (room.Reservations.Any(r => (res.EndDate.Date > r.StartDate.Date) && (res.EndDate.Date < r.EndDate.Date)))
            {
                return false;
            }

            if (room.Reservations.Any(r => (res.EndDate.Date > r.EndDate.Date) && (res.StartDate.Date < r.StartDate.Date)))
            {
                return false;
            }
            return true;
        }

        private decimal CountTotalCost(Reservation res)
        {
            decimal dailyCost = 140;
            if (res.Adults > 4)
            {
                dailyCost += (res.Adults - 4) * 10;
            }

            return (res.NumberOfAccomodations * dailyCost);
        }

        private void SetupLastOperationResults(string desc, bool success)
        {
            Monitor.Enter(_operationResultLocker);
            Logger.LogInformation("[Reserver.SetupLastOperationResults()] Entering Operation lock!");
            LastOperationResult = success;
            LastOperationResultsDescription = desc;
        }

        private void PrepareEmailForAdministration(SiedliskoUser user, Reservation reservation)
        {
            try
            {
                EmailMessage email = new EmailMessage
                {
                    CreationTime = DateTime.Now,
                    ReservationId = reservation.Id,
                    ToAdress = _config["EmailNotificationsConfiguration:TargetNotificationEmail"],
                    status = EmailStatus.ToSend,
                    ToLogin = user.UserName
                };
                string format = System.IO.File.ReadAllText(_config["EmailNotificationsConfiguration:TemplateForAdminPath"]);
                email.MessageBody = string.Format(format,
                    reservation.StartDate.ToString("dd-MMMM-yyyy"),
                    reservation.EndDate.ToString("dd-MMMM-yyyy"),
                    reservation.NumberOfAccomodations,
                    reservation.ReserverLastName,
                    user.Email,
                    user.PhoneNumber,
                    reservation.Id);
                _emailRepository.AddEmail(email);
            }
            catch (Exception ex)
            {
                Logger.LogError("[Reserver.PrepareEmailForAdministration()] Error occured while generating e-mail. ERROR: {0}", ex);
            }
        }

        private void PrepareEmail(SiedliskoUser user, Reservation reservation)
        {
            try
            {
                EmailMessage email = new EmailMessage
                {
                    CreationTime = DateTime.Now,
                    ReservationId = reservation.Id,
                    ToAdress = user.Email,
                    status = EmailStatus.ToSend,
                    ToLogin = user.UserName
                };
                string format = System.IO.File.ReadAllText(_config["EmailNotificationsConfiguration:TemplateForUserPath"]);
                email.MessageBody = string.Format(format,
                    email.ToLogin,
                    reservation.StartDate.ToString("dd-MMMM-yyyy"),
                    reservation.EndDate.ToString("dd-MMMM-yyyy"),
                    (reservation.Adults + reservation.Children),
                    reservation.ReserverLastName,
                    reservation.TotalCost,
                    reservation.Id);
                _emailRepository.AddEmail(email);
            }
            catch (Exception ex)
            {
                Logger.LogError("[Reserver.PrepareEmail()] Error occured while generating e-mail. ERROR: {0}", ex);
            }
        }
    }
}
