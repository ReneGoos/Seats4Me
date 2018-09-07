using System;
using System.Collections.Generic;
using Seats4Me.Data.Model;

namespace Seats4Me.API.Models.Output
{
    public class TimeSlotOutputModel
    {
        public int Id { get; set; }
        private DateTime Day { get; set; }
        public double Hours { get; set; }
        public int ShowId { get; set; }
        public int Week { get; private set; }
        public decimal PromoPrice { get; set; }
        public ICollection<TimeSlotSeat> TimeSlotSeats { get; set; } = new List<TimeSlotSeat>();
    }
}
