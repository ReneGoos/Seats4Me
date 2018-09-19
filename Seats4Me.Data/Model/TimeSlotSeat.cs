namespace Seats4Me.Data.Model
{
    public class TimeSlotSeat
    {
        public int Id { get; set; }

        public bool Paid { get; set; }

        public decimal Price { get; set; }

        public bool Reserved { get; set; }

        public virtual Seat Seat { get; set; }

        public int SeatId { get; set; }

        public virtual Seats4MeUser Seats4MeUser { get; set; }

        public int Seats4MeUserId { get; set; }

        public virtual TimeSlot TimeSlot { get; set; }

        public int TimeSlotId { get; set; }
    }
}