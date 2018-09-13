using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Seats4Me.API.Repositories;
using Seats4Me.Data.Model;

namespace Seats4Me.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = "Owner")]
    public class ShowsExportController : Controller
    {
        private readonly IShowsRepository _repository;

        public ShowsExportController(IShowsRepository repository)
        {
            _repository = repository;
        }

        //GET api/admin/show/export
        [HttpGet("export")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Export()
        {
            var showsExport = await _repository.GetExport();
            if (showsExport == null)
                return NotFound();

            return Ok(showsExport);
        }
    }
}