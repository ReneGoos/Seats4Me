using Seats4Me.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seats4Me.API.Data
{
    public class CalendarShows
    {
        public int Week { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        public IEnumerable<TimeSlotShow> Shows { get; set; }
}
}
