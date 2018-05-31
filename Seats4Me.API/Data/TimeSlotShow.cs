using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seats4Me.API.Data
{
    public class TimeSlotShow
    {
        public int ShowId { get; set; }
        public int TimeSlotId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal RegularPrice { get; set; }
        public decimal RegularDiscountPrice { get; set; }
        public decimal PromoPrice { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
