using Seats4Me.API.Repositories;
using Seats4Me.Data.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Seats4Me.Data.Common;
using Xunit;

namespace Seats4Me.API.Tests
{
    public class ShowsRepositoryTests
    {
        [Fact]
        public async Task ListShowsByWeek()
        {
            //Arrange
            var context = TheatreContextInit.InitializeContextInMemoryDb(System.Reflection.MethodBase.GetCurrentMethod()
                .DeclaringType.GUID.ToString());

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
                                Day = Convert.ToDateTime("31-05-2018 20:00", CultureInfo.CurrentCulture),
                                Hours = 2
                            },

                            new TimeSlot()
                            {
                                Day = Convert.ToDateTime("02-06-2018 20:00", CultureInfo.CurrentCulture),
                                Hours = 2
                            },
                            new TimeSlot()
                            {
                                Day = Convert.ToDateTime("08-06-2018 20:00", CultureInfo.CurrentCulture),
                                Hours = 3
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
                                Day = Convert.ToDateTime("01-06-2018 20:00", CultureInfo.CurrentCulture),
                                Hours = 1.5
                            },
                            new TimeSlot()
                            {
                                Day = Convert.ToDateTime("15-06-2018 20:00", CultureInfo.CurrentCulture),
                                Hours = 1.5
                            }
                        }
                    }
                });
            await context.SaveChangesAsync();
            var showsRepository = new ShowsRepository(context);
            //Act
            var listShows =
                await showsRepository.GetOnPeriodAsync(week: Convert
                    .ToDateTime("15-06-2018", CultureInfo.CurrentCulture).Week());
            //Assert
            Assert.True(listShows.Any());
            Assert.Contains(listShows, s => s.Name.Equals("Branden"));
        }

        [Fact]
        public async Task ExportCreateSucceeds()
        {
            var context = TheatreContextInit.InitializeContextInMemoryDb(System.Reflection.MethodBase.GetCurrentMethod()
                .DeclaringType.GUID.ToString());

            TheatreContextInit.AddTestData(context);
            await context.SaveChangesAsync();
            var showsRepository = new ShowsRepository(context);
            //Act
            var export = await showsRepository.GetExport();
            //Assert
            Assert.Contains("Hamlet", export);
        }
    }
}
