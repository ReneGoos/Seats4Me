using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seats4Me.API.Models.Search
{
    public class ShowSearchModel
    {
        public int Week { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public bool Promotion { get; set; }
    }
}
