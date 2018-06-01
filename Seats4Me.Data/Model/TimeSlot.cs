using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Newtonsoft.Json;

namespace Seats4Me.Data.Model
{
    public class TimeSlot
    {
        private DateTime _startDateTime;

        public int TimeSlotId { get; set; }
        public DateTime Start
        {
            get => _startDateTime;
            set
            {
                Week = CultureInfo.CurrentCulture.Calendar
                    .GetWeekOfYear(value,
                        CalendarWeekRule.FirstFourDayWeek,
                        DayOfWeek.Monday
                    );
                _startDateTime = value;
            }
        }

        public double Length { get; set; }
        public int ShowId { get; set; }
        public int Week { get; private set; }
    
        [JsonIgnore]
        public virtual Show Show { get; set; }
        public virtual ICollection<TimeSlotSeat> TimeSlotSeats { get; set; }
    }
}
