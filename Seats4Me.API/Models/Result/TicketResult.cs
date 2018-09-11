using Seats4Me.Data.Model;
using System;

namespace Seats4Me.API.Models.Result
{
    public class TicketResult
    {
        public TimeSlot TimeSlot { get; set;  }
        public TimeSlotSeat TimeSlotSeat { get; set; }
        public Seat Seat { get; set; }
    }
}
