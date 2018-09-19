using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Seats4Me.API.Models.Input;
using Seats4Me.API.Models.Output;
using Seats4Me.API.Models.Search;
using Seats4Me.API.Services;

namespace Seats4Me.API.Controllers
{
    [Route("api/[controller]")]
    public class ShowsController : Controller
    {
        private readonly IShowsService _showsService;

        public ShowsController(IShowsService showsService)
        {
            _showsService = showsService;
        }

        [Authorize(Policy = "Contributor")]

        // DELETE api/admin/show/5
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _showsService.DeleteAsync(id);

            return NoContent();
        }

        // GET api/shows
        [HttpGet]
        [ProducesResponseType(typeof(List<ShowOutputModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAsync(ShowSearchModel searchModel)
        {
            var showModels = await _showsService.GetAsync(searchModel);

            return Ok(showModels);
        }

        // GET api/shows/5
        [HttpGet("{id}", Name = "GetAsync")]
        [ProducesResponseType(typeof(ShowOutputModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAsync(int id)
        {
            var showModel = await _showsService.GetAsync(id);

            if (showModel == null)
            {
                return NotFound();
            }

            return Ok(showModel);
        }

        // POST api/show
        [Authorize(Policy = "Contributor")]
        [HttpPost]
        [ProducesResponseType(typeof(ShowOutputModel), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PostAsync([FromBody] ShowInputModel value)
        {
            if (value == null)
            {
                return BadRequest("Invalid input");
            }

            var result = await _showsService.AddAsync(value);

            if (result == null)
            {
                return BadRequest("Show not inserted");
            }

            return CreatedAtRoute("GetAsync",
                                  new
                                  {
                                      id = result.Id
                                  },
                                  result);
        }

        // PUT api/show/5
        [Authorize(Policy = "Contributor")]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ShowOutputModel), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PutAsync(int id, [FromBody] ShowInputModel value)
        {
            if (value == null)
            {
                return BadRequest("Invalid input");
            }

            var result = await _showsService.UpdateAsync(id, value);

            if (result == null)
            {
                return BadRequest($"Update of show {id} failed");
            }

            return Ok(result);
        }

        /*
        // GET api/shows/week
        [HttpGet("week/{week:int}/{year:int?}")]
        public async Task<IActionResult> GetWeekAsync(int week, int year = 0)
        {
            if (week < 1 || week > new DateTime(year == 0 ? DateTime.Today.Year : year, 12, 31).Week())
                return BadRequest(string.Format("Invalid week number {0}", week));
            return Ok(await _showsService.GetOnPeriodAsync(week: week, year: year));
        }

        // GET api/shows/week
        [HttpGet("month/{month:int}/{year:int?}")]
        public async Task<IActionResult> GetMonthAsync(int month, int year = 0)
        {
            if (month < 1 || month > 12)
                return BadRequest(string.Format("Invalid month {0}", month));
            return Ok(await _showsService.GetOnPeriodAsync(month: month, year: year));
        }

        // GET api/shows/week
        [HttpGet("year/{year:int?}")]
        public async Task<IActionResult> GetYearAsync(int year = 0)
        {
            if (year >= 100 && year < 1900 || year < 0)
                return BadRequest(string.Format("Invalid year number {0}", year));
            return Ok(await _showsService.GetOnPeriodAsync(year: year));
        }

        // GET api/shows/week
        [HttpGet("promo")]
        public async Task<IActionResult> GetPromoAsync()
        {
            return Ok(await _showsService.GetPromotionsAsync());
        }
        */
    }
}