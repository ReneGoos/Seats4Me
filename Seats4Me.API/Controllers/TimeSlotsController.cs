using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Seats4Me.API.Models.Input;
using Seats4Me.API.Models.Output;
using Seats4Me.API.Services;

namespace Seats4Me.API.Controllers
{
    [Route("api/shows/{showId}/[controller]")]
    public class TimeSlotsController : Controller
    {
        private readonly IShowsService _showsService;
        private readonly ITimeSlotsService _timeSlotsService;

        public TimeSlotsController(ITimeSlotsService timeSlotsService, IShowsService showsService)
        {
            _timeSlotsService = timeSlotsService;
            _showsService = showsService;
        }

        // DELETE api/admin/timeSlot/5
        [Authorize(Policy = "Contributor")]
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteAsync(int showId, int id)
        {
            if (!await _showsService.ExistsAsync(showId))
            {
                return NoContent();
            }

            await _timeSlotsService.DeleteAsync(showId, id);

            return NoContent();
        }

        // GET: api/timeSlots/5
        [HttpGet]
        [ProducesResponseType(typeof(List<TimeSlotOutputModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAsync(int showId)
        {
            if (!await _showsService.ExistsAsync(showId))
            {
                return NotFound();
            }

            return Ok(await _timeSlotsService.GetAsync(showId));
        }

        // GET: api/timeSlots/5
        [HttpGet("{timeSlotId}")]
        [ProducesResponseType(typeof(TimeSlotOutputModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAsync(int showId, int timeSlotId)
        {
            if (!await _showsService.ExistsAsync(showId))
            {
                return NotFound();
            }

            var timeSlot = await _timeSlotsService.GetAsync(showId, timeSlotId);

            if (timeSlot == null)
            {
                return NotFound();
            }

            return Ok(timeSlot);
        }

        // POST api/admin/timeSlot
        [Authorize(Policy = "Contributor")]
        [HttpPost]
        [ProducesResponseType(typeof(TimeSlotOutputModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PostAsync(int showId, [FromBody] TimeSlotInputModel value)
        {
            if (!await _showsService.ExistsAsync(showId))
            {
                return BadRequest($"Invalid show {showId}");
            }

            var result = await _timeSlotsService.AddAsync(showId, value);

            if (result == null)
            {
                return BadRequest("Timeslot not inserted");
            }

            return Ok(result);
        }

        // PUT api/admin/timeSlot/5
        [Authorize(Policy = "Contributor")]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(TimeSlotOutputModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PutAsync(int showId, int id, [FromBody] TimeSlotInputModel value)
        {
            if (!await _showsService.ExistsAsync(showId))
            {
                return BadRequest($"Invalid show {showId}");
            }

            var result = await _timeSlotsService.UpdateAsync(showId, id, value);

            if (result == null)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}