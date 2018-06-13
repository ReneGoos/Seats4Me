using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Seats4Me.API.Model;
using Seats4Me.Data.Model;
using Xunit;

namespace Seats4Me.API.Tests
{
    public class AdminTimeSlotsControllerTests
    {
        [Fact]
        public async Task NewTimeSlotInListWhenAdded()
        {
            //Arrange
            var context = TheatreContextInit.InitializeContextInMemoryDb(System.Reflection.MethodBase.GetCurrentMethod()
                .DeclaringType.GUID.ToString());
            var show = await context.Shows.AddAsync(new Show()
            {
                Name = "De Woef side story"
            });
            await context.SaveChangesAsync();

            var showId = show.Entity.Id;
            var timeSlotsRepository = new TimeSlotsRepository(context);
            var timeSlots = new Controllers.AdminTimeSlotController(timeSlotsRepository);
            //Act
            var result = await timeSlots.PostAsync(new TimeSlot()
            {
                Day = Convert.ToDateTime("01-06-2018 14:00", CultureInfo.CurrentCulture),
                Hours = 1.5,
                ShowId = showId
            });
            //Assert
            var okResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            Assert.IsAssignableFrom<int>(okResult.Value);
            Assert.True(context.Shows.Find(showId).TimeSlots.Any());
        }
        [Fact]
        public async Task UpdateTimeSlotInListWhenNewLength()
        {
            //Arrange
            var context = TheatreContextInit.InitializeContextInMemoryDb(System.Reflection.MethodBase.GetCurrentMethod()
                .DeclaringType.GUID.ToString());
            var show = await context.Shows.AddAsync(new Show()
            {
                Name = "De Woef side story"
            });
            var timeSlot = await context.TimeSlots.AddAsync(new TimeSlot()
            {
                Day = Convert.ToDateTime("01-06-2018 14:00", CultureInfo.CurrentCulture),
                Hours = 1.5,
                ShowId = show.Entity.Id
            });
            await context.SaveChangesAsync();

            var showId = show.Entity.Id;
            var timeSlotId = timeSlot.Entity.Id;
            var timeSlotsRepository = new TimeSlotsRepository(context);
            var timeSlots = new Controllers.AdminTimeSlotController(timeSlotsRepository);
            //Act
            timeSlot.Entity.Hours = 2.5;
            var result = await timeSlots.PutAsync(timeSlotId, timeSlot.Entity);
            //Assert
            Assert.IsAssignableFrom<OkResult>(result);
            Assert.NotNull(context.Shows.Find(showId).TimeSlots.First(ts => Math.Abs(ts.Hours - 2.5) < 0.01));
        }

        [Fact]
        public async Task DeleteNewEntrySucceds()
        {
            //Arrange
            var context = TheatreContextInit.InitializeContextInMemoryDb(System.Reflection.MethodBase.GetCurrentMethod()
                .DeclaringType.GUID.ToString());
            var show = await context.Shows.AddAsync(new Show()
            {
                Name = "De Woef side story"
            });
            await context.SaveChangesAsync();
            var showId = show.Entity.Id;
            var timeSlot = await context.TimeSlots.AddAsync(new TimeSlot()
            {
                Day = Convert.ToDateTime("01-06-2018 14:00", CultureInfo.CurrentCulture),
                Hours = 1.5,
                ShowId = showId
            });
            await context.SaveChangesAsync();
            var id = timeSlot.Entity.Id;
            var timeSlotsRepository = new TimeSlotsRepository(context);
            var timeSlots = new Controllers.AdminTimeSlotController(timeSlotsRepository);
            //Act
            var result = await timeSlots.DeleteAsync(id);
            //Assert
            Assert.IsAssignableFrom<OkResult>(result);
            Assert.Null(context.TimeSlots.Find(id));
        }

        [Fact]
        public async Task DeleteInvalidIdFails()
        {
            //Arrange
            var context = TheatreContextInit.InitializeContextInMemoryDb(System.Reflection.MethodBase.GetCurrentMethod()
                .DeclaringType.GUID.ToString());
            var timeSlotsRepository = new TimeSlotsRepository(context);
            var timeSlots = new Controllers.AdminTimeSlotController(timeSlotsRepository);
            var id = -1;
            //Act
            var result = await timeSlots.DeleteAsync(id);
            //Assert
            var errorResult = Assert.IsAssignableFrom<BadRequestObjectResult>(result);
            var message = Assert.IsAssignableFrom<string>(errorResult.Value);
            Assert.Contains(string.Format("'{0}'", id), message);
        }
    }
}
