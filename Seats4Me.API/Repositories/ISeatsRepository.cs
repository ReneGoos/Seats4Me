using System.Collections.Generic;
using System.Threading.Tasks;

using Seats4Me.Data.Model;

namespace Seats4Me.API.Repositories
{
    public interface ISeatsRepository
    {
        Task<List<Seat>> GetAsync();

        Task<Seat> GetAsync(int id);

        Task<Seat> GetAsync(int row, int chair);
    }
}