using System.Collections.Generic;
using System.Threading.Tasks;

using Seats4Me.API.Models.Input;
using Seats4Me.API.Models.Output;

namespace Seats4Me.API.Services
{
    public interface ITimeSlotsService
    {
        Task<TimeSlotOutputModel> AddAsync(int showId, TimeSlotInputModel timeSlotInput);

        Task DeleteAsync(int showId, int id);

        Task<bool> ExistsAsync(int showId, int id);

        Task<List<TimeSlotOutputModel>> GetAsync(int showId);

        Task<TimeSlotOutputModel> GetAsync(int showId, int id);

        Task<decimal> GetPriceAsync(int showId, int id, bool discount);

        Task<TimeSlotOutputModel> UpdateAsync(int showId, int id, TimeSlotInputModel timeSlotInput);
    }
}