using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Seats4Me.API.Data;
using Seats4Me.Data.Common;
using Seats4Me.Data.Model;

namespace Seats4Me.API.Model
{
    public class ShowsRepository : TheatreRepository
    {
        public ShowsRepository(TheatreContext context) : base(context)
        {
        }

        private async Task<IEnumerable<TimeSlotShow>> GetTimeSlotShow(Expression<Func<TimeSlotShow,bool>> test)
        {
            var shows = await _context.Shows
                .Join(_context.TimeSlots,
                    s => s.ShowId,
                    t => t.ShowId,
                    (s, t) => new TimeSlotShow()
                    {
                        ShowId = s.ShowId,
                        TimeSlotId = t.TimeSlotId,
                        Name = s.Name,
                        Title = s.Title,
                        Description = s.Description,
                        RegularPrice = s.RegularPrice,
                        RegularDiscountPrice = s.RegularDiscountPrice,
                        PromoPrice = t.PromoPrice,
                        Day = t.Day,
                        Hours = t.Hours,
                        SoldOut = !(_context.Seats
                            .GroupJoin(
                                _context.TimeSlotSeats.Where(a => a.TimeSlotId == t.TimeSlotId),
                                ss => ss.SeatId,
                                tt => tt.SeatId,
                                (ss, tt) => new
                                {
                                    ss,
                                    tt
                                }
                            )
                            .SelectMany(st => st.tt.DefaultIfEmpty(),
                                (a, tt) => new
                                {
                                    a.ss,
                                    tt
                                })
                            .Any(ss => ss.tt == null))
                    }
                )
                .Where(test)
                .ToListAsync();
            return shows;
        }

        public async Task<IEnumerable<TimeSlotShow>> GetPromotionsAsync()
        {
            return await GetTimeSlotShow(s => s.PromoPrice > 0);
        }

        public async Task<IEnumerable<TimeSlotShow>> GetAsync()
        {
            return await GetTimeSlotShow(s => s.Day >= DateTime.Today);
        }

        public async Task<IEnumerable<TimeSlotShow>> GetOnPeriodAsync(int week = 0, int month = 0, int year = 0)
        {
            var today = DateTime.Today;
            var start = today;
            var end = today.AddDays(7 - (int)today.DayOfWeek);

            if (week != 0)
            {
                if (year == 0)
                {
                    if (week < today.Week())
                        year = today.Year + 1;
                    else
                        year = today.Year;
                }

                start = DateTimeExtensions.FirstDayOfWeek(week, year);
                end = start.AddDays(7);
            }
            else if (month != 0)
            {
                if (year == 0)
                {
                    if (month < today.Month)
                        year = today.Year + 1;
                    else
                        year = today.Year;
                }

                start = new DateTime(year, month, 1);
                end = new DateTime(year, month + 1, 1);
            }
            else if (year != 0)
            {
                if (year != today.Year)
                {
                    start = new DateTime(year, 1, 1);
                }

                end = new DateTime(year + 1, 1, 1);
            }

            return await GetTimeSlotShow(s => s.Day >= start && s.Day < end);
        }

        public async Task<Show> GetAsync(int showId)
        {
            return await _context.Shows.Include(s => s.TimeSlots).SingleOrDefaultAsync(s => s.ShowId == showId);
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


        public async Task<bool> DeleteAsync(int showId)
        {
            var show = await _context.Shows.FindAsync(showId);
            if (show == null)
            {
                LastErrorMessage = string.Format("No record found to delete for '{0}'", showId);
                return false;
            }

            _context.Shows.Remove(show);
            return await SaveChangesAsync();
        }

        public async Task<string> GetExport()
        {
            var export = new StringBuilder();
            export.AppendLine("type: event-export");
            export.AppendLine("format: 1986.2");

            foreach (var show in await _context.Shows.Include(s => s.TimeSlots).Where(s => s.TimeSlots.Any(ts => ts.Day > DateTime.Today)).ToListAsync())
            {
                export.AppendLine("=");
                export.AppendLine(string.Format("name: {0}", show.Name));
                export.AppendLine(string.Format("title: {0}", show.Title));
                export.AppendLine(string.Format("description:\n{0}", show.Name));
                export.AppendLine("dates:");

                foreach (var timeSlot in show.TimeSlots.Where(ts => ts.Day > DateTime.Today))
                {
                    export.AppendLine(string.Format("\t{0}", timeSlot.Day));
                }

                if (show.RegularPrice > 0 || show.RegularDiscountPrice > 0)
                {
                    export.AppendLine("prices:");
                    if (show.RegularPrice > 0)
                        export.AppendLine(string.Format("\tregular: {0}", show.RegularPrice));
                    if (show.RegularDiscountPrice > 0)
                        export.AppendLine(string.Format("\tdiscount: {0}", show.RegularDiscountPrice));
                }
            }
            return export.ToString();
        }
    }
}
