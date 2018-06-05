using Microsoft.EntityFrameworkCore;
using Seats4Me.Data.Model;
using System;

namespace Seats4Me.API.Tests
{
    public static class TheatreContextInit
    {
        public static TheatreContext InitializeContextInMemoryDb(string name)
        {
            var builder = new DbContextOptionsBuilder<TheatreContext>()
                            .UseInMemoryDatabase(name);
            var context = new TheatreContext(builder.Options);
            return context;
        }
    }
}
