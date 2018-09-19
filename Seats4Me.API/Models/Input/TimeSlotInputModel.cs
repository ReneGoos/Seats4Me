using System;

namespace Seats4Me.API.Models.Input
{
    public class TimeSlotInputModel
    {
        public DateTime Day { get; set; }

        public double Hours { get; set; }

        public decimal PromoPrice { get; set; }
    }
}