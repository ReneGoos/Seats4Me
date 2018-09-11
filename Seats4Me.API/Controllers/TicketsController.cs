﻿using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Seats4Me.API.Models.Input;
using Seats4Me.API.Services;

namespace Seats4Me.API.Controllers
{
    [Route("api/[controller]")]
    public class TicketsController : Controller
    {
        private readonly ITicketsService _service;

        public TicketsController(ITicketsService service)
        {
            _service = service;
        }

        // GET: api/tickets/email@email.com
        [Authorize(Policy="Customer")]
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var email = User.Claims.First(c => c.Type.Equals(ClaimTypes.Email)).Value;
            return Ok(await _service.GetAsync(email));
        }

        // POST: api/tickets
        [Authorize(Policy = "Customer")]
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]TicketInputModel value)
        {
            var email = User.Claims.First(c => c.Type.Equals(ClaimTypes.Email)).Value;

            var ticket = await _service.AddAsync(value, email);
            if (ticket == null)
                return BadRequest();

            return Ok(ticket);
        }

        // PUT: api/tickets/5
        [Authorize(Policy = "Customer")]
        [HttpPut("{ticketId}")]
        public async Task<IActionResult> PutAsync(int ticketId, [FromBody]TicketInputModel value)
        {
            var email = User.Claims.First(c => c.Type.Equals(ClaimTypes.Email)).Value;

            var ticket = await _service.UpdateAsync(ticketId, value, email);
            if (ticket == null)
                return BadRequest();

            return Ok();
        }

        // DELETE: api/ApiWithActions/5
        [Authorize(Policy = "Customer")]
        [HttpDelete("{ticketId}")]
        public async Task<IActionResult> DeleteAsync(int ticketId)
        {
            var email = User.Claims.First(c => c.Type.Equals(ClaimTypes.Email)).Value;

            if (!await _service.DeleteAsync(ticketId, email))
                return BadRequest();

            return Ok();
        }
    }
}

