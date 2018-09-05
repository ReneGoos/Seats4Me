using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Seats4Me.API.Models;
using Seats4Me.Data.Common;
using Seats4Me.Data.Model;

namespace Seats4Me.API.Repositories
{
    public class ShowsRepository : TheatreRepository, IShowsRepository
    {
        public ShowsRepository(TheatreContext context) : base(context)
        {
        }

        private async Task<IEnumerable<TimeSlotShow>> GetTimeSlotShow(Expression<Func<TimeSlotShow,bool>> test)
        {
            var shows = await _context.Shows
                .Join(_context.TimeSlots,
                    s => s.Id,
                    t => t.ShowId,
                    (s, t) => new TimeSlotShow()
                    {
                        ShowId = s.Id,
                        TimeSlotId = t.Id,
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
                                _context.TimeSlotSeats.Where(a => a.TimeSlotId == t.Id),
                                ss => ss.Id,
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
            return await GetTimeSlotShow(s => s.Day >= DateTime.Today && s.PromoPrice > 0);
        }

        public async Task<IEnumerable<TimeSlotShow>> GetSlotsAsync()
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
                    year = today.Year;
                }

                start = DateTimeExtensions.FirstDayOfWeek(week, year);
                end = start.AddDays(7);
            }
            else if (month != 0)
            {
                if (year == 0)
                {
                    year = today.Year;
                }

                start = new DateTime(year, month, 1);
                end = new DateTime(year, month + 1, 1);
            }
            else
            {
                if (year == 0)
                {
                    year = today.Year;
                }
                else
                {
                    if (year > 0 && year < 100)
                        year = year + 2000;

                    start = new DateTime(year, 1, 1);
                }

                end = new DateTime(year + 1, 1, 1);
            }

            return await GetTimeSlotShow(s => s.Day >= start && s.Day < end);
        }

        public Task<List<Show>> GetAsync()
        {
            return _context.Shows.ToListAsync();
        }

        public Task<Show> GetAsync(int showId)
        {
            return _context.Shows.SingleOrDefaultAsync(s => s.Id == showId);
        }

        public async Task<Show> AddAsync(Show value)
        {
            var show = await _context.Shows.AddAsync(value);
            if (!await SaveChangesAsync())
                return null;

            return show.Entity;
        }

        public async Task<Show> UpdateAsync(Show value)
        {
            LastErrorMessage = "";
            _context.Shows.Attach(value);
            var showEntity = _context.Shows.Update(value);
            if (await SaveChangesAsync())
                return null;

            return showEntity.Entity;
        }


        public async Task<bool> DeleteAsync(int showId)
        {
            var show = await _context.Shows.FindAsync(showId);
            if (show == null)
            {
                LastErrorMessage = $"No record found to delete for '{showId}'";
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
                export.AppendLine($"name: {show.Name}");
                export.AppendLine($"title: {show.Title}");
                export.AppendLine($"description:\n{show.Description}");
                export.AppendLine("dates:");

                foreach (var timeSlot in show.TimeSlots.Where(ts => ts.Day > DateTime.Today))
                {
                    export.AppendLine($"\t{timeSlot.Day:dd-MM-yyyy HH:mm}");
                }

                if (show.RegularPrice > 0 || show.RegularDiscountPrice > 0)
                {
                    export.AppendLine("prices:");
                    if (show.RegularPrice > 0)
                        export.AppendLine($"\tregular: {show.RegularPrice:c}");
                    if (show.RegularDiscountPrice > 0)
                        export.AppendLine($"\tdiscount: {show.RegularDiscountPrice:c}");
                }
            }
            return export.ToString();
        }
    }
}
