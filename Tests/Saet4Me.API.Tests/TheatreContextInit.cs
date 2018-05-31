using Microsoft.EntityFrameworkCore;
using Seats4Me.Data.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Seats4Me.API.Tests
{
    public static class TheatreContextInit
    {
        public static TheatreContext InitializeContextInMemoryDb()
        {
            var builder = new DbContextOptionsBuilder<TheatreContext>()
                            .UseInMemoryDatabase(new Guid().ToString());
            var context = new TheatreContext(builder.Options);
            return context;
        }
    }
}
