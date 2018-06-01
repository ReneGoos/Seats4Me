using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Seats4Me.Data.Model;

namespace Seats4Me.API.Model
{
    public class TheatreRepository
    {
        protected readonly TheatreContext _context;

        public TheatreRepository(TheatreContext context)
        {
            _context = context;
        }
        public string LastErrorMessage { get; protected set; }

        protected async Task<bool> SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                if (e.InnerException != null)
                    LastErrorMessage = e.InnerException.Message;
                else
                    LastErrorMessage = e.Message;
                return false;
            }

            return true;
        }
    }
}