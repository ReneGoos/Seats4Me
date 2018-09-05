using System.Collections.Generic;
using System.Threading.Tasks;
using Seats4Me.API.Models.Input;
using Seats4Me.API.Models.Output;

namespace Seats4Me.API.Services
{
    public interface IShowsService
    {
        Task<List<ShowOutputModel>> GetAsync();
        Task<ShowOutputModel> GetAsync(int id);
        Task<ShowOutputModel> CreateAsync(ShowInputModel showInput);
        Task<ShowOutputModel> UpdateAsync(int id, ShowInputModel showInput);
    }
}