using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Seats4Me.Data.Model
{
    public class TimeSlot : DataWithId
    {
        public DateTime Day { get; set; }
        public double Hours { get; set; }
        public int ShowId { get; set; }
        public decimal PromoPrice { get; set; }

        [JsonIgnore]
        public virtual Show Show { get; set; }
        public virtual ICollection<TimeSlotSeat> TimeSlotSeats { get; set; }
    }
}
