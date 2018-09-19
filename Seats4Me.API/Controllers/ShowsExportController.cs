using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Seats4Me.API.Services;

namespace Seats4Me.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = "Owner")]
    public class ShowsExportController : Controller
    {
        private readonly IShowsService _showsService;

        public ShowsExportController(IShowsService showsService)
        {
            _showsService = showsService;
        }

        //GET api/admin/show/export
        [HttpGet]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAsync()
        {
            var showsExport = await _showsService.GetExportAsync();

            if (showsExport == null)
            {
                return NotFound();
            }

            return Ok(showsExport);
        }
    }
}