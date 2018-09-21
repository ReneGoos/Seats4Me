using System;

namespace Seats4Me.API.Models.Output
{
    public class TicketOutputModel
    {
        public int Chair { get; set; }

        public DateTime Day { get; set; }

        public string Description { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        public bool Paid { get; set; }

        public decimal Price { get; set; }

        public decimal PromoPrice { get; set; }

        public decimal RegularDiscountPrice { get; set; }

        public decimal RegularPrice { get; set; }

        public bool Reserved { get; set; }

        public int Row { get; set; }

        public string Title { get; set; }
    }
}