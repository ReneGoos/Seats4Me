using System.Collections.Generic;
using System.Threading.Tasks;
using Seats4Me.API.Models.Input;
using Seats4Me.API.Models.Output;

namespace Seats4Me.API.Services
{
    public interface ITimeSlotsService
    {
        Task<List<TimeSlotOutputModel>> GetAsync(int showId);
        Task<TimeSlotOutputModel> GetAsync(int showId, int id);
        Task<TimeSlotOutputModel> AddAsync(int showId, TimeSlotInputModel timeSlotInput);
        Task<TimeSlotOutputModel> UpdateAsync(int showId, int id, TimeSlotInputModel timeSlotInput);
        Task<bool> DeleteAsync(int showId, int id);
        Task<TicketOutputModel> GetTicketsAsync(int showId, int timeSlotId);
    }
}