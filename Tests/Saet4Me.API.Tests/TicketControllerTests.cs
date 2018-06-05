using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Seats4Me.API.Data;
using Seats4Me.API.Model;
using Seats4Me.Data.Model;
using Xunit;

namespace Seats4Me.API.Tests
{
    public class TicketControllerTests
    {
        private async Task<TheatreContext> SetupData(string name)
        {
            var context = TheatreContextInit.InitializeContextInMemoryDb(name);
            await context.Shows.AddAsync(new Show()
            {
                Name = "De Woef side story",
                RegularPrice = 15,
                RegularDiscountPrice = 12,
                TimeSlots = new List<TimeSlot>()
                {
                    new TimeSlot()
                    {
                        Day = Convert.ToDateTime("01-06-2018 14:00", CultureInfo.CurrentCulture),
                        Hours = 1.5,
                        PromoPrice = 10
                    }
                }
            });
            for (var row = 1; row <= 15; row++)
            for (var chair = 1; chair <= 6; chair++)
                context.Seats.Add(new Seats4Me.Data.Model.Seat()
                {
                    Row = row,
                    Chair = chair
                });
            await context.SaveChangesAsync();
            return context;
        }

        [Fact]
        public async Task OrderTicketsSucceeds()
        {
            //Arrange
            var context = await SetupData(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.GUID.ToString());

            var ticketsRepository = new TicketsRepository(context);
            var ticketsCtrl = new Controllers.TicketsController(ticketsRepository);

            var show = context.Shows.First();
            var timeSlot = show.TimeSlots.First();
            var seat = context.Seats.FirstOrDefault(s => s.Row == 1 && s.Chair == 1);

            var ticket = new Ticket()
            {
                TimeSlotId = timeSlot.TimeSlotId,
                SeatId = seat.SeatId,
                Price = timeSlot.PromoPrice,
                CustomerEmail = "test@test.nl",
                Paid = true,
                Reserved = true
            };


            //Act
            var result = await ticketsCtrl.PostAsync(ticket);
            //Assert
            var okResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var tickets = Assert.IsAssignableFrom<int>(okResult.Value);
            Assert.True(tickets > 0);
            Assert.Contains(context.TimeSlotSeats, s => s.Paid);
        }

        [Fact]
        public async Task OrderedTicketsDisplaySucceeds()
        {
            //Arrange
            var context = await SetupData(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.GUID.ToString());

            var ticketsRepository = new TicketsRepository(context);
            var ticketsCtrl = new Controllers.TicketsController(ticketsRepository);

            var show = context.Shows.First();
            var timeSlot = show.TimeSlots.First();
            var seat = context.Seats.FirstOrDefault(s => s.Row == 1 && s.Chair == 1);

            var ticket = new Ticket()
            {
                TimeSlotId = timeSlot.TimeSlotId,
                SeatId = seat.SeatId,
                Price = timeSlot.PromoPrice,
                CustomerEmail = "test@test.nl",
                Paid = true,
                Reserved = true
            };

            //Act
            var result = await ticketsCtrl.PostAsync(ticket);
            //Assert
            var okResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var tickets = Assert.IsAssignableFrom<int>(okResult.Value);
            Assert.True(tickets > 0);
            Assert.Contains(context.TimeSlotSeats, s => s.Paid);
        }

        [Fact]
        public async Task ReserveTicketsSucceeds()
        {
            //Arrange
            var context = await SetupData(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.GUID.ToString());

            var ticketsRepository = new TicketsRepository(context);
            var ticketsCtrl = new Controllers.TicketsController(ticketsRepository);

            var show = context.Shows.First();
            var timeSlot = show.TimeSlots.First();
            var seat = context.Seats.First(s => s.Row == 1 && s.Chair == 1);
            context.TimeSlotSeats.Add(new TimeSlotSeat()
            {
                TimeSlotId = timeSlot.TimeSlotId,
                SeatId = seat.SeatId,
                Price = show.RegularPrice,
                CustomerEmail = "test@test.nl",
                Paid = false,
                Reserved = true
            });
            await context.SaveChangesAsync();

            //Act
            var result = await ticketsCtrl.GetAsync("test@test.nl");
            //Assert
            var okResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var tickets = Assert.IsAssignableFrom<IEnumerable<Ticket>>(okResult.Value);
            Assert.True(tickets.Any());
        }

        [Fact]
        public async Task PayReservedTicketsSucceeds()
        {
            //Arrange
            var context = await SetupData(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.GUID.ToString());

            var ticketsRepository = new TicketsRepository(context);
            var ticketsCtrl = new Controllers.TicketsController(ticketsRepository);

            var show = context.Shows.First();
            var timeSlot = show.TimeSlots.First();
            var seat = context.Seats.First(s => s.Row == 1 && s.Chair == 1);
            var timeSlotSeat = context.TimeSlotSeats.Add(new TimeSlotSeat()
            {
                TimeSlotId = timeSlot.TimeSlotId,
                SeatId = seat.SeatId,
                Price = show.RegularPrice,
                CustomerEmail = "test@test.nl",
                Paid = false,
                Reserved = true
            });
            await context.SaveChangesAsync();
            var timeSlotSeatId = timeSlotSeat.Entity.TimeSlotSeatId;

            var ticket = new Ticket()
            {
                TimeSlotSeatId = timeSlotSeatId,
                TimeSlotId = timeSlot.TimeSlotId,
                SeatId = seat.SeatId,
                Price = show.RegularPrice,
                CustomerEmail = "test2@test.nl",
                Paid = true,
                Reserved = true
            };

            //Act
            var result = await ticketsCtrl.PutAsync(timeSlotSeatId, ticket);
            //Assert
            Assert.IsAssignableFrom<OkResult>(result);
            Assert.Contains(context.TimeSlotSeats, s => s.TimeSlotSeatId == timeSlotSeatId && s.Paid && s.Reserved);
        }

        [Fact]
        public async Task OrderTicketsWrongPriceFails()
        {
            //Arrange
            var context = await SetupData(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.GUID.ToString());

            var ticketsRepository = new TicketsRepository(context);
            var ticketsCtrl = new Controllers.TicketsController(ticketsRepository);

            var show = context.Shows.First();
            var timeSlot = show.TimeSlots.First();
            var seat = context.Seats.FirstOrDefault(s => s.Row == 1 && s.Chair == 1);

            var ticket = new Ticket()
            {
                TimeSlotId = timeSlot.TimeSlotId,
                SeatId = seat.SeatId,
                Price = 1,
                CustomerEmail = "test@test.nl",
                Paid = true,
                Reserved = true
            };

            //Act
            var result = await ticketsCtrl.PostAsync(ticket);
            //Assert
            var badResult = Assert.IsAssignableFrom<BadRequestObjectResult>(result);
            var badMessage = Assert.IsAssignableFrom<string>(badResult.Value);
            Assert.Contains("price", badMessage);
        }
    }
}
