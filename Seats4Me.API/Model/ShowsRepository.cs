using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Seats4Me.API.Data;
using Seats4Me.Data.Model;

namespace Seats4Me.API.Model
{
    public class ShowsRepository
    {
        private TheatreContext _context;

        public ShowsRepository(TheatreContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Show>> GetAsync()
        {
            return await _context.Shows.Include(s => s.TimeSlots).ToListAsync();
        }

        public async Task<Show> GetAsync(int id)
        {
            return await _context.Shows.Include(s => s.TimeSlots).SingleOrDefaultAsync(s => s.ShowId == id);
        }

        public async Task<int> AddAsync(Show value)
        {
            var story = await _context.Shows.AddAsync(value);
            await _context.SaveChangesAsync();
            return story.Entity.ShowId;
        }

        public async Task<IEnumerable<CalendarShows>> GetCalendarAsync()
        {
            return await _context.TimeSlots
                            .Select(t => new
                            {
                                Week = CultureInfo.CurrentCulture.Calendar
                                                    .GetWeekOfYear(t.Start,
                                                                    CalendarWeekRule.FirstFourDayWeek,
                                                                    DayOfWeek.Monday
                                                                    ),
                                t.Start.Month,
                                t.Start.Year
                            })
                            .Distinct()
                            .Select(a => new { a.Week, a.Month, a.Year })
                            .GroupJoin(
                                _context.TimeSlots
                                    .Join(_context.Shows,
                                            t => t.ShowId,
                                            s => s.ShowId,
                                            (t, s) =>
                                            new
                                            {
                                                Week = CultureInfo.CurrentCulture.Calendar
                                                        .GetWeekOfYear(t.Start,
                                                                        CalendarWeekRule.FirstFourDayWeek,
                                                                        DayOfWeek.Monday
                                                                        ),
                                                t.Start.Month,
                                                t.Start.Year,
                                                TimeSlot = t,
                                                Show = s
                                            }),
                                a => new { a.Week, a.Month, a.Year },
                                ts => new { ts.Week, ts.Month, ts.Year },
                                (a, tsc) =>
                                new CalendarShows
                                { 
                                    Week = a.Week,
                                    Month = a.Month,
                                    Year = a.Year,
                                    Shows = tsc.Select ( ts =>
                                        new TimeSlotShow
                                        {
                                             ShowId =  ts.Show.ShowId,
                                             TimeSlotId =  ts.TimeSlot.TimeSlotId,
                                             Name = ts.Show.Name,
                                             Title = ts.Show.Title,
                                             Description = ts.Show.Description,
                                             RegularPrice = ts.Show.RegularPrice,
                                             RegularDiscountPrice = ts.Show.RegularDiscountPrice,
                                             PromoPrice  = ts.Show.PromoPrice,
                                             Start = ts.TimeSlot.Start,
                                             End = ts.TimeSlot.End
                                        })
                                }
                                ).ToListAsync();
        }
    }
}
