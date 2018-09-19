using System;
using System.Data;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Seats4Me.Data.Model;

namespace Seats4Me.API.Repositories
{
    public class TheatreRepository
    {
        protected readonly TheatreContext _context;

        public TheatreRepository(TheatreContext context)
        {
            _context = context;
        }

        protected async Task SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                ThrowDataException(e);
            }
            catch (ArgumentException e)
            {
                ThrowDataException(e);
            }
        }

        private static void ThrowDataException(Exception e)
        {
            var errMsg = e.Message;

            if (e.InnerException != null)
            {
                errMsg = e.InnerException.Message;
            }

            throw new DataException(errMsg);
        }
    }
}