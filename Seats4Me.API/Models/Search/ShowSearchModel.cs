using System;

namespace Seats4Me.API.Models.Search
{
    public class ShowSearchModel
    {
        public int Month { get; set; }

        public bool Promotion { get; set; } = false;

        public int Week { get; set; }

        public int Year { get; set; } = DateTime.Today.Year;
    }
}