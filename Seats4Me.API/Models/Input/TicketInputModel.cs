namespace Seats4Me.API.Models.Input
{
    public class TicketInputModel
    {
        public int Chair { get; set; }

        public bool Discount { get; set; }

        public bool Paid { get; set; }

        public bool Reserved { get; set; }

        public int Row { get; set; }
    }
}