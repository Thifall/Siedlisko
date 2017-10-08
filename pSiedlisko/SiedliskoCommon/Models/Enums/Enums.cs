using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiedliskoCommon.Models.Enums
{
    public enum ReservationStatus
    {
        //day reservation state
        Free,
        PartiallyTaken,
        LastSpots,
        Full,

        //room reservation state
        WaitingForConfirmation,
        Confirmed,
        UnConfirmed,
        PreBooked,
    }

    public enum PriceFor
    {
        Adult,
        Child,
        Room,
    }

    public enum EmailStatus
    {
        ToSend,
        Sending,
        Sent,
    }
}
