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
        public IQueryable GetCalendarAsync(TheatreContext _context)
        {
            return  _context.TimeSlots
                            .Select(wmy => new
                            {
                                Week = CultureInfo.CurrentCulture.Calendar
                                                    .GetWeekOfYear(wmy.Start,
                                                                    CalendarWeekRule.FirstFourDayWeek,
                                                                    DayOfWeek.Monday
                                                                    ),
                                wmy.Start.Month,
                                wmy.Start.Year
                            })
                            .Distinct()
                            .Select(a => new { a.Week, a.Month, a.Year })
                            .GroupJoin(
                                _context.TimeSlots
                                    .Join(_context.Shows,
                                            t => t.ShowId,
                                            s => s.ShowId,
                                            (t, s) =>
                                            new
                                            {
                                                Week = CultureInfo.CurrentCulture.Calendar
                                                        .GetWeekOfYear(t.Start,
                                                                        CalendarWeekRule.FirstFourDayWeek,
                                                                        DayOfWeek.Monday
                                                                        ),
                                                t.Start.Month,
                                                t.Start.Year,
                                                TimeSlot = t,
                                                Show = s
                                            }),
                                a => new { a.Week, a.Month, a.Year },
                                tss => new { tss.Week, tss.Month, tss.Year },
                                (a, tsc) =>
                                new CalendarShows
                                {
                                    Week = a.Week,
                                    Month = a.Month,
                                    Year = a.Year,
                                    Shows = tsc.Select(ts =>
                                      new TimeSlotShow
                                      {
                                          ShowId = ts.Show.ShowId,
                                          TimeSlotId = ts.TimeSlot.TimeSlotId,
                                          Name = ts.Show.Name,
                                          Title = ts.Show.Title,
                                          Description = ts.Show.Description,
                                          RegularPrice = ts.Show.RegularPrice,
                                          RegularDiscountPrice = ts.Show.RegularDiscountPrice,
                                          PromoPrice = ts.Show.PromoPrice,
                                          Start = ts.TimeSlot.Start,
                                          End = ts.TimeSlot.End
                                      })
                                }
                                );
        }
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
            //Act
            var listShows = await showsRepository.GetCalendarAsync();
            //Assert
            Assert.True(listShows.Any());
            Assert.True(listShows.First().Shows.Any());
        }
    }
}
