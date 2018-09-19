using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Seats4Me.API.Models.Input;
using Seats4Me.API.Models.Output;
using Seats4Me.API.Services;

namespace Seats4Me.API.Controllers
{
    [Route("api/[controller]")]
    public class TicketsController : Controller
    {
        private readonly IShowsService _showsService;
        private readonly ITicketsService _ticketsService;
        private readonly ITimeSlotsService _timeSlotsService;

        public TicketsController(ITicketsService ticketsService, ITimeSlotsService timeSlotsService, IShowsService showsService)
        {
            _ticketsService = ticketsService;
            _timeSlotsService = timeSlotsService;
            _showsService = showsService;
        }

        [Authorize(Policy = "Customer")]
        [HttpDelete("{ticketId}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteAsync(int ticketId)
        {
            var email = User.Claims.First(c => c.Type.Equals(ClaimTypes.Email)).Value;

            await _ticketsService.DeleteAsync(ticketId, email);

            return NoContent();
        }

        [Authorize(Policy = "Customer")]
        [HttpGet]
        [ProducesResponseType(typeof(List<TicketOutputModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAsync()
        {
            var email = User.Claims.First(c => c.Type.Equals(ClaimTypes.Email)).Value;

            return Ok(await _ticketsService.GetTicketsByUserAsync(email));
        }

        [Authorize(Policy = "Contributor")]
        [HttpGet("/api/Shows/{showId}/Timeslots/{timeSlotId}/Tickets/{id}")]
        [ProducesResponseType(typeof(TicketOutputModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAsync(int showId, int timeSlotId, int id)
        {
            if (!await _showsService.ExistsAsync(showId))
            {
                return NotFound();
            }

            return Ok(await _ticketsService.GetTicketAsync(id));
        }

        [Authorize(Policy = "Contributor")]
        [HttpGet("/api/Shows/{showId}/Timeslots/{timeSlotId}/Tickets")]
        [ProducesResponseType(typeof(List<TicketOutputModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetTicketsAsync(int showId, int timeSlotId)
        {
            if (!await _showsService.ExistsAsync(showId))
            {
                return NotFound();
            }

            return Ok(await _ticketsService.GetTicketsByTimeSlotAsync(showId, timeSlotId));
        }

        [Authorize(Policy = "Customer")]
        [HttpPost("/api/Shows/{showId}/Timeslots/{timeSlotId}/Tickets")]
        [ProducesResponseType(typeof(TicketOutputModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PostAsync(int showId, int timeSlotId, [FromBody] TicketInputModel value)
        {
            if (!await _showsService.ExistsAsync(showId))
            {
                return BadRequest($"Invalid show {showId}");
            }

            if (!await _timeSlotsService.ExistsAsync(showId, timeSlotId))
            {
                return BadRequest($"Invalid show {showId} and timeslot {timeSlotId}");
            }

            var email = User.Claims.First(c => c.Type.Equals(ClaimTypes.Email)).Value;

            var ticket = await _ticketsService.AddAsync(showId, timeSlotId, value, email);

            if (ticket == null)
            {
                return BadRequest("Ticket not inserted");
            }

            return Ok(ticket);
        }

        [Authorize(Policy = "Customer")]
        [HttpPut("{ticketId}")]
        [ProducesResponseType(typeof(TicketOutputModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PutAsync(int ticketId, [FromBody] TicketInputModel value)
        {
            var email = User.Claims.First(c => c.Type.Equals(ClaimTypes.Email)).Value;

            var ticket = await _ticketsService.UpdateAsync(ticketId, value, email);

            if (ticket == null)
            {
                return BadRequest("Ticket not updated");
            }

            return Ok(ticket);
        }
    }
}