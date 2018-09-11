using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Seats4Me.Data.Model;

namespace Seats4Me.API.Repositories
{
    public class TimeSlotsRepository : TheatreRepository, ITimeSlotsRepository
    {
        public TimeSlotsRepository(TheatreContext context) : base(context)
        {
        }
        public Task<List<TimeSlot>> GetAsync()
        {
            return _context.TimeSlots.ToListAsync();
        }

        public Task<TimeSlot> GetAsync(int showId)
        {
            return _context.TimeSlots.SingleOrDefaultAsync(s => s.Id == showId);
        }

        public async Task<TimeSlot> AddAsync(TimeSlot value)
        {
            var timeSlot = await _context.TimeSlots.AddAsync(value);
            if (!await SaveChangesAsync())
                return null;

            return timeSlot.Entity;
        }

        public async Task<TimeSlot> UpdateAsync(TimeSlot value)
        {
            LastErrorMessage = "";
            _context.TimeSlots.Attach(value);
            var timeSlotEntity = _context.TimeSlots.Update(value);
            if (!await SaveChangesAsync())
                return null;

            return timeSlotEntity.Entity;
        }


        public async Task<bool> DeleteAsync(int timeSlotId)
        {
            var timeSlot = await _context.TimeSlots.FindAsync(timeSlotId);
            if (timeSlot == null)
            {
                LastErrorMessage = $"No record found to delete for '{timeSlotId}'";
                return false;
            }

            _context.TimeSlots.Remove(timeSlot);
            return await SaveChangesAsync();
        }
    }
}
