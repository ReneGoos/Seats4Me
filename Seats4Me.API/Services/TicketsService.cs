using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AutoMapper;

using Seats4Me.API.Models.Input;
using Seats4Me.API.Models.Output;
using Seats4Me.API.Repositories;
using Seats4Me.Data.Model;

namespace Seats4Me.API.Services
{
    public class TicketsService : ITicketsService
    {
        private readonly ISeatsRepository _seatsRepository;
        private readonly ITimeSlotSeatsRepository _timeSlotSeatsRepository;
        private readonly ITimeSlotsService _timeSlotsService;
        private readonly IUsersRepository _usersRepository;

        public TicketsService(ITimeSlotsService timeSlotsService, ITimeSlotSeatsRepository timeSlotSeatsRepository, IUsersRepository usersRepository, ISeatsRepository seatsRepository)
        {
            _timeSlotsService = timeSlotsService;
            _timeSlotSeatsRepository = timeSlotSeatsRepository;
            _usersRepository = usersRepository;
            _seatsRepository = seatsRepository;
        }

        public async Task<TicketOutputModel> AddAsync(int showId, int timeSlotId, TicketInputModel ticketInput, string email)
        {
            var user = await ValidateUserAsync(email);

            if (user == null)
            {
                throw new UnauthorizedAccessException($"User with {email} is not valid");
            }

            var seat = await _seatsRepository.GetAsync(ticketInput.Row, ticketInput.Chair);

            if (seat == null)
            {
                throw new ArgumentException($"Seat {ticketInput.Chair} on row {ticketInput.Row} is not valid");
            }

            var price = await _timeSlotsService.GetPriceAsync(showId, timeSlotId, ticketInput.Discount);

            var ticket = Mapper.Map<TimeSlotSeat>(ticketInput);
            ticket.TimeSlotId = timeSlotId;
            ticket.Seats4MeUserId = user.Id;
            ticket.SeatId = seat.Id;
            ticket.Price = price;

            ticket = await _timeSlotSeatsRepository.AddAsync(ticket);

            return ticket == null
                       ? null
                       : Mapper.Map<TicketOutputModel>(await GetTicketAsync(ticket.Id));
        }

        public async Task DeleteAsync(int id, string email)
        {
            var user = await ValidateUserAsync(email);

            if (user == null)
            {
                throw new UnauthorizedAccessException($"User with {email} is not valid");
            }

            var ticket = await _timeSlotSeatsRepository.GetAsync(id);

            if (ticket == null)
            {
                return;
            }

            if (ticket.Seats4MeUserId != user.Id)
            {
                throw new UnauthorizedAccessException($"User with {email} is not allowed to delete this ticket {id}");
            }

            await _timeSlotSeatsRepository.DeleteAsync(ticket);
        }

        public async Task<TicketOutputModel> GetTicketAsync(int id)
        {
            var ticket = await _timeSlotSeatsRepository.GetTicketAsync(id);

            return ticket == null
                       ? null
                       : Mapper.Map<TicketOutputModel>(ticket);
        }

        public async Task<List<TicketOutputModel>> GetTicketsByTimeSlotAsync(int showId, int timeSlotId)
        {
            var tickets = await _timeSlotSeatsRepository.GetTicketsByTimeSlotAsync(showId, timeSlotId);

            return Mapper.Map<List<TicketOutputModel>>(tickets);
        }

        public async Task<List<TicketOutputModel>> GetTicketsByUserAsync(string email)
        {
            var user = await ValidateUserAsync(email);

            if (user == null)
            {
                throw new UnauthorizedAccessException($"User with {email} is not valid");
            }

            var tickets = await _timeSlotSeatsRepository.GetTicketsByUserAsync(user.Id);

            return Mapper.Map<List<TicketOutputModel>>(tickets);
        }

        public async Task<TicketOutputModel> UpdateAsync(int id, TicketInputModel ticketInput, string email)
        {
            var user = await ValidateUserAsync(email);

            if (user == null)
            {
                throw new UnauthorizedAccessException($"User with {email} is not valid");
            }

            var ticket = await _timeSlotSeatsRepository.GetAsync(id);

            if (ticket == null)
            {
                throw new ArgumentException($"Ticket {id} is not valid");
            }

            if (ticket.Seats4MeUserId != user.Id)
            {
                throw new UnauthorizedAccessException($"User with {email} cannot update this ticket {id}");
            }

            var seat = await _seatsRepository.GetAsync(ticketInput.Row, ticketInput.Chair);

            if (seat == null)
            {
                throw new ArgumentException($"Seat {ticketInput.Chair} on row {ticketInput.Row} is not valid");
            }

            var price = ticket.Price == 0
                            ? await _timeSlotsService.GetPriceAsync(ticket.TimeSlot.ShowId, ticket.TimeSlotId, ticketInput.Discount)
                            : ticket.Price;

            ticket.Paid = ticketInput.Paid;
            ticket.Reserved = ticketInput.Reserved;
            ticket.SeatId = seat.Id;
            ticket.Price = price;

            ticket = await _timeSlotSeatsRepository.UpdateAsync(ticket);

            return ticket == null
                       ? null
                       : Mapper.Map<TicketOutputModel>(await GetTicketAsync(ticket.Id));
        }

        private Task<Seats4MeUser> ValidateUserAsync(string email)
        {
            if (email == null)
            {
                return null;
            }

            return _usersRepository.GetUserAsync(email);
        }

        /*
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
        */
    }
}