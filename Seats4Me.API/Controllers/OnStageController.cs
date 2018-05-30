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
        public async Task<IEnumerable<Show>> GetAsync()
        {
            return await _repository.GetAsync();
        }

        // GET api/onstage/5
        [HttpGet("{id}")]
        public async Task<Show> GetAsync(int id)
        {
            return await _repository.GetAsync(id);
        }

        // GET api/onstage/5
        [HttpGet("calendar")]
        public async Task<IEnumerable<CalendarShows>> GetCalendarAsync()
        {
            return await _repository.GetCalendarAsync();
        }

        // POST api/onstage
        [HttpPost]
        public async Task<int> PostAsync([FromBody]Show value)
        {
            return await _repository.AddAsync(value);
        }

        // PUT api/onstage/5
        [HttpPut("{id}")]
        public async Task PutAsync(int id, [FromBody]Show value)
        {
        }

        // DELETE api/onstage/5
        [HttpDelete("{id}")]
        public async Task DeleteAsync(int id)
        {
        }
    }
}
