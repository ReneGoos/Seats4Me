using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Seats4Me.API.Models.Output;
using Seats4Me.API.Models.Input;
using Seats4Me.API.Repositories;
using Seats4Me.API.Services;
using Seats4Me.Data.Common;

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

        // GET api/shows
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var shows = await _showsService.GetAsync();

            var showModels = Mapper.Map<IEnumerable<ShowOutputModel>>(shows);

            return Ok(showModels);
        }

        // GET api/shows/5
        [HttpGet("{id}", Name = "GetAsync")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var show = await _showsService.GetAsync(id);

            var showModel = Mapper.Map<ShowOutputModel>(show);

            return Ok(showModel);
        }

        // POST api/show
        [Authorize(Policy = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]ShowInputModel value)
        {
            if (value == null)
                return BadRequest();

            var result = await _showsService.CreateAsync(value);
            if (result == null)
                return BadRequest();
            //return Ok(result);

            return CreatedAtRoute("GetAsync", new { id = result.Id }, result);
        }

        // PUT api/show/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody]ShowInputModel value)
        {
            if (value == null)
                return BadRequest();

            var result = await _showsService.UpdateAsync(id, value);
            if (result == null)
                return BadRequest();
            return Ok(result);
        }

        /*
        // DELETE api/admin/show/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (!await _showsService.DeleteAsync(id))
                return BadRequest(_showsService.LastErrorMessage);
            return Ok();
        }

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
