using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Seats4Me.API.Model;
using Seats4Me.Data.Model;
using Xunit;

namespace Seats4Me.API.Tests
{
    public class AdminShowsControllerTests
    {
        [Fact]
        public async Task NewShowInListWhenAdded()
        {
            //Arrange
            var context = TheatreContextInit.InitializeContextInMemoryDb(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.GUID.ToString());
            var showsRepository = new ShowsRepository(context);
            var shows = new Controllers.AdminShowController(showsRepository);
            //Act
            var result = await shows.PostAsync(new Show()
            {
                Name = "De Woef side story",
                TimeSlots = new List<TimeSlot>()
                {
                    new TimeSlot()
                    {
                        Day = Convert.ToDateTime("01-06-2018 14:00", CultureInfo.CurrentCulture),
                        Hours = 1.5
                    }
                }
            });
            //Assert
            var okResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var id = Assert.IsAssignableFrom<int>(okResult.Value);
            Assert.True(context.Shows.Any());
            Assert.Equal("De Woef side story", context.Shows.Find(id).Name);
        }

        [Fact]
        public async Task UpdateShowPriceWhenChanged()
        {
            //Arrange
            var context = TheatreContextInit.InitializeContextInMemoryDb(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.GUID.ToString());
            var show = await context.Shows.AddAsync(new Show()
            {
                Name = "De Woef side story",
                TimeSlots = new List<TimeSlot>()
                {
                    new TimeSlot()
                    {
                        Day = Convert.ToDateTime("01-06-2018 14:00", CultureInfo.CurrentCulture),
                        Hours = 1.5
                    }
                }
            });
            await context.SaveChangesAsync();

            var showsRepository = new ShowsRepository(context);
            var shows = new Controllers.AdminShowController(showsRepository);
            //Act
            show.Entity.RegularPrice = 100;
            var result = await shows.PutAsync(show.Entity.Id, show.Entity);
            //Assert
            var okResult = Assert.IsAssignableFrom<OkResult>(result);
            Assert.NotNull(context.Shows.First(s => s.RegularPrice == 100));
        }

        [Fact]
        public async Task DeleteNewEntrySucceds()
        {
            //Arrange
            var context = TheatreContextInit.InitializeContextInMemoryDb(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.GUID.ToString());
            //Act
            var show = await context.Shows.AddAsync(new Show()
            {
                Name = "De Woef side story",
                TimeSlots = new List<TimeSlot>()
                                {
                                    new TimeSlot()
                                    {
                                        Day = Convert.ToDateTime("01-06-2018 14:00", CultureInfo.CurrentCulture),
                                        Hours = 1.5
                                    }
                                }
            });
            await context.SaveChangesAsync();
            var id = show.Entity.Id;
            var showsRepository = new ShowsRepository(context);
            var shows = new Controllers.AdminShowController(showsRepository);
            //Act
            var result = await shows.DeleteAsync(id);
            //Assert
            Assert.IsAssignableFrom<OkResult>(result);
            Assert.Null(context.Shows.Find(id));
        }

        [Fact]
        public async Task DeleteInvalidIdFails()
        {
            //Arrange
            var context = TheatreContextInit.InitializeContextInMemoryDb(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.GUID.ToString());
            var showsRepository = new ShowsRepository(context);
            var shows = new Controllers.AdminShowController(showsRepository);
            var id = -1;
            //Act
            var result = await shows.DeleteAsync(id);
            //Assert
            var errorResult = Assert.IsAssignableFrom<BadRequestObjectResult>(result);
            var message = Assert.IsAssignableFrom<string>(errorResult.Value);
            Assert.Contains(string.Format("'{0}'", id), message);
        }
    }
}
