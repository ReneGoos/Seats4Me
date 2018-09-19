using Microsoft.EntityFrameworkCore;

namespace Seats4Me.Data.Model
{
    public class TheatreContext : DbContext
    {
        public DbSet<Seat> Seats { get; set; }

        public DbSet<Seats4MeUser> Seats4MeUsers { get; set; }

        public DbSet<Show> Shows { get; set; }

        public DbSet<TimeSlot> TimeSlots { get; set; }

        public DbSet<TimeSlotSeat> TimeSlotSeats { get; set; }

        public TheatreContext(DbContextOptions<TheatreContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Show>().HasKey(s => s.Id);

            modelBuilder.Entity<Show>().Property(s => s.Name).IsRequired().HasMaxLength(40);

            modelBuilder.Entity<Show>().Property(s => s.Title).IsRequired().HasMaxLength(100);

            modelBuilder.Entity<Show>().Property(s => s.Description).IsRequired().HasMaxLength(4000);

            modelBuilder.Entity<Show>().Property(s => s.RegularPrice).IsRequired();

            modelBuilder.Entity<Show>().Property(s => s.RegularDiscountPrice).IsRequired();

            modelBuilder.Entity<TimeSlot>().HasKey(s => s.Id);

            modelBuilder.Entity<TimeSlot>().Property(s => s.Day).IsRequired();

            modelBuilder.Entity<TimeSlot>().Property(s => s.ShowId).IsRequired();

            modelBuilder.Entity<TimeSlotSeat>()
                        .HasKey(s => new
                                     {
                                         TimeSlotSeatId = s.Id
                                     });

            modelBuilder.Entity<TimeSlotSeat>()
                        .HasIndex(s => new
                                       {
                                           s.TimeSlotId,
                                           s.SeatId
                                       })
                        .IsUnique();

            modelBuilder.Entity<TimeSlotSeat>().Property(s => s.Seats4MeUserId).IsRequired();

            modelBuilder.Entity<Seat>().HasKey(s => s.Id);

            modelBuilder.Entity<Seat>().Property(s => s.Row).IsRequired();

            modelBuilder.Entity<Seat>().Property(s => s.Chair).IsRequired();

            modelBuilder.Entity<Seats4MeUser>().HasKey(s => s.Id);

            modelBuilder.Entity<Seats4MeUser>().Property(s => s.Name).IsRequired();

            modelBuilder.Entity<Seats4MeUser>().Property(s => s.Email).IsRequired();

            modelBuilder.Entity<Seats4MeUser>().Property(s => s.Password).IsRequired();

            modelBuilder.Entity<Seats4MeUser>().HasIndex(s => s.Email).IsUnique();
        }
    }
}