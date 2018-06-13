using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Seats4Me.API.Model;
using Seats4Me.Data.Model;

namespace Seats4Me.API.Controllers
{
    [Route("api/admin/show")]
    [Authorize(Policy = "Administrator")]
    public class AdminShowController : Controller
    {
        private readonly ShowsRepository _repository;

        public AdminShowController(ShowsRepository repository)
        {
            _repository = repository;
        }

        // POST api/admin/show
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]Show value)
        {
            var result = await _repository.AddAsync(value);
            if (result <= 0)
                return BadRequest(_repository.LastErrorMessage);
            return Ok(result);
        }

        // PUT api/admin/show/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody]Show value)
        {
            value.Id = id;
            if (!await _repository.UpdateAsync(value))
                return BadRequest(_repository.LastErrorMessage);
            return Ok();
        }

        // DELETE api/admin/show/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (!await _repository.DeleteAsync(id))
                return BadRequest(_repository.LastErrorMessage);
            return Ok();
        }

        //GET api/admin/show/export
        [HttpGet("export")]
        public async Task<string> Export()
        {
            var showsExport = await _repository.GetExport();
            return showsExport;
        }
    }
}