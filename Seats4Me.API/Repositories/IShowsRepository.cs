using System.Collections.Generic;
using System.Threading.Tasks;
using Seats4Me.API.Models;
using Seats4Me.Data.Model;

namespace Seats4Me.API.Repositories
{
    public interface IShowsRepository
    {
        Task<Show> AddAsync(Show value);
        Task<bool> DeleteAsync(int showId);
        Task<List<Show>> GetAsync();
        Task<Show> GetAsync(int showId);
        Task<string> GetExport();
        Task<IEnumerable<TimeSlotShow>> GetOnPeriodAsync(int week = 0, int month = 0, int year = 0);
        Task<IEnumerable<TimeSlotShow>> GetPromotionsAsync();
        Task<Show> UpdateAsync(Show value);
    }
}