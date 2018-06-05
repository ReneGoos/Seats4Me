using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Seats4Me.API.Data;
using Seats4Me.Data.Model;

namespace Seats4Me.API.Model
{
    public class TicketsRepository : TheatreRepository
    {
        public TicketsRepository(TheatreContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Ticket>> GetAsync(int timeslotId)
        {
            var timeSlot = await _context.TimeSlots.Include(t => t.Show)
                .FirstOrDefaultAsync(s => s.TimeSlotId == timeslotId);
            return await _context.Seats
                .GroupJoin(_context.TimeSlotSeats.Where(a => a.TimeSlotId == timeslotId),
                    s => s.SeatId,
                    t => t.SeatId,
                    (s, t) => new
                    {
                        s,
                        t
                    }
                )
                .SelectMany(
                    st => st.t.DefaultIfEmpty(),
                    (a, t) => new
                    {
                        a.s,
                        t
                    }
                )
                .Select(s => new Ticket()
                    {
                        ShowId = timeSlot.Show.ShowId,
                        Name = timeSlot.Show.Name,
                        Title = timeSlot.Show.Title,
                        Description = timeSlot.Show.Description,
                        RegularPrice = timeSlot.Show.RegularPrice,
                        RegularDiscountPrice = timeSlot.Show.RegularDiscountPrice,
                        PromoPrice = timeSlot.PromoPrice,
                        SeatId = s.s.SeatId,
                        Row = s.s.Row,
                        Chair = s.s.Chair,
                        TimeSlotId = timeSlot.TimeSlotId,
                        Start = timeSlot.Day,
                        TimeSlotSeatId = s.t == null ? -1 : s.t.TimeSlotSeatId,
                        Reserved = s.t != null && s.t.Reserved,
                        Paid = s.t != null && s.t.Paid,
                        CustomerEmail = s.t == null ? null : s.t.CustomerEmail
                    }
                )
                .ToListAsync();
        }

        public async Task<int> AddAsync(Ticket value)
        {
            if (!value.Reserved && !value.Paid)
            {
                LastErrorMessage = "The ticket should be reserved or paid!";
                return -1;
            }

            if (value.Price == 0)
            {
                LastErrorMessage = "A price should be chosen for the ticket!";
                return -1;
            }

            var show = await _context.TimeSlots
                .Include(s => s.Show)
                .FirstOrDefaultAsync(s =>
                    s.TimeSlotId == value.TimeSlotId && (s.Show.RegularPrice == value.Price ||
                                                         s.Show.RegularDiscountPrice == value.Price ||
                                                         s.PromoPrice == value.Price));
            if (show == null)
            {
                LastErrorMessage = "The ticket price does not equal any of the shows' prices!";
                return -1;
            }

            var timeslotSeat = await _context.TimeSlotSeats.AddAsync(
                new TimeSlotSeat()
                {
                    CustomerEmail = value.CustomerEmail,
                    Paid = value.Paid,
                    Reserved = value.Reserved,
                    SeatId = value.SeatId,
                    TimeSlotId = value.TimeSlotId,
                    Price = value.Price
                });
            if (!await SaveChangesAsync())
                return -1;

            return timeslotSeat.Entity.TimeSlotSeatId;
        }

        public async Task<bool> UpdateAsync(Ticket value)
        {
            if (!value.Reserved && !value.Paid)
            {
                return await DeleteAsync(value.TimeSlotSeatId);
            }

            var timeSlotSeat = _context.TimeSlotSeats.First(ts => ts.TimeSlotSeatId == value.TimeSlotSeatId);
            timeSlotSeat.CustomerEmail = value.CustomerEmail;
            timeSlotSeat.Paid = value.Paid;
            timeSlotSeat.Reserved = value.Reserved;
            timeSlotSeat.SeatId = value.SeatId;
            timeSlotSeat.TimeSlotId = value.TimeSlotId;

            _context.TimeSlotSeats.Update(timeSlotSeat);
            return await SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int timeslotSeatId)
        {
            var timeSlotSeat = await _context.TimeSlotSeats.FindAsync(timeslotSeatId);
            if (timeSlotSeat == null)
            {
                LastErrorMessage = String.Format("No record found to delete for '{0}'", timeslotSeatId);
                return false;
            }

            _context.TimeSlotSeats.Remove(timeSlotSeat);
            return await SaveChangesAsync();
        }
        public async Task<IEnumerable<Ticket>> GetFreeSeats(int timeslotId)
        {
            var timeSlot = await _context.TimeSlots.Include(t => t.Show)
                .FirstOrDefaultAsync(s => s.TimeSlotId == timeslotId);
            return await _context.Seats
                .GroupJoin(_context.TimeSlotSeats.Where(a => a.TimeSlotId == timeslotId),
                    s => s.SeatId,
                    t => t.SeatId,
                    (s, t) => new
                    {
                        s,
                        t
                    }
                )
                .SelectMany(
                    st => st.t.DefaultIfEmpty(),
                    (a, t) => new
                    {
                        a.s,
                        t
                    }
                )
                .Where(s => s.t == null)
                .Select(s => new Ticket()
                    {
                        ShowId = timeSlot.Show.ShowId,
                        Name = timeSlot.Show.Name,
                        Title = timeSlot.Show.Title,
                        Description = timeSlot.Show.Description,
                        RegularPrice = timeSlot.Show.RegularPrice,
                        RegularDiscountPrice = timeSlot.Show.RegularDiscountPrice,
                        PromoPrice = timeSlot.PromoPrice,
                        SeatId = s.s.SeatId,
                        Row = s.s.Row,
                        Chair = s.s.Chair,
                        TimeSlotId = timeSlot.TimeSlotId,
                        Start = timeSlot.Day,
                        TimeSlotSeatId = -1,
                        Reserved = false,
                        Paid = false,
                        CustomerEmail = null
                    }
                )
                .ToListAsync();
        }

        public async Task<IEnumerable<Ticket>> GetMyTicketsAsync(string email)
        {
            return await _context.TimeSlotSeats
                                .Include(ts => ts.Seat)
                                .Include(ts => ts.TimeSlot)
                                .ThenInclude(s => s.Show)
                                .Where(ts => ts.CustomerEmail.Equals(email))
                                .Select(s => new Ticket()
                                    {
                                        ShowId = s.TimeSlot.Show.ShowId,
                                        Name = s.TimeSlot.Show.Name,
                                        Title = s.TimeSlot.Show.Title,
                                        Description = s.TimeSlot.Show.Description,
                                        RegularPrice = s.TimeSlot.Show.RegularPrice,
                                        RegularDiscountPrice = s.TimeSlot.Show.RegularDiscountPrice,
                                        PromoPrice = s.TimeSlot.PromoPrice,
                                        SeatId = s.Seat.SeatId,
                                        Row = s.Seat.Row,
                                        Chair = s.Seat.Chair,
                                        TimeSlotId = s.TimeSlot.TimeSlotId,
                                        Start = s.TimeSlot.Day,
                                        TimeSlotSeatId = s.TimeSlotSeatId,
                                        Reserved = s.Reserved,
                                        Paid = s.Paid,
                                        CustomerEmail = s.CustomerEmail
                                    }
                                )
                                .ToListAsync();
        }
    }
}
