using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Seats4Me.Data.Model;

namespace Seats4Me.API.Repositories
{
    public interface IShowsRepository
    {
        Task<Show> AddAsync(Show show);

        Task DeleteAsync(Show show);

        Task<List<Show>> GetAsync(DateTime? firstDay, DateTime? lastDay, bool onlyPromo);

        Task<Show> GetAsync(int showId);

        Task<string> GetExportAsync();

        Task<Show> UpdateAsync(Show show);
    }
}