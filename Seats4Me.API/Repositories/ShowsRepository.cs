using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Seats4Me.Data.Model;

namespace Seats4Me.API.Repositories
{
    public class ShowsRepository : TheatreRepository, IShowsRepository
    {
        public ShowsRepository(TheatreContext context) : base(context)
        {
        }

        public Task<List<Show>> GetAsync(DateTime? firstDay, DateTime? lastDay, bool onlyPromo)
        {
            var shows = _context.Shows
                .Include(s => s.TimeSlots)
                .Where(s => s.TimeSlots.Any(ts =>
                    (firstDay == null || ts.Day >= firstDay) &&
                    (lastDay == null || ts.Day <= lastDay) && (!onlyPromo || ts.PromoPrice > 0)))
                .Select(s => new Show
                {
                    Name = s.Name,
                    Title = s.Title,
                    Description = s.Description,
                    RegularPrice = s.RegularPrice,
                    RegularDiscountPrice = s.RegularDiscountPrice,
                    TimeSlots = s.TimeSlots.Where(ts =>
                        (firstDay == null || ts.Day >= firstDay) &&
                        (lastDay == null || ts.Day <= lastDay) && (!onlyPromo || ts.PromoPrice > 0)).ToList()
                })
                .ToListAsync();

            return shows;
        }

        public Task<Show> GetAsync(int showId)
        {
            return _context.Shows
                .Include(s => s.TimeSlots)
                .SingleOrDefaultAsync(s => s.Id == showId);
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

            foreach (var show in await _context.Shows.Include(s => s.TimeSlots)
                .Where(s => s.TimeSlots.Any(ts => ts.Day > DateTime.Today)).ToListAsync())
            {
                export.AppendLine("=");
                export.AppendLine($"name: {show.Name}");
                export.AppendLine($"title: {show.Title}");
                export.AppendLine($"description:\n{show.Description}");
                export.AppendLine("dates:");

                foreach (var timeSlot in show.TimeSlots.Where(ts => ts.Day > DateTime.Today))
                    export.AppendLine($"\t{timeSlot.Day:dd-MM-yyyy HH:mm}");

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

        /*
        private async Task<IEnumerable<Show>> GetTimeSlotShow(DateTime? firstDay, DateTime? lastDay, bool onlyPromo)
        {
        var shows = await _context.Shows.Include(s => s.TimeSlots.Where(ts => (firstDay == null || ts.Day >= firstDay) && (lastDay == null || ts.Day <= lastDay) && (!onlyPromo || ts.PromoPrice > 0))).ToListAsync();

        .Join(_context.TimeSlots,
            s => s.Id,
            t => t.ShowId,
            (s, t) => new Show
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
                SoldOut = !_context.Seats
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
                    .Any(ss => ss.tt == null)
            }
        )
        .Where(test)
        .ToListAsync();

            return shows;
        }

        public async Task<IEnumerable<TimeSlotShow>> GetSlotsAsync()
        {
            return await GetTimeSlotShow(s => s.Day >= DateTime.Today);
        }
        */
    }
}