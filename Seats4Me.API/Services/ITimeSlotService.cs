using System.Collections.Generic;
using System.Threading.Tasks;
using Seats4Me.API.Models.Input;
using Seats4Me.API.Models.Output;

namespace Seats4Me.API.Services
{
    public interface ITimeSlotService
    {
        Task<List<ShowOutputModel>> GetAsync();
        Task<ShowOutputModel> GetAsync(int id);
        Task<ShowOutputModel> CreateAsync(TimeSlotInputModel timeSlotInput);
        Task<ShowOutputModel> UpdateAsync(int id, TimeSlotInputModel timeSlotInput);
        Task<bool> DeleteAsync(int id);
    }
}