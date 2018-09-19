using System.Collections.Generic;
using System.Threading.Tasks;

using Seats4Me.Data.Model;

namespace Seats4Me.API.Repositories
{
    public interface ITimeSlotsRepository
    {
        Task<TimeSlot> AddAsync(TimeSlot timeSlot);

        Task DeleteAsync(TimeSlot timeSlot);

        Task<List<TimeSlot>> GetAsync(int showId);

        Task<TimeSlot> GetAsync(int showId, int id);

        Task<TimeSlot> UpdateAsync(TimeSlot timeSlot);
    }
}