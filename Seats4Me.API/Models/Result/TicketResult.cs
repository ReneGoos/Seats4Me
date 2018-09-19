using Seats4Me.Data.Model;

namespace Seats4Me.API.Models.Result
{
    public class TicketResult
    {
        public Seat Seat { get; set; }

        public TimeSlot TimeSlot { get; set; }

        public TimeSlotSeat TimeSlotSeat { get; set; }
    }
}