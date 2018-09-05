using System;

namespace Seats4Me.API.Models
{
    public class TimeSlotShow
    {
        public int ShowId { get; set; }
        public int TimeSlotId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal RegularPrice { get; set; }
        public decimal RegularDiscountPrice { get; set; }
        public decimal PromoPrice { get; set; }
        public DateTime Day { get; set; }
        public double Hours { get; set; }
        public bool SoldOut { get; set; }
    }
}
