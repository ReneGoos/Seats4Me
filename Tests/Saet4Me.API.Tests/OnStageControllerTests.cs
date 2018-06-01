using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Seats4Me.API.Data;
using Seats4Me.API.Model;
using Seats4Me.Data.Model;
using Xunit;

namespace Seats4Me.API.Tests
{
    public class OnStageControllerTests
    {
        [Fact]
        public async Task ListShowsWhenGetTheatre()
        {
            //Arrange
            var context = TheatreContextInit.InitializeContextInMemoryDb();
            context.Shows.Add(new Show()
            {
                Name = "Hamlet",
                TimeSlots = new List<TimeSlot>()
                {
                    new TimeSlot()
                    {
                        Start = Convert.ToDateTime("01-06-2018 20:00", CultureInfo.CurrentCulture),
                        Length =  2
                    }
                }
            });
            await context.SaveChangesAsync();
            var showsRepository = new ShowsRepository(context);
            var shows = new Controllers.OnStageController(showsRepository);
            //Act
            var result = await shows.GetAsync();
            //Assert
            var okResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var listShows = Assert.IsAssignableFrom<IEnumerable<TimeSlotShow>>(okResult.Value);
            Assert.True(listShows.Any());
        }

        [Fact]
        public async Task ListShowsByWeekWhenGetTheatreWeek()
        {
            //Arrange
            var context = TheatreContextInit.InitializeContextInMemoryDb();
            context.Shows.AddRange(
                    new List<Show>()
                    {
                        new Show()
                        {
                            Name = "Hamlet vs Hamlet",
                            TimeSlots = new List<TimeSlot>()
                            {
                                new TimeSlot()
                                {
                                    Start = Convert.ToDateTime("31-05-2018 20:00", CultureInfo.CurrentCulture),
                                    Length =  2.5
                                },

                                new TimeSlot()
                                {
                                    Start = Convert.ToDateTime("02-06-2018 20:00", CultureInfo.CurrentCulture),
                                    Length =  2.5
                                },
                                new TimeSlot()
                                {
                                    Start = Convert.ToDateTime("08-06-2018 20:00", CultureInfo.CurrentCulture),
                                    Length =  3
                                }
                            }
                        },
                        new Show()
                        {
                            Name = "Branden",
                            TimeSlots = new List<TimeSlot>()
                            {
                                new TimeSlot()
                                {
                                    Start = Convert.ToDateTime("01-06-2018 20:00", CultureInfo.CurrentCulture),
                                    Length =  2
                                },
                                new TimeSlot()
                                {
                                    Start = Convert.ToDateTime("15-06-2018 20:00", CultureInfo.CurrentCulture),
                                    Length =  2
                                }
                            }
                        }
                   });
            await context.SaveChangesAsync();
            var showsRepository = new ShowsRepository(context);
            var shows = new Controllers.OnStageController(showsRepository);
            //Act
            var result = await shows.GetCalendarAsync();
            //Assert
            var okResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var listShows = Assert.IsAssignableFrom<IEnumerable<CalendarShows>>(okResult.Value);
            Assert.True(listShows.Any());
            Assert.True(listShows.First().Shows.Any());
        }

        [Fact]
        public async Task NewShowInListWhenAdded()
        {
            //Arrange
            var context = TheatreContextInit.InitializeContextInMemoryDb();
            var showsRepository = new ShowsRepository(context);
            var shows = new Controllers.OnStageController(showsRepository);
            //Act
            var result = await shows.PostAsync(new Show()
            {
                Name = "De Woef side story",
                TimeSlots = new List<TimeSlot>()
                {
                    new TimeSlot()
                    {
                        Start = Convert.ToDateTime("01-06-2018 14:00", CultureInfo.CurrentCulture),
                        Length = 1.5
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
        public async Task DeleteNewEntrySucceds()
        {
            //Arrange
            var context = TheatreContextInit.InitializeContextInMemoryDb();
            //Act
            var show = await context.Shows.AddAsync(new Show()
                            {
                                Name = "De Woef side story",
                                TimeSlots = new List<TimeSlot>()
                                {
                                    new TimeSlot()
                                    {
                                        Start = Convert.ToDateTime("01-06-2018 14:00", CultureInfo.CurrentCulture),
                                        Length = 1.5
                                    }
                                }
                            });
            await context.SaveChangesAsync();
            var id = show.Entity.ShowId;
            var showsRepository = new ShowsRepository(context);
            var shows = new Controllers.OnStageController(showsRepository);
            //Act
            var result = await shows.DeleteAsync(id);
            //Assert
            var okResult = Assert.IsAssignableFrom<OkResult>(result);
            Assert.Null(context.Shows.Find(id));
        }

        [Fact]
        public async Task DeleteInvalidIdFails()
        {
            //Arrange
            var context = TheatreContextInit.InitializeContextInMemoryDb();
            var showsRepository = new ShowsRepository(context);
            var shows = new Controllers.OnStageController(showsRepository);
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
