using System;
using Seats4Me.Data.Model;

namespace Seats4Me.API.Data
{
    public class Ticket
    {
        public int ShowId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal RegularPrice { get; set; }
        public decimal RegularDiscountPrice { get; set; }
        public decimal PromoPrice { get; set; }
        public int TimeSlotId { get; set; }
        public DateTime Start { get; set; }
        public int SeatId { get; set; }
        public int Row { get; set; }
        public int Chair { get; set; }
        public int TimeSlotSeatId { get; set; }
        public bool Reserved { get; set; }
        public bool Paid { get; set; }
        public decimal Price { get; set; }
        public string Email { get; set; }
    }
}
