using System.Threading.Tasks;
using Seats4Me.Data.Model;

namespace Seats4Me.API.Repositories
{
    public interface IUsersRepository
    {
        Task<Seats4MeUser> GetUserAsync(string email);
        Task<Seats4MeUser> GetAuthenticatedUserAsync(string email, string password);
        string GetToken(Seats4MeUser user, string jwtKey, string jwtIssuer);
    }
}