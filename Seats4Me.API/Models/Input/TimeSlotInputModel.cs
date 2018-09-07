using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Seats4Me.Data.Common;
using Seats4Me.Data.Model;

namespace Seats4Me.API.Models.Input
{
    public class TimeSlotInputModel
    {
        public DateTime Day { get; set; }
        public double Hours { get; set; }
        public decimal PromoPrice { get; set; }
        public ICollection<TimeSlotSeat> TimeSlotSeats { get; set; } = new List<TimeSlotSeat>();
    }
}
