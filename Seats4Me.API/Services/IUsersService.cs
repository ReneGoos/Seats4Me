using System.Threading.Tasks;

using Seats4Me.API.Models.Input;

namespace Seats4Me.API.Services
{
    public interface IUsersService
    {
        Task<string> GetTokenAsync(LoginInputModel login);
    }
}