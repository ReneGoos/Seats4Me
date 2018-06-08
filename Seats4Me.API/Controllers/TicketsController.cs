using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Seats4Me.API.Data;
using Seats4Me.API.Model;

namespace Seats4Me.API.Controllers
{
    [Route("api/[controller]")]
    public class TicketsController : Controller
    {
        private readonly TicketsRepository _repository;

        public TicketsController(TicketsRepository repository)
        {
            _repository = repository;
        }

        // GET: api/tickets/5
        [HttpGet("{timeSlotId}")]
        public async Task<IActionResult> GetAsync(int timeSlotId)
        {
            return Ok(await _repository.GetAsync(timeSlotId));
        }

        // GET: api/tickets/email@email.com
        [Authorize(Policy="Customer")]
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var email = User.Claims.First(c => c.Type.Equals(JwtRegisteredClaimNames.Email)).Value;
            return Ok(await _repository.GetMyTicketsAsync(email));
        }

        // POST: api/tickets
        [Authorize(Policy = "Customer")]
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]Ticket value)
        {
            var email = User.Claims.First(c => c.Type.Equals(JwtRegisteredClaimNames.Email)).Value;
            if (!value.Email.Equals(email))
                return BadRequest(_repository.LastErrorMessage);

            var timeSlotSeatId = await _repository.AddAsync(value);
            if (timeSlotSeatId <= 0)
                return BadRequest(_repository.LastErrorMessage);
            return Ok(timeSlotSeatId);
        }

        // PUT: api/tickets/5
        [Authorize(Policy = "Customer")]
        [HttpPut("{timeSlotId}")]
        public async Task<IActionResult> PutAsync(int timeSlotSeatId, [FromBody]Ticket value)
        {
            var email = User.Claims.First(c => c.Type.Equals(JwtRegisteredClaimNames.Email)).Value;
            if (!_repository.ValidTicketUser(timeSlotSeatId, email))
                return BadRequest(_repository.LastErrorMessage);

            value.TimeSlotSeatId = timeSlotSeatId;
            if (!await _repository.UpdateAsync(value))
                return BadRequest(_repository.LastErrorMessage);
            return Ok();
        }

        // DELETE: api/ApiWithActions/5
        [Authorize(Policy = "Customer")]
        [HttpDelete("{timeSlotId}")]
        public async Task<IActionResult> DeleteAsync(int timeSlotSeatId)
        {
            var email = User.Claims.First(c => c.Type.Equals(JwtRegisteredClaimNames.Email)).Value;
            if (!_repository.ValidTicketUser(timeSlotSeatId, email))
                return BadRequest(_repository.LastErrorMessage);

            if (!await _repository.DeleteAsync(timeSlotSeatId))
                return BadRequest(_repository.LastErrorMessage);
            return Ok();
        }
    }
}

