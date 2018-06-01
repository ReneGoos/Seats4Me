using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
                        PromoPrice = timeSlot.Show.PromoPrice,
                        SeatId = s.s.SeatId,
                        Row = s.s.Row,
                        Chair = s.s.Chair,
                        TimeSlotId = timeSlot.TimeSlotId,
                        Start = timeSlot.Start,
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

            var timeslotSeat = await _context.TimeSlotSeats.AddAsync(
                new TimeSlotSeat()
                {
                    CustomerEmail = value.CustomerEmail,
                    Paid = value.Paid,
                    Reserved = value.Reserved,
                    SeatId = value.SeatId,
                    TimeSlotId = value.TimeSlotId
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

            var timeSlotSeat = new TimeSlotSeat()
            {
                TimeSlotSeatId = value.TimeSlotSeatId,
                CustomerEmail = value.CustomerEmail,
                Paid = value.Paid,
                Reserved = value.Reserved,
                SeatId = value.SeatId,
                TimeSlotId = value.TimeSlotId
            };

            _context.TimeSlotSeats.Attach(timeSlotSeat);
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
    }
}
