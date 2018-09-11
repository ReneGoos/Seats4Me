using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Seats4Me.API.Models.Result;
using Seats4Me.Data.Model;

namespace Seats4Me.API.Repositories
{
    public interface IShowsRepository
    {
        Task<Show> AddAsync(Show value);
        Task<bool> DeleteAsync(int showId);
        Task<List<Show>> GetAsync(DateTime? firstDay, DateTime? lastDay, bool onlyPromo);
        Task<Show> GetAsync(int showId);
        Task<string> GetExport();
        Task<Show> UpdateAsync(Show value);
    }
}