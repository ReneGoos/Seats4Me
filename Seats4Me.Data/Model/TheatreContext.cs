using Microsoft.EntityFrameworkCore;

namespace Seats4Me.Data.Model
{
    public class TheatreContext : DbContext
    {
        public TheatreContext(DbContextOptions<TheatreContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Show>()
                .HasKey(s => s.ShowId);

            modelBuilder.Entity<Show>()
                .Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(40);

            modelBuilder.Entity<Show>()
                .Property(s => s.Title)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Show>()
                .Property(s => s.Description)
                .IsRequired()
                .HasMaxLength(4000);

            modelBuilder.Entity<Show>()
                .Property(s => s.RegularPrice)
                .IsRequired();

            modelBuilder.Entity<Show>()
                .Property(s => s.RegularDiscountPrice)
                .IsRequired();

            modelBuilder.Entity<TimeSlot>()
                .HasKey(s => s.TimeSlotId);

            modelBuilder.Entity<TimeSlot>()
                .Property(s => s.Day)
                .IsRequired();

            modelBuilder.Entity<TimeSlot>()
                .Property(s => s.ShowId)
                .IsRequired();

            modelBuilder.Entity<TimeSlotSeat>()
                .HasKey(s => new { s.TimeSlotSeatId });

            modelBuilder.Entity<TimeSlotSeat>()
                .HasIndex(s => new { s.TimeSlotId, s.SeatId })
                .IsUnique();

            modelBuilder.Entity<TimeSlotSeat>()
                .Property(s => s.CustomerEmail)
                .IsRequired();

            modelBuilder.Entity<Seat>()
                .HasKey(s => s.SeatId);

            modelBuilder.Entity<Seat>()
                .Property(s => s.Row)
                .IsRequired();

            modelBuilder.Entity<Seat>()
                .Property(s => s.Chair)
                .IsRequired();
        }

        public DbSet<Show> Shows { get; set; }
        public DbSet<TimeSlot> TimeSlots { get; set; }
        public DbSet<TimeSlotSeat> TimeSlotSeats { get; set; }
        public DbSet<Seat> Seats { get; set; }
    }
}
