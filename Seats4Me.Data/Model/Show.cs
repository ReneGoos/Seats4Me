﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Seats4Me.Data.Model
{
    public class Show
    {
        public int ShowId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal RegularPrice { get; set; }
        public decimal RegularDiscountPrice { get; set; }
        public decimal PromoPrice { get; set; }

        public ICollection<TimeSlot> TimeSlots { get; set;  }
    }
}
