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
            var context = TheatreContextInit.InitializeContextInMemoryDb(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.GUID.ToString());
            var show = context.Shows.Add(
                    new Show()
                    {
                        Name = "Hamlet vs Hamlet",
                        RegularPrice = 12,
                        TimeSlots = new List<TimeSlot>()
                        {
                            new TimeSlot()
                            {
                                Day = Convert.ToDateTime("31-05-2018 20:00", CultureInfo.CurrentCulture),
                                Hours =  2
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
                Price = show.Entity.RegularPrice,
                SeatId = seat.Entity.SeatId,
                TimeSlotId = show.Entity.TimeSlots.First().TimeSlotId
            });
            //Assert
            Assert.True(timeSlotSeatId > 0);
            //Assert.True(listShows.First().Shows.Any());
        }
    }
}
