using Seats4Me.API.Data;
using Seats4Me.API.Model;
using Seats4Me.Data.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Seats4Me.API.Tests
{
    public class ShowsRepositoryTests
    {
        [Fact]
        public async Task ListShowsByWeek()
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
                                    Length =  2
                                },

                                new TimeSlot()
                                {
                                    Start = Convert.ToDateTime("02-06-2018 20:00", CultureInfo.CurrentCulture),
                                    Length =  2
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
                                    Length =  1.5
                                },
                                new TimeSlot()
                                {
                                    Start = Convert.ToDateTime("15-06-2018 20:00", CultureInfo.CurrentCulture),
                                    Length = 1.5
                                }
                            }
                        }
                   });
            await context.SaveChangesAsync();
            var showsRepository = new ShowsRepository(context);
            //Act
            var listShows = await showsRepository.GetCalendarAsync();
            //Assert
            Assert.True(listShows.Any());
            Assert.True(listShows.First().Shows.Any());
        }
    }
}
