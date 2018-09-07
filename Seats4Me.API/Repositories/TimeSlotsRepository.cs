using System.Threading.Tasks;
using Seats4Me.Data.Model;
using TimeSlot = Seats4Me.API.Models.Input.TimeSlot;

namespace Seats4Me.API.Repositories
{
    public class TimeSlotsRepository : TheatreRepository
    {
        public TimeSlotsRepository(TheatreContext context) : base(context)
        {
        }

        public async Task<int> AddAsync(TimeSlot value)
        {
            var timeSlot = await _context.TimeSlots.AddAsync(value);
            if (!await SaveChangesAsync())
                return -1;

            return timeSlot.Entity.Id;
        }

        public async Task<bool> UpdateAsync(TimeSlot value)
        {
            LastErrorMessage = "";
            _context.TimeSlots.Attach(value);
            _context.TimeSlots.Update(value);
            return await SaveChangesAsync();
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
