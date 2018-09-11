using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Seats4Me.API.Models.Input;
using Seats4Me.API.Repositories;
using Seats4Me.API.Services;

namespace Seats4Me.API.Controllers
{
    [Route("api/[controller]")]
    public class TimeSlotsController : Controller
    {
        private readonly ITimeSlotsService _service;

        public TimeSlotsController(ITimeSlotsService service)
        {
            _service = service;
        }

        // GET: api/timeSlots/5
        [HttpGet("{timeSlotId}")]
        public async Task<IActionResult> GetAsync(int timeSlotId)
        {
            return Ok(await _service.GetAsync(timeSlotId));
        }

        // GET: api/timeSlots/5/tickets
        [HttpGet("{timeSlotId}/tickets")]
        public async Task<IActionResult> GetTicketsAsync(int timeSlotId)
        {
            return Ok(await _service.GetTicketsAsync(timeSlotId));
        }

        // POST api/admin/timeSlot
        [Authorize(Policy = "Contributor")]
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]TimeSlotInputModel value)
        {
            var result = await _service.AddAsync(value);
            if (result == null)
                return BadRequest();
            return Ok(result);
        }

        // PUT api/admin/timeSlot/5
        [Authorize(Policy = "Contributor")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody]TimeSlotInputModel value)
        {
            var result = await _service.UpdateAsync(id, value);
            if (result == null)
                return BadRequest();
            return Ok();
        }

        // DELETE api/admin/timeSlot/5
        [Authorize(Policy = "Contributor")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (!await _service.DeleteAsync(id))
                return BadRequest();
            return Ok();
        }
    }
}