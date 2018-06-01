using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Seats4Me.API.Data;
using Seats4Me.Data.Model;

namespace Seats4Me.API.Model
{
    public class ShowsRepository : TheatreRepository
    {
        public ShowsRepository(TheatreContext context) : base(context)
        {
        }

        public async Task<IEnumerable<TimeSlotShow>> GetAsync()
        {
            return await _context.Shows
                                .Join(_context.TimeSlots,
                                        s => s.ShowId,
                                        t => t.ShowId,
                                        (s,t) => new TimeSlotShow()
                                        {
                                            ShowId = s.ShowId,
                                            TimeSlotId = t.TimeSlotId,
                                            Name = s.Name,
                                            Title = s.Title,
                                            Description = s.Description,
                                            RegularPrice = s.RegularPrice,
                                            RegularDiscountPrice = s.RegularDiscountPrice,
                                            PromoPrice = s.PromoPrice,
                                            Start = t.Start,
                                            Length = t.Length
                                        }
                                     )
                                .OrderBy(s => s.Start)
                                .ToListAsync();
        }

        public async Task<Show> GetAsync(int id)
        {
            return await _context.Shows.Include(s => s.TimeSlots).SingleOrDefaultAsync(s => s.ShowId == id);
        }

        public async Task<int> AddAsync(Show value)
        {
            var story = await _context.Shows.AddAsync(value);
            if (!await SaveChangesAsync())
                return -1;

            return story.Entity.ShowId;
        }

        public async Task<bool> UpdateAsync(Show value)
        {
            LastErrorMessage = "";
            _context.Shows.Attach(value);
            _context.Shows.Update(value);
            return await SaveChangesAsync();
        }


        public async Task<bool> DeleteAsync(int id)
        {
            var show = await _context.Shows.FindAsync(id);
            if (show == null)
            {
                LastErrorMessage = string.Format("No record found to delete for '{0}'", id);
                return false;
            }

            _context.Shows.Remove(show);
            return await SaveChangesAsync();
        }
        public async Task<IEnumerable<CalendarShows>> GetCalendarAsync()
        {
            return await _context.TimeSlots
                            .Select(t => new
                            {
                                t.Week,
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
                                                t.Week,
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
                                             Length = ts.TimeSlot.Length
                                        })
                                }
                                ).ToListAsync();
        }
    }
}
