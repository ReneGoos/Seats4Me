using System.Collections.Generic;

namespace Seats4Me.API.Models.Input
{
    public class ShowInputModel
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal RegularPrice { get; set; }
        public decimal RegularDiscountPrice { get; set; }
        public ICollection<TimeSlotInputModel> TimeSlots { get; set; } = new List<TimeSlotInputModel>();
    }
}
