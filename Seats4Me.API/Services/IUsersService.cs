using System.Threading.Tasks;
using Seats4Me.API.Models.Input;
using Seats4Me.Data.Model;

namespace Seats4Me.API.Services
{
    public interface IUsersService
    {
        Task<string> GetTokenAsync(LoginInputModel login);
    }
}