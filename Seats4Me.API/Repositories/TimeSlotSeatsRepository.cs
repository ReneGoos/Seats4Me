﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Seats4Me.API.Models.Result;
using Seats4Me.Data.Model;

namespace Seats4Me.API.Repositories
{
    public class TimeSlotSeatsRepository : TheatreRepository, ITimeSlotSeatsRepository
    {
        public TimeSlotSeatsRepository(TheatreContext context) : base(context)
        {
        }

        public Task<TimeSlotSeat> AddAsync(TimeSlotSeat value)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int ticketId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TimeSlotSeat>> GetAsync(int? showId = null, int? timeSlotId = null,
            int? userId = null)
        {
            var timeSlotSeats = await _context.TimeSlotSeats.Where(tss =>
                showId == null || tss.TimeSlot.ShowId == showId || timeSlotId == null || tss.TimeSlotId == timeSlotId ||
                userId == null || tss.Seats4MeUserId == userId).ToListAsync();

            return timeSlotSeats;
        }

        public Task<TimeSlotSeat> GetAsync(int ticketId)
        {
            var timeSlotSeat = _context.TimeSlotSeats.FirstOrDefaultAsync(tss => tss.Id == ticketId);

            return timeSlotSeat;
        }

        public async Task<TimeSlotSeat> UpdateAsync(TimeSlotSeat value)
        {
            LastErrorMessage = "";
            _context.TimeSlotSeats.Attach(value);
            var timeSlotSeatEntity = _context.TimeSlotSeats.Update(value);
            if (!await SaveChangesAsync())
                return null;

            return timeSlotSeatEntity.Entity;
        }

        public async Task<IEnumerable<TicketResult>> GetTicketsByTimeSlotAsync(int timeSlotId)
        {
            var timeSlot = await _context.TimeSlots.Include(t => t.Show)
                .FirstOrDefaultAsync(s => s.Id == timeSlotId);
            return await _context.Seats
                .GroupJoin(_context.TimeSlotSeats.Where(a => a.TimeSlotId == timeSlotId),
                    s => s.Id,
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
                .Select(s => new TicketResult
                    {
                        TimeSlot = timeSlot,
                        TimeSlotSeat = s.t,
                        Seat = s.s
                    }
                )
                .ToListAsync();
        }

        /*
        public bool ValidTicketUser(int timeSlotSeatId, string email)
        {
            var ticket = _context.TimeSlotSeats.Include(t => t.Seats4MeUser).First(t => t.Id == timeSlotSeatId);
            if (ticket == null || !ticket.Seats4MeUser.Email.Equals(email))
            {
                LastErrorMessage = "Invalid ticket for this user!";
                return false;
            }
            return true;
        }

        public async Task<int> AddAsync(TicketOutputModel value)
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
                    s.Id == value.TimeSlotId && (s.Show.RegularPrice == value.Price ||
                                                         s.Show.RegularDiscountPrice == value.Price ||
                                                         s.PromoPrice == value.Price));
            if (show == null)
            {
                LastErrorMessage = "The ticket price does not equal any of the shows' prices!";
                return -1;
            }

            var seats4MeUser = await _context.Seats4MeUsers.FirstOrDefaultAsync(u => u.Email.Equals(value.Email));
            if (seats4MeUser == null)
            {
                LastErrorMessage = "User not found";
                return -1;
            }

            var timeSlotSeat = await _context.TimeSlotSeats.AddAsync(
                new TimeSlotSeat()
                {
                    Seats4MeUserId = seats4MeUser.Id,
                    Paid = value.Paid,
                    Reserved = value.Reserved,
                    SeatId = value.SeatId,
                    TimeSlotId = value.TimeSlotId,
                    Price = value.Price
                });
            if (!await SaveChangesAsync())
                return -1;

            return timeSlotSeat.Entity.Id;
        }

        public async Task<bool> UpdateAsync(TicketOutputModel value)
        {
            if (!value.Reserved && !value.Paid)
            {
                return await DeleteAsync(value.TimeSlotSeatId);
            }

            var timeSlotSeat = _context.TimeSlotSeats.First(ts => ts.Id == value.TimeSlotSeatId);
            timeSlotSeat.Paid = value.Paid;
            timeSlotSeat.Reserved = value.Reserved;
            timeSlotSeat.SeatId = value.SeatId;
            timeSlotSeat.TimeSlotId = value.TimeSlotId;

            _context.TimeSlotSeats.Update(timeSlotSeat);
            return await SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int timeSlotSeatId)
        {
            var timeSlotSeat = await _context.TimeSlotSeats.FindAsync(timeSlotSeatId);
            if (timeSlotSeat == null)
            {
                LastErrorMessage = $"No record found to delete for '{timeSlotSeatId}'";
                return false;
            }

            _context.TimeSlotSeats.Remove(timeSlotSeat);
            return await SaveChangesAsync();
        }
        public async Task<IEnumerable<TicketOutputModel>> GetFreeSeats(int timeSlotId)
        {
            var timeSlot = await _context.TimeSlots.Include(t => t.Show)
                .FirstOrDefaultAsync(s => s.Id == timeSlotId);
            return await _context.Seats
                .GroupJoin(_context.TimeSlotSeats.Where(a => a.TimeSlotId == timeSlotId),
                    s => s.Id,
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
                .Select(s => new TicketOutputModel()
                    {
                        ShowId = timeSlot.Show.Id,
                        Name = timeSlot.Show.Name,
                        Title = timeSlot.Show.Title,
                        Description = timeSlot.Show.Description,
                        RegularPrice = timeSlot.Show.RegularPrice,
                        RegularDiscountPrice = timeSlot.Show.RegularDiscountPrice,
                        PromoPrice = timeSlot.PromoPrice,
                        SeatId = s.s.Id,
                        Row = s.s.Row,
                        Chair = s.s.Chair,
                        TimeSlotId = timeSlot.Id,
                        Start = timeSlot.Day,
                        TimeSlotSeatId = -1,
                        Reserved = false,
                        Paid = false,
                        Email = null
                    }
                )
                .ToListAsync();
        }

        public async Task<IEnumerable<TicketOutputModel>> GetMyTicketsAsync(string email)
        {
            return await _context.TimeSlotSeats
                                .Include(ts => ts.Seats4MeUser)
                                .Include(ts => ts.Seat)
                                .Include(ts => ts.TimeSlot)
                                .ThenInclude(s => s.Show)
                                .Where(ts => ts.Seats4MeUser.Email.Equals(email))
                                .Select(s => new TicketOutputModel()
                                    {
                                        ShowId = s.TimeSlot.Show.Id,
                                        Name = s.TimeSlot.Show.Name,
                                        Title = s.TimeSlot.Show.Title,
                                        Description = s.TimeSlot.Show.Description,
                                        RegularPrice = s.TimeSlot.Show.RegularPrice,
                                        RegularDiscountPrice = s.TimeSlot.Show.RegularDiscountPrice,
                                        PromoPrice = s.TimeSlot.PromoPrice,
                                        SeatId = s.Seat.Id,
                                        Row = s.Seat.Row,
                                        Chair = s.Seat.Chair,
                                        TimeSlotId = s.TimeSlot.Id,
                                        Start = s.TimeSlot.Day,
                                        TimeSlotSeatId = s.Id,
                                        Reserved = s.Reserved,
                                        Paid = s.Paid,
                                        Email = s.Seats4MeUser.Email
                                }
                                )
                                .ToListAsync();
        }
        */
    }
}