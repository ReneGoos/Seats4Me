using System;

namespace Seats4Me.API.Models.Output
{
    public class TimeSlotOutputModel
    {
        public DateTime Day { get; set; }

        public double Hours { get; set; }

        public int Id { get; set; }

        public decimal PromoPrice { get; set; }

        public bool SoldOut { get; set; }

        public int Week { get; set; }
    }
}