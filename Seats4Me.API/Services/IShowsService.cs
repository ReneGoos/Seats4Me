using System.Collections.Generic;
using System.Threading.Tasks;
using Seats4Me.API.Models.Input;
using Seats4Me.API.Models.Output;
using Seats4Me.API.Models.Search;

namespace Seats4Me.API.Services
{
    public interface IShowsService
    {
        Task<bool> ShowExistsAsync(int id);
        Task<IEnumerable<ShowOutputModel>> GetAsync(ShowSearchModel searchModel);
        Task<ShowOutputModel> GetAsync(int id);
        Task<ShowOutputModel> AddAsync(ShowInputModel showInput);
        Task<ShowOutputModel> UpdateAsync(int id, ShowInputModel showInput);
        Task<bool> DeleteAsync(int id);
    }
}