using System.Collections.Generic;
using System.Threading.Tasks;

using Seats4Me.API.Models.Input;
using Seats4Me.API.Models.Output;
using Seats4Me.API.Models.Search;

namespace Seats4Me.API.Services
{
    public interface IShowsService
    {
        Task<ShowOutputModel> AddAsync(ShowInputModel showInput);

        Task DeleteAsync(int id);

        Task<bool> ExistsAsync(int id);

        Task<IEnumerable<ShowOutputModel>> GetAsync(ShowSearchModel searchModel);

        Task<ShowOutputModel> GetAsync(int id);

        Task<string> GetExportAsync();

        Task<ShowOutputModel> UpdateAsync(int id, ShowInputModel showInput);
    }
}