using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Seats4Me.API.Models.Input;
using Seats4Me.API.Models.Output;
using Seats4Me.API.Repositories;
using Seats4Me.API.Services;

namespace Seats4Me.API.Controllers
{
    [Route("api/shows/{showId}/[controller]")]
    public class TimeSlotsController : Controller
    {
        private readonly ITimeSlotsService _service;
        private readonly IShowsService _showsService;

        public TimeSlotsController(ITimeSlotsService service, IShowsService showsService)
        {
            _service = service;
            _showsService = showsService;
        }

        // GET: api/timeSlots/5
        [HttpGet]
        [ProducesResponseType(typeof(List<TimeSlotOutputModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAsync(int showId)
        {
            if (!await _showsService.ShowExistsAsync(showId))
                return NotFound();

            return Ok(await _service.GetAsync(showId));
        }
        // GET: api/timeSlots/5
        [HttpGet("{timeSlotId}")]
        [ProducesResponseType(typeof(TimeSlotOutputModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAsync(int showId, int timeSlotId)
        {
            if (!await _showsService.ShowExistsAsync(showId))
                return NotFound();

            return Ok(await _service.GetAsync(showId, timeSlotId));
        }

        // GET: api/timeSlots/5/tickets
        [HttpGet("{timeSlotId}/tickets")]
        [ProducesResponseType(typeof(List<TicketOutputModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetTicketsAsync(int showId, int timeSlotId)
        {
            if (!await _showsService.ShowExistsAsync(showId))
                return NotFound();

            return Ok(await _service.GetTicketsAsync(showId, timeSlotId));
        }

        // POST api/admin/timeSlot
        [Authorize(Policy = "Contributor")]
        [HttpPost]
        [ProducesResponseType(typeof(TimeSlotOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> PostAsync(int showId, [FromBody]TimeSlotInputModel value)
        {
            if (!await _showsService.ShowExistsAsync(showId))
                return BadRequest();

            var result = await _service.AddAsync(showId, value);
            if (result == null)
                return BadRequest();
            return Ok(result);
        }

        // PUT api/admin/timeSlot/5
        [Authorize(Policy = "Contributor")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int showId, int id, [FromBody]TimeSlotInputModel value)
        {
            if (!await _showsService.ShowExistsAsync(showId))
                return BadRequest();

            var result = await _service.UpdateAsync(showId, id, value);
            if (result == null)
                return BadRequest();
            return Ok();
        }

        // DELETE api/admin/timeSlot/5
        [Authorize(Policy = "Contributor")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int showId, int id)
        {
            if (!await _showsService.ShowExistsAsync(showId))
                return BadRequest();

            if (!await _service.DeleteAsync(showId, id))
                return BadRequest();
            return Ok();
        }
    }
}