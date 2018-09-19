using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Seats4Me.Data.Model;

namespace Seats4Me.API.Repositories
{
    public class SeatsRepository : TheatreRepository, ISeatsRepository
    {
        public SeatsRepository(TheatreContext context)
            : base(context)
        {
        }

        public Task<List<Seat>> GetAsync()
        {
            return _context.Seats.ToListAsync();
        }

        public Task<Seat> GetAsync(int id)
        {
            return _context.Seats.FirstOrDefaultAsync(s => s.Id == id);
        }

        public Task<Seat> GetAsync(int row, int chair)
        {
            return _context.Seats.FirstOrDefaultAsync(s => s.Row == row && s.Chair == chair);
        }
    }
}