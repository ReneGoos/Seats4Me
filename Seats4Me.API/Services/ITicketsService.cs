using System.Collections.Generic;
using System.Threading.Tasks;

using Seats4Me.API.Models.Input;
using Seats4Me.API.Models.Output;

namespace Seats4Me.API.Services
{
    public interface ITicketsService
    {
        Task<TicketOutputModel> AddAsync(int showId, int timeSlotId, TicketInputModel ticketInput, string email);

        Task DeleteAsync(int id, string email);

        Task<TicketOutputModel> GetTicketAsync(int id);

        Task<List<TicketOutputModel>> GetTicketsByTimeSlotAsync(int showId, int timeSlotId);

        Task<List<TicketOutputModel>> GetTicketsByUserAsync(string email);

        Task<TicketOutputModel> UpdateAsync(int id, TicketInputModel ticketInput, string email);
    }
}