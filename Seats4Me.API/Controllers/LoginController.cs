using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Seats4Me.API.Repositories;
using Seats4Me.Data.Model;
using Seats4Me.API.Models.Input;
using Seats4Me.API.Services;

namespace Seats4Me.API.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly IUsersService _service;

        public LoginController(IUsersService service)
        {
            _service = service;
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PostAsync([FromBody] LoginInputModel user)
        {
            var token = await _service.GetTokenAsync(user);
            if (token == null)
                return BadRequest();

            return Ok(token);
        }
    }
}