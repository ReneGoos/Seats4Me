using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Seats4Me.API.Model;
using Seats4Me.Data.Model;

namespace Seats4Me.API.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly UsersRepository _repository;
        private readonly IConfiguration _configuration;

        public LoginController(UsersRepository repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] Seats4MeUser user)
        {
            var authUser = await _repository.GetAuthenticatedUserAsync(user);
            if (authUser == null)
                return Unauthorized();
            return Ok(_repository.GetToken(authUser, _configuration["Signing:Key"], _configuration["Signing:Issuer"]));
        }
    }
}