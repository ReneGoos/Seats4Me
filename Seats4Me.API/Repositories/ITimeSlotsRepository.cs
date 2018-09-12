using System.Collections.Generic;
using System.Threading.Tasks;
using Seats4Me.Data.Model;

namespace Seats4Me.API.Repositories
{
    public interface ITimeSlotsRepository
    {
        Task<List<TimeSlot>> GetAsync(int showId);
        Task<TimeSlot> GetAsync(int showId, int id);
        Task<TimeSlot> AddAsync(TimeSlot value);
        Task<bool> DeleteAsync(int timeSlotId);
        Task<TimeSlot> UpdateAsync(TimeSlot value);
    }
}