using System.Threading.Tasks;
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
        [HttpGet("{timeslotId}")]
        public async Task<IActionResult> GetAsync(int timeslotId)
        {
            return Ok(await _repository.GetAsync(timeslotId));
        }

        // GET: api/tickets/email@email.com
        [HttpGet("my/{email:alpha}")]
        public async Task<IActionResult> GetAsync(string email)
        {
            return Ok(await _repository.GetMyTicketsAsync(email));
        }

        // POST: api/tickets
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]Ticket value)
        {
            var timeslotId = await _repository.AddAsync(value);
            if (timeslotId <= 0)
                return BadRequest(_repository.LastErrorMessage);
            return Ok(timeslotId);
        }
        
        // PUT: api/tickets/5
        [HttpPut("{timeslotId}")]
        public async Task<IActionResult> PutAsync(int timeslotId, [FromBody]Ticket value)
        {
            value.TimeSlotId = timeslotId;
            if (!await _repository.UpdateAsync(value))
                return BadRequest(_repository.LastErrorMessage);
            return Ok();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{timeslotId}")]
        public async Task<IActionResult> DeleteAsync(int timeslotId)
        {
            if (!await _repository.DeleteAsync(timeslotId))
                return BadRequest(_repository.LastErrorMessage);
            return Ok();
        }
    }
}
