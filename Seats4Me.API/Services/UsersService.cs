using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;

using Seats4Me.API.Models.Input;
using Seats4Me.API.Repositories;

namespace Seats4Me.API.Services
{
    public class UsersService : IUsersService
    {
        private readonly IConfiguration _configuration;
        private readonly IUsersRepository _repository;

        public UsersService(IUsersRepository repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }

        public async Task<string> GetTokenAsync(LoginInputModel login)
        {
            var authUser = await _repository.GetAuthenticatedUserAsync(login.Email, login.Password);

            return authUser == null
                       ? null
                       : _repository.GetToken(authUser, _configuration["Signing:Key"], _configuration["Signing:Issuer"]);
        }
    }
}