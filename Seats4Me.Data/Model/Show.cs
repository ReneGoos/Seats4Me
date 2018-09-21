using System.Collections.Generic;

namespace Seats4Me.Data.Model
{
    public class Show
    {
        public string Description { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        public decimal RegularDiscountPrice { get; set; }

        public decimal RegularPrice { get; set; }

        public ICollection<TimeSlot> TimeSlots { get; set; }

        public string Title { get; set; }
    }
}