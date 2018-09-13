using System.Collections.Generic;
using System.Threading.Tasks;
using Seats4Me.API.Models;
using Seats4Me.API.Models.Result;
using Seats4Me.Data.Model;

namespace Seats4Me.API.Repositories
{
    public interface ITimeSlotSeatsRepository
    {
        Task<TimeSlotSeat> AddAsync(TimeSlotSeat value);
        Task<bool> DeleteAsync(int ticketId);
        Task<IEnumerable<TimeSlotSeat>> GetAsync(int? showId = null, int? timeSlotId = null, int? userId = null);
        Task<TimeSlotSeat> GetAsync(int ticketId);
        Task<TimeSlotSeat> UpdateAsync(TimeSlotSeat value);
        Task<IEnumerable<TicketResult>> GetTicketsByTimeSlotAsync(int timeSlotId);
    }
}