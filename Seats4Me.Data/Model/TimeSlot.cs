using System;
using System.Collections.Generic;
using System.Text;

namespace Seats4Me.Data.Model
{
    public class TimeSlot
    {
        public int TimeSlotId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int ShowId { get; set; }

        public Show Show { get; set; }
        public ICollection<TimeSlotSeat> TimeSlotSeats { get; set; }
    }
}
