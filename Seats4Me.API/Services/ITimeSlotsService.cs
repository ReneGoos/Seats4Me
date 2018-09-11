using System.Collections.Generic;
using System.Threading.Tasks;
using Seats4Me.API.Models.Input;
using Seats4Me.API.Models.Output;

namespace Seats4Me.API.Services
{
    public interface ITimeSlotsService
    {
        Task<List<TimeSlotOutputModel>> GetAsync();
        Task<TimeSlotOutputModel> GetAsync(int id);
        Task<TimeSlotOutputModel> AddAsync(TimeSlotInputModel timeSlotInput);
        Task<TimeSlotOutputModel> UpdateAsync(int id, TimeSlotInputModel timeSlotInput);
        Task<bool> DeleteAsync(int id);
        Task<TicketOutputModel> GetTicketsAsync(int timeSlotId);
    }
}