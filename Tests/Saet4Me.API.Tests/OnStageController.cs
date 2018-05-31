using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Seats4Me.API.Model;
using Seats4Me.Data.Model;
using Xunit;

namespace Seats4Me.API.Tests
{
    public class OnStageController
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
                        End =  Convert.ToDateTime("01-06-2018 22:00", CultureInfo.CurrentCulture)
                    }
                }
            });
            await context.SaveChangesAsync();
            var showsRepository = new ShowsRepository(context);
            var shows = new Controllers.OnStageController(showsRepository);
            //Act
            var listShows = await shows.GetAsync();
            //Assert
            Assert.True(listShows.Any());
            Assert.True(listShows.First().TimeSlots.Any());
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
                                    End =  Convert.ToDateTime("31-05-2018 22:00", CultureInfo.CurrentCulture)
                                },

                                new TimeSlot()
                                {
                                    Start = Convert.ToDateTime("02-06-2018 20:00", CultureInfo.CurrentCulture),
                                    End =  Convert.ToDateTime("02-06-2018 22:00", CultureInfo.CurrentCulture)
                                },
                                new TimeSlot()
                                {
                                    Start = Convert.ToDateTime("08-06-2018 20:00", CultureInfo.CurrentCulture),
                                    End =  Convert.ToDateTime("08-06-2018 22:00", CultureInfo.CurrentCulture)
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
                                    End =  Convert.ToDateTime("01-06-2018 22:00", CultureInfo.CurrentCulture)
                                },
                                new TimeSlot()
                                {
                                    Start = Convert.ToDateTime("15-06-2018 20:00", CultureInfo.CurrentCulture),
                                    End =  Convert.ToDateTime("15-06-2018 22:00", CultureInfo.CurrentCulture)
                                }
                            }
                        }
                   });
            await context.SaveChangesAsync();
            var showsRepository = new ShowsRepository(context);
            var shows = new Controllers.OnStageController(showsRepository);
            //Act
            var listShows = await shows.GetCalendarAsync();
            //Assert
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
            var id = await shows.PostAsync(new Show()
            {
                Name = "De Woof-side story",
                TimeSlots = new List<TimeSlot>()
                {
                    new TimeSlot()
                    {
                        Start = Convert.ToDateTime("01-06-2018 14:00", CultureInfo.CurrentCulture),
                        End =  Convert.ToDateTime("01-06-2018 15:00", CultureInfo.CurrentCulture)
                    }
                }
            });
            //Assert
            Assert.True(context.Shows.Any());
            Assert.Equal("De Woof-side story", context.Shows.Find(id).Name);
        }
    }
}
