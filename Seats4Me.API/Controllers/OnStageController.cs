using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Seats4Me.API.Data;
using Seats4Me.API.Model;
using Seats4Me.Data.Model;

namespace Seats4Me.API.Controllers
{
    [Route("api/[controller]")]
    public class OnStageController : Controller
    {
        private ShowsRepository _repository;

        public OnStageController(ShowsRepository repository)
        {
            _repository = repository;
        }

        // GET api/onstage
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _repository.GetAsync());
        }

        // GET api/onstage/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            return Ok(await _repository.GetAsync(id));
        }

        // GET api/onstage/calendar
        [HttpGet("calendar")]
        public async Task<IActionResult> GetCalendarAsync()
        {
            return Ok(await _repository.GetCalendarAsync());
        }

        // POST api/onstage
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]Show value)
        {
            var result = await _repository.AddAsync(value);
            if (result <= 0)
                return BadRequest(_repository.LastErrorMessage);
            return Ok(result);
        }

        // PUT api/onstage/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody]Show value)
        {
            value.ShowId = id;
            if (!await _repository.UpdateAsync(value))
                return BadRequest(_repository.LastErrorMessage);
            return Ok();
        }

        // DELETE api/onstage/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (!await _repository.DeleteAsync(id))
                return BadRequest(_repository.LastErrorMessage);
            return Ok();
        }
    }
}
