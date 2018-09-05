using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Seats4Me.API.Repositories;
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

        //GET api/admin/show/export
        [HttpGet("export")]
        public async Task<string> Export()
        {
            var showsExport = await _repository.GetExport();
            return showsExport;
        }
    }
}