using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Seats4Me.Data.Model
{
    public class TheatreContext : DbContext
    {
        public TheatreContext(DbContextOptions<TheatreContext> options) : base(options)
        {

        }

        public DbSet<Show> Shows { get; set; }
        public DbSet<TimeSlot> TimeSlots { get; set; }
        public DbSet<TimeSlotSeat> TimeSlotSeats { get; set; }
        public DbSet<Seat> Seats { get; set; }
    }
}
