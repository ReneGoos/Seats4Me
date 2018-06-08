using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Seats4Me.API.Model;
using Seats4Me.Data.Common;
using Seats4Me.Data.Model;

namespace Seats4Me.API.Controllers
{
    [Route("api/[controller]")]
    public class OnStageController : Controller
    {
        private readonly ShowsRepository _repository;

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

        // GET api/onstage/week
        [HttpGet("week/{week:int}/{year:int}")]
        public async Task<IActionResult> GetWeekAsync(int week, int year)
        {
            if (week < 1 || week > new DateTime(year, 12, 31).Week())
                return BadRequest(string.Format("Invalid week number {0}", week));
            return Ok(await _repository.GetOnPeriodAsync(week: week, year: year));
        }

        // GET api/onstage/week
        [HttpGet("month/{month:int}/{year:int}")]
        public async Task<IActionResult> GetMonthAsync(int month, int year)
        {
            if (month < 1 || month > 12)
                return BadRequest(string.Format("Invalid month {0}", month));
            return Ok(await _repository.GetOnPeriodAsync(month: month, year: year));
        }

        // GET api/onstage/week
        [HttpGet("year/{year:int}")]
        public async Task<IActionResult> GetYearAsync(int year)
        {
            if (year < 1900)
                return BadRequest(string.Format("Invalid year number {0}", year));
            return Ok(await _repository.GetOnPeriodAsync(year: year));
        }

        // GET api/onstage/week
        [HttpGet("promo")]
        public async Task<IActionResult> GetPromoAsync()
        {
            return Ok(await _repository.GetPromotionsAsync());
        }
    }
}
