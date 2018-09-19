using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Seats4Me.Data.Model;

namespace Seats4Me.API.Repositories
{
    public class TimeSlotsRepository : TheatreRepository, ITimeSlotsRepository
    {
        public TimeSlotsRepository(TheatreContext context)
            : base(context)
        {
        }

        public async Task<TimeSlot> AddAsync(TimeSlot timeSlot)
        {
            var timeSlotEntity = await _context.TimeSlots.AddAsync(timeSlot);

            await SaveChangesAsync();

            return timeSlotEntity.Entity;
        }

        public async Task DeleteAsync(TimeSlot timeSlot)
        {
            _context.TimeSlots.Remove(timeSlot);

            await SaveChangesAsync();
        }

        public Task<List<TimeSlot>> GetAsync(int showId)
        {
            return _context.TimeSlots.Where(ts => ts.ShowId == showId).ToListAsync();
        }

        public Task<TimeSlot> GetAsync(int showId, int id)
        {
            return _context.TimeSlots.FirstOrDefaultAsync(s => s.Id == id && s.ShowId == showId);
        }

        public async Task<TimeSlot> UpdateAsync(TimeSlot timeSlot)
        {
            var timeSlotEntity = _context.TimeSlots.Update(timeSlot);

            await SaveChangesAsync();

            return timeSlotEntity.Entity;
        }
    }
}