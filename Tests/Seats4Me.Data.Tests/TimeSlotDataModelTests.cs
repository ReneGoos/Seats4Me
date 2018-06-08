using Seats4Me.Data.Model;
using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Seats4Me.Data.Tests
{
    public class TimeSlotDataModelTests
    {
        [Fact]
        public async Task AddNewTimeSlotCalculatesWeek()
        {
            //Arrange
            var builder = new DbContextOptionsBuilder<TheatreContext>()
                .UseInMemoryDatabase(new Guid().ToString());
            var context = new TheatreContext(builder.Options);
            //Act
            var timeSlotEntry = await context.TimeSlots.AddAsync(new TimeSlot()
                {
                    Day = Convert.ToDateTime("08-06-2018 20:00", CultureInfo.CurrentCulture),
                    Hours = 1.5
                });
            await context.SaveChangesAsync();
            //Assert
            var timeSlot = await context.TimeSlots.FindAsync(timeSlotEntry.Entity.Id);
            Assert.NotEqual(0, timeSlot.Week);
        }
    }
}