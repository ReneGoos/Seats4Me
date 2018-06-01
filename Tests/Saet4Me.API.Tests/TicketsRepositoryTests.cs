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
    public class TicketsRepositoryTests
    {
        [Fact]
        public async Task ListShowsByWeek()
        {
            //Arrange
            var context = TheatreContextInit.InitializeContextInMemoryDb();
            var show = context.Shows.Add(
                    new Show()
                    {
                        Name = "Hamlet vs Hamlet",
                        TimeSlots = new List<TimeSlot>()
                        {
                            new TimeSlot()
                            {
                                Start = Convert.ToDateTime("31-05-2018 20:00", CultureInfo.CurrentCulture),
                                Length =  2
                            }
                        }
                    });

            var seat = context.Seats.Add(
                new Seat()
                {
                    Row = 1,
                    Chair = 1
                });
            await context.SaveChangesAsync();

            var ticketsRepository = new TicketsRepository(context);
            //Act
            var timeSlotSeatId = await ticketsRepository.AddAsync(new Ticket()
            {
                Paid = true,
                CustomerEmail = "test@test.nl",
                Reserved = false,
                SeatId = seat.Entity.SeatId,
                TimeSlotId = show.Entity.TimeSlots.First().TimeSlotId
            });
            //Assert
            Assert.True(timeSlotSeatId > 0);
            //Assert.True(listShows.First().Shows.Any());
        }
    }
}
