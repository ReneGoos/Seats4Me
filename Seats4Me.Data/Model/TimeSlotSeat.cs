using System;
using System.Collections.Generic;
using System.Text;

namespace Seats4Me.Data.Model
{
    public class TimeSlotSeat
    {
        public int TimeSlotSeatId { get; set; }
        public int SeatId { get; set; }
        public int TimeSlotId { get; set; }
        public bool Reserved { get; set; }
        public bool Paid { get; set; }
        public string CustomerEmail { get; set; }

        public Seat Seat { get; set; }
        public TimeSlot TimeSlot { get; set; }
    }
}
