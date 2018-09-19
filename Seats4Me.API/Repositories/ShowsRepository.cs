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
        public ShowsRepository(TheatreContext context)
            : base(context)
        {
        }

        public async Task<Show> AddAsync(Show show)
        {
            var showEntity = await _context.Shows.AddAsync(show);

            await SaveChangesAsync();

            return showEntity.Entity;
        }

        public async Task DeleteAsync(Show show)
        {
            _context.Shows.Remove(show);

            await SaveChangesAsync();
        }

        public Task<List<Show>> GetAsync(DateTime? firstDay, DateTime? lastDay, bool onlyPromo)
        {
            var shows = _context.Shows.Include(s => s.TimeSlots)
                                .Where(s => s.TimeSlots.Any(ts => (firstDay == null || ts.Day >= firstDay) && (lastDay == null || ts.Day <= lastDay) && (!onlyPromo || ts.PromoPrice > 0)))
                                .Select(s => new Show
                                             {
                                                 Id = s.Id,
                                                 Name = s.Name,
                                                 Title = s.Title,
                                                 Description = s.Description,
                                                 RegularPrice = s.RegularPrice,
                                                 RegularDiscountPrice = s.RegularDiscountPrice,
                                                 TimeSlots = s.TimeSlots.Where(ts => (firstDay == null || ts.Day >= firstDay) && (lastDay == null || ts.Day < lastDay) && (!onlyPromo || ts.PromoPrice > 0)).ToList()
                                             })
                                .ToListAsync();

            return shows;
        }

        public Task<Show> GetAsync(int showId)
        {
            return _context.Shows.Include(s => s.TimeSlots).SingleOrDefaultAsync(s => s.Id == showId);
        }

        public async Task<string> GetExportAsync()
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
                    {
                        export.AppendLine($"\tregular: {show.RegularPrice:c}");
                    }

                    if (show.RegularDiscountPrice > 0)
                    {
                        export.AppendLine($"\tdiscount: {show.RegularDiscountPrice:c}");
                    }
                }
            }

            return export.ToString();
        }

        public async Task<Show> UpdateAsync(Show show)
        {
            var showEntity = _context.Shows.Update(show);

            await SaveChangesAsync();

            return showEntity.Entity;
        }
    }
}