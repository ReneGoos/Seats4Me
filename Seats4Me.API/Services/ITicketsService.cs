using System.Collections.Generic;
using System.Threading.Tasks;
using Seats4Me.API.Models.Input;
using Seats4Me.API.Models.Output;

namespace Seats4Me.API.Services
{
    public interface ITicketsService
    {
        Task<List<TicketOutputModel>> GetAsync(string email = null);
        Task<TicketOutputModel> GetAsync(int id);
        Task<TicketOutputModel> AddAsync(TicketInputModel ticketInput, string email);
        Task<TicketOutputModel> UpdateAsync(int id, TicketInputModel ticketInput, string email);
        Task<bool> DeleteAsync(int id, string email);
    }
}