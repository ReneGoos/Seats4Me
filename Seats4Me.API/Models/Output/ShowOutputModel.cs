using System.Collections.Generic;

namespace Seats4Me.API.Models.Output
{
    public class ShowOutputModel
    {
        public string Description { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        public decimal RegularDiscountPrice { get; set; }

        public decimal RegularPrice { get; set; }

        public ICollection<TimeSlotOutputModel> TimeSlots { get; set; } = new List<TimeSlotOutputModel>();

        public string Title { get; set; }
    }
}