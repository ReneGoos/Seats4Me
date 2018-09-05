using System.Collections.Generic;

namespace Seats4Me.API.Models.Output
{
    public class ShowOutputModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal RegularPrice { get; set; }
        public decimal RegularDiscountPrice { get; set; }
    }
}
