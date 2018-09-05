using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Seats4Me.API.Repositories;
using Seats4Me.Data.Model;

namespace Seats4Me.API.Controllers
{
    [Route("api/admin/timeSlot")]
    [Authorize(Policy = "Administrator")]
    public class AdminTimeSlotController : Controller
    {
        private readonly TimeSlotsRepository _repository;

        public AdminTimeSlotController(TimeSlotsRepository repository)
        {
            _repository = repository;
        }

        // POST api/admin/timeSlot
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]TimeSlot value)
        {
            var result = await _repository.AddAsync(value);
            if (result <= 0)
                return BadRequest(_repository.LastErrorMessage);
            return Ok(result);
        }

        // PUT api/admin/timeSlot/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody]TimeSlot value)
        {
            value.Id = id;
            if (!await _repository.UpdateAsync(value))
                return BadRequest(_repository.LastErrorMessage);
            return Ok();
        }

        // DELETE api/admin/timeSlot/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (!await _repository.DeleteAsync(id))
                return BadRequest(_repository.LastErrorMessage);
            return Ok();
        }
    }
}