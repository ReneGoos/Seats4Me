using Seats4Me.Data.Model;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Seats4Me.Data.Tests
{
    public class ShowData
    {
        [Fact]
        public async Task AddNewShowReturnsShow()
        {
            //Arrange
            var builder = new DbContextOptionsBuilder<TheatreContext>()
                                .UseInMemoryDatabase(new Guid().ToString());
            var context = new TheatreContext(builder.Options);
            //Act
            var showEntry = await context.Shows.AddAsync(new Show() {Name = "Midsummer night dream"});
            await context.SaveChangesAsync();
            //Assert
            Assert.True(context.Shows.Any());
            var show = await context.Shows.FindAsync(showEntry.Entity.ID);
            Assert.Equal("Midsummer night dream", show.Name);
        }
    }
}
