using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Seats4Me.Data.Model;

namespace Seats4Me.API.Models.Result
{
    public class ShowResult
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal RegularPrice { get; set; }
        public decimal RegularDiscountPrice { get; set; }
        public DateTime Day { get; set; }
        public double Hours { get; set; }
        public int Week { get; set; }
        public decimal PromoPrice { get; set; }
    }
}
