using SiedliskoCommon.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siedlisko.Models.Helper
{
    public static class NamingHelper
    {
        #region Fields and Properties
        private static IDictionary<int, string> MonthsNames;
        #endregion

        #region Public API
        public static string GetMonthString(int month)
        {
            if (MonthsNames == null)
            {
                MonthsNames = new Dictionary<int, string>();
                MonthsNames.Add(1, "Styczeń");
                MonthsNames.Add(2, "Luty");
                MonthsNames.Add(3, "Marzec");
                MonthsNames.Add(4, "Kwiecień");
                MonthsNames.Add(5, "Maj");
                MonthsNames.Add(6, "Czerwiec");
                MonthsNames.Add(7, "Lipiec");
                MonthsNames.Add(8, "Sierpień");
                MonthsNames.Add(9, "Wrzesień");
                MonthsNames.Add(10, "Październik");
                MonthsNames.Add(11, "Listopad");
                MonthsNames.Add(12, "Grudzień");
            }
            return MonthsNames[month];
        } 

        public static string GetReservationStatusString(ReservationStatus status)
        {
            switch (status)
            {
                case ReservationStatus.Free:
                case ReservationStatus.PartiallyTaken:
                    return "Wolne miejsca";
                case ReservationStatus.LastSpots:
                    return "Ostatnie miejsca";
                case ReservationStatus.Full:
                    return "Brak miejsc";
                case ReservationStatus.WaitingForConfirmation:
                    return "Oczekuje na potwierdzenie";
                case ReservationStatus.Confirmed:
                    return "Potwierdzona";
                case ReservationStatus.UnConfirmed:
                    return "Niepotwierdzona";
                case ReservationStatus.PreBooked:
                    return "Wstępnie zarezerwowana";
                default:
                    return string.Empty;
            }
        }
        #endregion
    }
}
