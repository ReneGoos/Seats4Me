using System.Collections.Generic;
using System.Threading.Tasks;

using Seats4Me.API.Models.Result;
using Seats4Me.Data.Model;

namespace Seats4Me.API.Repositories
{
    public interface ITimeSlotSeatsRepository
    {
        Task<TimeSlotSeat> AddAsync(TimeSlotSeat timeSlotSeat);

        Task DeleteAsync(TimeSlotSeat timeSlotSeat);

        Task<TimeSlotSeat> GetAsync(int ticketId);

        Task<TimeSlotSeat> GetAsync(int timeSlotId, int row, int chair);

        Task<TicketResult> GetTicketAsync(int ticketId);

        Task<List<TicketResult>> GetTicketsByTimeSlotAsync(int showId, int timeSlotId);

        Task<List<TicketResult>> GetTicketsByUserAsync(int userId);

        Task<TimeSlotSeat> UpdateAsync(TimeSlotSeat timeSlotSeat);
    }
}