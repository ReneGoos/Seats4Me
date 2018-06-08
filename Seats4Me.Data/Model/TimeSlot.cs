using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Seats4Me.Data.Common;

namespace Seats4Me.Data.Model
{
    public class TimeSlot
    {
        private DateTime _startDateTime;

        public int Id { get; set; }
        public DateTime Day
        {
            get => _startDateTime;
            set
            {
                Week = value.Week();
                _startDateTime = value;
            }
        }

        public double Hours { get; set; }
        public int ShowId { get; set; }
        public int Week { get; private set; }
        public decimal PromoPrice { get; set; }

        [JsonIgnore]
        public virtual Show Show { get; set; }
        public virtual ICollection<TimeSlotSeat> TimeSlotSeats { get; set; }
    }
}
