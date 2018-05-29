using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Seats4Me.API.Model;
using Seats4Me.Data.Model;

namespace Seats4Me.API.Controllers
{
    [Route("api/[controller]")]
    public class OnStageController : Controller
    {
        // GET api/values
        [HttpGet]
        public async Task<IEnumerable<Show>> GetAsync()
        {
            var showsRepository = new ShowsRepository();
            return await showsRepository.GetAsync();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<string> GetAsync(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public async Task PostAsync([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task PutAsync(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task DeleteAsync(int id)
        {
        }
    }
}
