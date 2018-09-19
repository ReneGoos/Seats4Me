﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Seats4Me.API.Tests.Repositories;
using Seats4Me.Data.Model;

namespace Seats4Me.API.Tests.Services
{
    public class TicketsServicesTests
    {
        private async Task<TheatreContext> SetupData(string name)
        {
            var context = TheatreContextInit.InitializeContextInMemoryDb(name);
            TheatreContextInit.AddTestData(context);

            var today = DateTime.Today;
            await context.Shows.AddAsync(new Show
                                         {
                                             Name = "De Woef side story",
                                             RegularPrice = 15,
                                             RegularDiscountPrice = 12,
                                             TimeSlots = new List<TimeSlot>
                                                         {
                                                             new TimeSlot
                                                             {
                                                                 Day = new DateTime(today.Year, today.Month, today.Day, 14, 0, 0).AddDays(2),
                                                                 Hours = 1.5,
                                                                 PromoPrice = 10
                                                             }
                                                         }
                                         });

            for (var row = 1; row <= 15; row++)
            {
                for (var chair = 1; chair <= 6; chair++)
                {
                    await context.Seats.AddAsync(new Seat
                                                 {
                                                     Row = row,
                                                     Chair = chair
                                                 });
                }
            }

            await context.Seats4MeUsers.AddAsync(new Seats4MeUser
                                                 {
                                                     Name = "René",
                                                     Email = "rene@seats4me.com",
                                                     Roles = "visitor"
                                                 });

            await context.SaveChangesAsync();

            return context;
        }

        /*
        [Fact]
        public async Task OrderTicketsSucceeds()
        {
            //Arrange
            var timeSlotsService = new Mock<ITimeSlotsService>();
            var ticketsRepository = new Mock<ITimeSlotSeatsRepository>();
            var seatsRepository = new Mock<ISeatsRepository>();
            var usersRepository = new Mock<IUsersRepository>();
            var ticketsService = new TicketsService(timeSlotsService.Object, ticketsRepository.Object, usersRepository.Object, seatsRepository.Object);

            var show = context.Shows.First(s => s.Name.Contains("Woef"));
            var timeSlot = show.TimeSlots.First();
            var seat = context.Seats.First(s => s.Row == 1 && s.Chair == 1);
            var user = context.Seats4MeUsers.First();

            ticketsService.ControllerContext.HttpContext = new DefaultHttpContext();
            ticketsService.ControllerContext.HttpContext.User = new TestPrincipal(new Claim(ClaimTypes.Email, user.Email));

            var ticket = new TicketOutputModel
                         {
                             TimeSlotId = timeSlot.Id,
                             SeatId = seat.Id,
                             Price = timeSlot.PromoPrice,
                             Email = user.Email,
                             Paid = true,
                             Reserved = true
                         };

            //Act
            var result = await ticketsService.UpdateAsync(ticketId, ticket,);

            //Assert
            var okResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var tickets = Assert.IsAssignableFrom<int>(okResult.Value);
            Assert.True(tickets > 0);
            Assert.Contains(context.TimeSlotSeats, s => s.Paid);
        }

        [Fact]
        public async Task OrderTicketsWrongPriceFails()
        {
            //Arrange
            var timeSlotsService = new Mock<ITimeSlotsService>();
            var ticketsRepository = new Mock<ITimeSlotSeatsRepository>();
            var seatsRepository = new Mock<ISeatsRepository>();
            var usersRepository = new Mock<IUsersRepository>();
            var ticketsService = new TicketsService(timeSlotsService.Object, ticketsRepository.Object, usersRepository.Object, seatsRepository.Object);

            var show = context.Shows.First();
            var timeSlot = show.TimeSlots.First();
            var seat = context.Seats.First(s => s.Row == 1 && s.Chair == 1);
            var user = context.Seats4MeUsers.First();

            var ticket = new TicketOutputModel
                         {
                             TimeSlotId = timeSlot.Id,
                             SeatId = seat.Id,
                             Price = 1,
                             Email = user.Email,
                             Paid = true,
                             Reserved = true
                         };

            ticketsService.ControllerContext.HttpContext = new DefaultHttpContext();
            ticketsService.ControllerContext.HttpContext.User = new TestPrincipal(new Claim(ClaimTypes.Email, user.Email));

            //Act
            var result = await ticketsService.PostAsync(ticket);

            //Assert
            var badResult = Assert.IsAssignableFrom<BadRequestObjectResult>(result);
            var badMessage = Assert.IsAssignableFrom<string>(badResult.Value);
            Assert.Contains("price", badMessage);
        }

        [Fact]
        public async Task PayReservedTicketsSucceeds()
        {
            //Arrange
            var timeSlotsService = new Mock<ITimeSlotsService>();
            var ticketsRepository = new Mock<ITimeSlotSeatsRepository>();
            var seatsRepository = new Mock<ISeatsRepository>();
            var usersRepository = new Mock<IUsersRepository>();
            var ticketsService = new TicketsService(timeSlotsService.Object, ticketsRepository.Object, usersRepository.Object, seatsRepository.Object);

            var show = context.Shows.First();
            var timeSlot = show.TimeSlots.First();
            var seat = context.Seats.First(s => s.Row == 1 && s.Chair == 1);
            var user = context.Seats4MeUsers.First();

            var timeSlotSeat = context.TimeSlotSeats.Add(new TimeSlotSeat
                                                         {
                                                             TimeSlotId = timeSlot.Id,
                                                             SeatId = seat.Id,
                                                             Price = show.RegularPrice,
                                                             Seats4MeUserId = user.Id,
                                                             Paid = false,
                                                             Reserved = true
                                                         });

            await context.SaveChangesAsync();
            var timeSlotSeatId = timeSlotSeat.Entity.Id;

            var ticket = new TicketOutputModel
                         {
                             TimeSlotSeatId = timeSlotSeatId,
                             TimeSlotId = timeSlot.Id,
                             SeatId = seat.Id,
                             Price = show.RegularPrice,
                             Email = user.Email,
                             Paid = true,
                             Reserved = true
                         };

            ticketsService.ControllerContext.HttpContext = new DefaultHttpContext();
            ticketsService.ControllerContext.HttpContext.User = new TestPrincipal(new Claim(ClaimTypes.Email, user.Email));

            //Act
            var result = await ticketsService.PutAsync(timeSlotSeatId, ticket);

            //Assert
            Assert.IsAssignableFrom<OkResult>(result);
            Assert.Contains(context.TimeSlotSeats, s => s.Id == timeSlotSeatId && s.Paid && s.Reserved);
        }

        [Fact]
        public async Task PostTicketSucceeds()
        {
            //Arrange
            var context = await SetupData(MethodBase.GetCurrentMethod().DeclaringType.GUID.ToString());

            var timeSlotsService = new Mock<ITimeSlotsService>();
            var ticketsRepository = new Mock<ITimeSlotSeatsRepository>();
            var seatsRepository = new Mock<ISeatsRepository>();
            var usersRepository = new Mock<IUsersRepository>();
            var ticketsService = new TicketsService(timeSlotsService.Object, ticketsRepository.Object, usersRepository.Object, seatsRepository.Object);

            var show = context.Shows.First(s => s.Name.Contains("Woef"));
            var timeSlot = show.TimeSlots.First();
            var seat = context.Seats.First(s => s.Row == 1 && s.Chair == 1);
            var user = context.Seats4MeUsers.First();

            var ticket = new TicketInputModel
                         {
                             Row = 1,
                             Chair = 1,
                             Discount = false,
                             Paid = true,
                             Reserved = true
                         };

            //Act
            var result = await ticketsService.AddAsync(showId, timeSlotId, ticket, "test@test.com");

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
            var timeSlotsService = new Mock<ITimeSlotsService>();
            var ticketsRepository = new Mock<ITimeSlotSeatsRepository>();
            var seatsRepository = new Mock<ISeatsRepository>();
            var usersRepository = new Mock<IUsersRepository>();
            var ticketsService = new TicketsService(timeSlotsService.Object, ticketsRepository.Object, usersRepository.Object, seatsRepository.Object);

            var show = context.Shows.First();
            var timeSlot = show.TimeSlots.First();
            var seat = context.Seats.First(s => s.Row == 1 && s.Chair == 1);
            var user = context.Seats4MeUsers.First();

            context.TimeSlotSeats.Add(new TimeSlotSeat
                                      {
                                          TimeSlotId = timeSlot.Id,
                                          SeatId = seat.Id,
                                          Price = show.RegularPrice,
                                          Seats4MeUserId = user.Id,
                                          Paid = false,
                                          Reserved = true
                                      });

            await context.SaveChangesAsync();

            ticketsService.ControllerContext.HttpContext = new DefaultHttpContext();
            ticketsService.ControllerContext.HttpContext.User = new TestPrincipal(new Claim(ClaimTypes.Email, user.Email));

            //Act
            var result = await ticketsService.GetTicketAsync();

            //Assert
            var okResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var tickets = Assert.IsAssignableFrom<IEnumerable<TicketOutputModel>>(okResult.Value);
            Assert.True(tickets.Any());
        }
        */
    }
}