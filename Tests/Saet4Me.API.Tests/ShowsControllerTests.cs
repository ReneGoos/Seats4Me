using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Seats4Me.API.Models;
using Seats4Me.API.Models.Output;
using Seats4Me.API.Repositories;
using Seats4Me.API.Services;
using Seats4Me.API.Common;
using Seats4Me.Data.Model;
using Xunit;

namespace Seats4Me.API.Tests
{
    public class ShowsControllerTests
    {
        private void Setup()
        {
            AutoMapper.Mapper.Initialize(cfg => cfg.AddProfile(new Seats4MeProfile()));
        }

        [Fact]
        public async Task ListShowsWhenGetTheatre()
        {
            //Arrange
            Setup();
            var showModels = new List<ShowOutputModel>()
            { new ShowOutputModel()
            {
                Name = "Hamlet"
            }
            };

            var showsService = new Mock<IShowsService>();
            showsService.Setup(s => s.GetAsync()).ReturnsAsync(showModels);

            var shows = new Controllers.ShowsController(showsService.Object);
            //Act
            var result = await shows.GetAsync();
            //Assert
            var okResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var listShows = Assert.IsAssignableFrom<IEnumerable<ShowOutputModel>>(okResult.Value);
            Assert.True(listShows.Any());
        }

        /*
        [Fact]
        public async Task ListShowsByWeekWhenGetTheatreWeek()
        {
            //Arrange
            var day = new DateTime(2019, 5, 2, 20, 0, 0);
            var context = TheatreContextInit.InitializeContextInMemoryDb(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.GUID.ToString());
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
                                    Hours =  2.5
                                },

                                new TimeSlot()
                                {
                                    Day = Convert.ToDateTime("02-06-2018 20:00", CultureInfo.CurrentCulture),
                                    Hours =  2.5
                                },
                                new TimeSlot()
                                {
                                    Day = Convert.ToDateTime("08-06-2018 20:00", CultureInfo.CurrentCulture),
                                    Hours =  3
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
                                    Day = day,
                                    Hours =  1.5
                                },
                                new TimeSlot()
                                {
                                    Day = Convert.ToDateTime("15-06-2018 20:00", CultureInfo.CurrentCulture),
                                    Hours =  2
                                }
                            }
                        }
                   });
            await context.SaveChangesAsync();
            var showsRepository = new ShowsRepository(context);
            var shows = new Controllers.ShowsController(showsRepository);
            //Act
            var result = await shows.GetWeekAsync(day.Week(), day.Year);
            //Assert
            var okResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var listShows = Assert.IsAssignableFrom<IEnumerable<TimeSlotShow>>(okResult.Value).ToList();
            Assert.Contains(listShows, s => s.Name.Equals("Branden") && Math.Abs(s.Hours - 1.5) < 0.01);
        }
        [Fact]
        public async Task ListShowsWithSoldOutCorrectByWeekWhenGetTheatreWeek()
        {
            //Arrange
            var context = TheatreContextInit.InitializeContextInMemoryDb(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.GUID.ToString());
            var show = context.Shows.Add(
                        new Show()
                        {
                            Name = "Hamlet vs Hamlet",
                            TimeSlots = new List<TimeSlot>()
                            {
                                new TimeSlot()
                                {
                                    Day = DateTime.Today.AddDays(2),
                                    Hours =  2.5
                                },

                                new TimeSlot()
                                {
                                    Day = DateTime.Today.AddDays(3),
                                    Hours =  2.5
                                },
                                new TimeSlot()
                                {
                                    Day = DateTime.Today.AddDays(10),
                                    Hours =  3
                                }
                            }
                        });
            context.Seats.RemoveRange(context.Seats);
            var seat = context.Seats.Add(
                new Seat()
                {
                    Row = 1,
                    Chair = 1
                });
            var user = context.Seats4MeUsers.Add(new Seats4MeUser()
            {
                Name = "René",
                Email = "rene@seats4me.com",
                Roles = "visitor"

            });
            context.TimeSlotSeats.Add(new TimeSlotSeat()
            {
                Seats4MeUserId = user.Entity.Id,
                Paid = true,
                Reserved = true,
                SeatId = seat.Entity.Id,
                TimeSlotId = show.Entity.TimeSlots.First().Id
            });
            await context.SaveChangesAsync();
            var showsRepository = new ShowsRepository(context);
            var shows = new Controllers.ShowsController(showsRepository);
            //Act
            var result = await shows.GetAsync();
            //Assert
            var okResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var calendarShows = Assert.IsAssignableFrom<IEnumerable<TimeSlotShow>>(okResult.Value).ToList();
            Assert.Contains(calendarShows, s => s.SoldOut);
        }

        [Fact]
        public async Task ListPromoEmptyWhenLastWeek()
        {
            //Arrange
            var day = new DateTime(2019, 5, 2, 20, 0, 0);
            var context = TheatreContextInit.InitializeContextInMemoryDb(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.GUID.ToString());
            context.Shows.AddRange(
                    new List<Show>()
                    {
                        new Show()
                        {
                            Name = "Hamlet vs Hamlet",
                            RegularPrice = 12,
                            TimeSlots = new List<TimeSlot>()
                            {
                                new TimeSlot()
                                {
                                    Day = Convert.ToDateTime("31-05-2018 20:00", CultureInfo.CurrentCulture),
                                    Hours =  2.5,
                                    PromoPrice = 12,
                                },

                                new TimeSlot()
                                {
                                    Day = Convert.ToDateTime("02-06-2018 20:00", CultureInfo.CurrentCulture),
                                    Hours =  2.5
                                },
                                new TimeSlot()
                                {
                                    Day = Convert.ToDateTime("08-06-2018 20:00", CultureInfo.CurrentCulture),
                                    Hours =  3
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
                                    Day = day,
                                    Hours =  1.5
                                },
                                new TimeSlot()
                                {
                                    Day = Convert.ToDateTime("15-06-2018 20:00", CultureInfo.CurrentCulture),
                                    Hours =  2
                                }
                            }
                        }
                   });
            await context.SaveChangesAsync();
            var showsRepository = new ShowsRepository(context);
            var shows = new Controllers.ShowsController(showsRepository);
            //Act
            var result = await shows.GetPromoAsync();
            //Assert
            var okResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var listShows = Assert.IsAssignableFrom<IEnumerable<TimeSlotShow>>(okResult.Value).ToList();
            Assert.Empty(listShows);
        }
        [Fact]
        public async Task ListPromotionsSucceeds()
        {
            //Arrange
            var context = TheatreContextInit.InitializeContextInMemoryDb(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.GUID.ToString());
            context.Shows.Add(
                        new Show()
                        {
                            Name = "Hamlet vs Hamlet",
                            RegularPrice = 12,
                            TimeSlots = new List<TimeSlot>()
                            {
                                new TimeSlot()
                                {
                                    Day = DateTime.Today.AddDays(2),
                                    Hours =  2.5,
                                    PromoPrice = 12
                                },

                                new TimeSlot()
                                {
                                    Day = DateTime.Today.AddDays(3),
                                    Hours =  2.5
                                },
                                new TimeSlot()
                                {
                                    Day = DateTime.Today.AddDays(10),
                                    Hours =  3
                                }
                            }
                        });
            await context.SaveChangesAsync();
            var showsRepository = new ShowsRepository(context);
            var shows = new Controllers.ShowsController(showsRepository);
            //Act
            var result = await shows.GetPromoAsync();
            //Assert
            var okResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var calendarShows = Assert.IsAssignableFrom<IEnumerable<TimeSlotShow>>(okResult.Value).ToList();
            Assert.Contains(calendarShows, s => s.PromoPrice > 0);
            Assert.DoesNotContain(calendarShows, s => s.PromoPrice == 0);
        }
        */
    }
}
