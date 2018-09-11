using System;
using System.Collections.Generic;
using Seats4Me.Data.Model;

namespace Seats4Me.API.Models.Output
{
    public class TimeSlotOutputModel
    {
        public int Id { get; set; }
        public DateTime Day { get; set; }
        public double Hours { get; set; }
        public int Week { get; set; }
        public decimal PromoPrice { get; set; }
        public ICollection<TicketOutputModel> Tickets { get; set; } = new List<TicketOutputModel>();
    }
}
