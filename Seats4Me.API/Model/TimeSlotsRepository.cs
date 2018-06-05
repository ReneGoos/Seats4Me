using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Seats4Me.API.Data;
using Seats4Me.Data.Common;
using Seats4Me.Data.Model;

namespace Seats4Me.API.Model
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

            return timeSlot.Entity.TimeSlotId;
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
                LastErrorMessage = string.Format("No record found to delete for '{0}'", timeSlotId);
                return false;
            }

            _context.TimeSlots.Remove(timeSlot);
            return await SaveChangesAsync();
        }
    }
}
