using System;
using System.Collections.Generic;

namespace Seats4Me.Data.Model
{
    public class TimeSlot
    {
        public int Id { get; set; }

        public DateTime Day { get; set; }

        public double Hours { get; set; }

        public decimal PromoPrice { get; set; }

        public virtual Show Show { get; set; }

        public int ShowId { get; set; }

        public virtual ICollection<TimeSlotSeat> TimeSlotSeats { get; set; }
    }
}