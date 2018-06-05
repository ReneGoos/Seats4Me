using Newtonsoft.Json;

namespace Seats4Me.Data.Model
{
    public class TimeSlotSeat
    {
        public int TimeSlotSeatId { get; set; }
        public int SeatId { get; set; }
        public int TimeSlotId { get; set; }
        public bool Reserved { get; set; }
        public bool Paid { get; set; }
        public decimal Price { get; set; }
        public string CustomerEmail { get; set; }

        [JsonIgnore]
        public virtual Seat Seat { get; set; }
        [JsonIgnore]
        public virtual TimeSlot TimeSlot { get; set; }
    }
}
