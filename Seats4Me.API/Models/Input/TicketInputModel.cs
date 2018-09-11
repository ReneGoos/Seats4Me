using System;

namespace Seats4Me.API.Models.Input
{
    public class TicketInputModel
    {
        public int Row { get; set; }
        public int Chair { get; set; }
        public bool Reserve { get; set; }
        public bool Pay { get; set; }
        public decimal Price { get; set; }
    }
}
