using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using Moq;

using Seats4Me.API.Models.Input;
using Seats4Me.API.Models.Output;
using Seats4Me.API.Models.Result;
using Seats4Me.API.Repositories;
using Seats4Me.API.Services;
using Seats4Me.Data.Model;

using Xunit;

namespace Seats4Me.API.Tests.Services
{
    public class TicketsServicesTests : MapperTest
    {
        [Fact]
        public async Task AddAsyncIncorrectEmail()
        {
            //Arrange
            var ticketInputModel = new TicketInputModel();
            var seat = new Seat();
            var email = "rene@seats4me.com";

            var ticket = Mapper.Map<TimeSlotSeat>(ticketInputModel);
            var ticketResult = new TicketResult
                               {
                                   TimeSlot = new TimeSlot
                                              {
                                                  Show = new Show
                                                         {
                                                             Name = "Hamlet"
                                                         }
                                              },
                                   Seat = new Seat(),
                                   TimeSlotSeat = new TimeSlotSeat()
                               };

            var timeSlotsService = new Mock<ITimeSlotsService>();
            var timeSlotSeatsRepository = new Mock<ITimeSlotSeatsRepository>();
            var usersRepository = new Mock<IUsersRepository>();
            var seatsRepository = new Mock<ISeatsRepository>();

            timeSlotsService.Setup(s => s.GetPriceAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>())).ReturnsAsync(10);
            timeSlotSeatsRepository.Setup(s => s.AddAsync(It.IsAny<TimeSlotSeat>())).ReturnsAsync(ticket);
            timeSlotSeatsRepository.Setup(s => s.GetAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(default(TimeSlotSeat));
            timeSlotSeatsRepository.Setup(s => s.GetTicketAsync(It.IsAny<int>())).ReturnsAsync(ticketResult);
            usersRepository.Setup(s => s.GetUserAsync(It.IsAny<string>())).ReturnsAsync(default(Seats4MeUser));

            seatsRepository.Setup(s => s.GetAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(seat);

            var tickets = new TicketsService(timeSlotsService.Object, timeSlotSeatsRepository.Object, usersRepository.Object, seatsRepository.Object);

            var exception = default(Exception);

            //Act
            try
            {
                await tickets.AddAsync(1, 1, ticketInputModel, email);
            }
            catch (Exception e)
            {
                exception = e;
            }

            //Assert
            Assert.IsType<UnauthorizedAccessException>(exception);
            Assert.NotNull(exception.Message);
            timeSlotSeatsRepository.Verify(s => s.AddAsync(It.IsAny<TimeSlotSeat>()), Times.Never);
        }

        [Fact]
        public async Task AddAsyncNoEmail()
        {
            //Arrange
            var ticketInputModel = new TicketInputModel();
            var seat = new Seat();

            var ticket = Mapper.Map<TimeSlotSeat>(ticketInputModel);
            var ticketResult = new TicketResult
                               {
                                   TimeSlot = new TimeSlot
                                              {
                                                  Show = new Show
                                                         {
                                                             Name = "Hamlet"
                                                         }
                                              },
                                   Seat = new Seat(),
                                   TimeSlotSeat = new TimeSlotSeat()
                               };

            var timeSlotsService = new Mock<ITimeSlotsService>();
            var timeSlotSeatsRepository = new Mock<ITimeSlotSeatsRepository>();
            var usersRepository = new Mock<IUsersRepository>();
            var seatsRepository = new Mock<ISeatsRepository>();

            timeSlotsService.Setup(s => s.GetPriceAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>())).ReturnsAsync(10);
            timeSlotSeatsRepository.Setup(s => s.AddAsync(It.IsAny<TimeSlotSeat>())).ReturnsAsync(ticket);
            timeSlotSeatsRepository.Setup(s => s.GetAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(default(TimeSlotSeat));
            timeSlotSeatsRepository.Setup(s => s.GetTicketAsync(It.IsAny<int>())).ReturnsAsync(ticketResult);
            usersRepository.Setup(s => s.GetUserAsync(It.IsAny<string>())).ReturnsAsync(default(Seats4MeUser));

            seatsRepository.Setup(s => s.GetAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(seat);

            var tickets = new TicketsService(timeSlotsService.Object, timeSlotSeatsRepository.Object, usersRepository.Object, seatsRepository.Object);

            var exception = default(Exception);

            //Act
            try
            {
                await tickets.AddAsync(1, 1, ticketInputModel, default(string));
            }
            catch (Exception e)
            {
                exception = e;
            }

            //Assert
            Assert.IsType<UnauthorizedAccessException>(exception);
            Assert.NotNull(exception.Message);
            timeSlotSeatsRepository.Verify(s => s.AddAsync(It.IsAny<TimeSlotSeat>()), Times.Never);
        }

        [Fact]
        public async Task AddAsyncNoSave()
        {
            //Arrange
            var ticketInputModel = new TicketInputModel();
            var user = new Seats4MeUser();
            var seat = new Seat();
            var email = "rene@seats4me.com";

            var ticketResult = new TicketResult
                               {
                                   TimeSlot = new TimeSlot
                                              {
                                                  Show = new Show
                                                         {
                                                             Name = "Hamlet"
                                                         }
                                              },
                                   Seat = new Seat(),
                                   TimeSlotSeat = new TimeSlotSeat()
                               };

            var timeSlotsService = new Mock<ITimeSlotsService>();
            var timeSlotSeatsRepository = new Mock<ITimeSlotSeatsRepository>();
            var usersRepository = new Mock<IUsersRepository>();
            var seatsRepository = new Mock<ISeatsRepository>();

            timeSlotsService.Setup(s => s.GetPriceAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>())).ReturnsAsync(10);
            timeSlotSeatsRepository.Setup(s => s.AddAsync(It.IsAny<TimeSlotSeat>())).ReturnsAsync(default(TimeSlotSeat));
            timeSlotSeatsRepository.Setup(s => s.GetAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(default(TimeSlotSeat));
            timeSlotSeatsRepository.Setup(s => s.GetTicketAsync(It.IsAny<int>())).ReturnsAsync(ticketResult);

            usersRepository.Setup(s => s.GetUserAsync(It.IsAny<string>())).ReturnsAsync(user);

            seatsRepository.Setup(s => s.GetAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(seat);

            var tickets = new TicketsService(timeSlotsService.Object, timeSlotSeatsRepository.Object, usersRepository.Object, seatsRepository.Object);

            //Act
            var result = await tickets.AddAsync(1, 1, ticketInputModel, email);

            //Assert
            Assert.Null(result);
            timeSlotSeatsRepository.Verify(s => s.AddAsync(It.IsAny<TimeSlotSeat>()), Times.Once);
        }

        [Fact]
        public async Task AddAsyncNoSeat()
        {
            //Arrange
            //Arrange
            var ticketInputModel = new TicketInputModel();
            var user = new Seats4MeUser();
            var email = "rene@seats4me.com";

            var ticket = Mapper.Map<TimeSlotSeat>(ticketInputModel);
            var ticketResult = new TicketResult
                               {
                                   TimeSlot = new TimeSlot
                                              {
                                                  Show = new Show
                                                         {
                                                             Name = "Hamlet"
                                                         }
                                              },
                                   Seat = new Seat(),
                                   TimeSlotSeat = new TimeSlotSeat()
                               };

            var timeSlotsService = new Mock<ITimeSlotsService>();
            var timeSlotSeatsRepository = new Mock<ITimeSlotSeatsRepository>();
            var usersRepository = new Mock<IUsersRepository>();
            var seatsRepository = new Mock<ISeatsRepository>();

            timeSlotsService.Setup(s => s.GetPriceAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>())).ReturnsAsync(10);
            timeSlotSeatsRepository.Setup(s => s.AddAsync(It.IsAny<TimeSlotSeat>())).ReturnsAsync(ticket);
            timeSlotSeatsRepository.Setup(s => s.GetAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(default(TimeSlotSeat));
            timeSlotSeatsRepository.Setup(s => s.GetTicketAsync(It.IsAny<int>())).ReturnsAsync(ticketResult);
            usersRepository.Setup(s => s.GetUserAsync(It.IsAny<string>())).ReturnsAsync(user);

            seatsRepository.Setup(s => s.GetAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(default(Seat));

            var tickets = new TicketsService(timeSlotsService.Object, timeSlotSeatsRepository.Object, usersRepository.Object, seatsRepository.Object);

            var exception = default(Exception);

            //Act
            try
            {
                await tickets.AddAsync(1, 1, ticketInputModel, email);
            }
            catch (Exception e)
            {
                exception = e;
            }

            //Assert
            Assert.IsType<ArgumentException>(exception);
            Assert.NotNull(exception.Message);
            timeSlotSeatsRepository.Verify(s => s.AddAsync(It.IsAny<TimeSlotSeat>()), Times.Never);
        }

        [Fact]
        public async Task AddAsyncNoTicketAvailable()
        {
            //Arrange
            var ticketInputModel = new TicketInputModel();
            var user = new Seats4MeUser();
            var seat = new Seat();
            var email = "rene@seats4me.com";

            var ticket = Mapper.Map<TimeSlotSeat>(ticketInputModel);
            var ticketResult = new TicketResult
                               {
                                   TimeSlot = new TimeSlot
                                              {
                                                  Show = new Show
                                                         {
                                                             Name = "Hamlet"
                                                         }
                                              },
                                   Seat = new Seat(),
                                   TimeSlotSeat = new TimeSlotSeat()
                               };

            var timeSlotsService = new Mock<ITimeSlotsService>();
            var timeSlotSeatsRepository = new Mock<ITimeSlotSeatsRepository>();
            var usersRepository = new Mock<IUsersRepository>();
            var seatsRepository = new Mock<ISeatsRepository>();

            timeSlotsService.Setup(s => s.GetPriceAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>())).ReturnsAsync(10);
            timeSlotSeatsRepository.Setup(s => s.AddAsync(It.IsAny<TimeSlotSeat>())).ReturnsAsync(ticket);
            timeSlotSeatsRepository.Setup(s => s.GetAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(ticket);
            timeSlotSeatsRepository.Setup(s => s.GetTicketAsync(It.IsAny<int>())).ReturnsAsync(ticketResult);

            usersRepository.Setup(s => s.GetUserAsync(It.IsAny<string>())).ReturnsAsync(user);

            seatsRepository.Setup(s => s.GetAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(seat);

            var tickets = new TicketsService(timeSlotsService.Object, timeSlotSeatsRepository.Object, usersRepository.Object, seatsRepository.Object);

            var exception = default(Exception);

            //Act
            try
            {
                await tickets.AddAsync(1, 1, ticketInputModel, email);
            }
            catch (Exception e)
            {
                exception = e;
            }

            //Assert
            Assert.IsType<ArgumentException>(exception);
            Assert.NotNull(exception.Message);
            timeSlotSeatsRepository.Verify(s => s.AddAsync(It.IsAny<TimeSlotSeat>()), Times.Never);
        }

        [Fact]
        public async Task AddAsyncOk()
        {
            //Arrange
            var ticketInputModel = new TicketInputModel();
            var user = new Seats4MeUser();
            var seat = new Seat();
            var email = "rene@seats4me.com";

            var ticket = Mapper.Map<TimeSlotSeat>(ticketInputModel);
            var ticketResult = new TicketResult
                               {
                                   TimeSlot = new TimeSlot
                                              {
                                                  Show = new Show
                                                         {
                                                             Name = "Hamlet"
                                                         }
                                              },
                                   Seat = new Seat(),
                                   TimeSlotSeat = new TimeSlotSeat()
                               };

            var timeSlotsService = new Mock<ITimeSlotsService>();
            var timeSlotSeatsRepository = new Mock<ITimeSlotSeatsRepository>();
            var usersRepository = new Mock<IUsersRepository>();
            var seatsRepository = new Mock<ISeatsRepository>();

            timeSlotsService.Setup(s => s.GetPriceAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>())).ReturnsAsync(10);
            timeSlotSeatsRepository.Setup(s => s.AddAsync(It.IsAny<TimeSlotSeat>())).ReturnsAsync(ticket);
            timeSlotSeatsRepository.Setup(s => s.GetAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(default(TimeSlotSeat));
            timeSlotSeatsRepository.Setup(s => s.GetTicketAsync(It.IsAny<int>())).ReturnsAsync(ticketResult);
            usersRepository.Setup(s => s.GetUserAsync(It.IsAny<string>())).ReturnsAsync(user);

            seatsRepository.Setup(s => s.GetAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(seat);

            var tickets = new TicketsService(timeSlotsService.Object, timeSlotSeatsRepository.Object, usersRepository.Object, seatsRepository.Object);

            //Act
            var result = await tickets.AddAsync(1, 1, ticketInputModel, email);

            //Assert
            Assert.IsType<TicketOutputModel>(result);
            timeSlotSeatsRepository.Verify(s => s.AddAsync(It.IsAny<TimeSlotSeat>()), Times.Once);
        }

        [Fact]
        public async Task DeleteTicketDifferentUser()
        {
            //Arrange
            var ticket = new TimeSlotSeat
                         {
                             Id = 1,
                             Seats4MeUserId = 1
                         };

            var user = new Seats4MeUser
                       {
                           Id = ticket.Seats4MeUserId + 1
                       };

            var email = "rene@seats4me.com";

            var timeSlotsService = new Mock<ITimeSlotsService>();
            var timeSlotSeatsRepository = new Mock<ITimeSlotSeatsRepository>();
            var usersRepository = new Mock<IUsersRepository>();
            var seatsRepository = new Mock<ISeatsRepository>();

            usersRepository.Setup(s => s.GetUserAsync(It.IsAny<string>())).ReturnsAsync(user);

            timeSlotSeatsRepository.Setup(s => s.GetAsync(It.IsAny<int>())).ReturnsAsync(ticket);

            timeSlotSeatsRepository.Setup(s => s.DeleteAsync(It.IsAny<TimeSlotSeat>())).Returns(Task.CompletedTask);

            var tickets = new TicketsService(timeSlotsService.Object, timeSlotSeatsRepository.Object, usersRepository.Object, seatsRepository.Object);

            var exception = default(Exception);

            //Act
            try
            {
                await tickets.DeleteAsync(1, email);

                //Assert
            }
            catch (Exception e)
            {
                exception = e;
            }

            Assert.IsType<UnauthorizedAccessException>(exception);
            Assert.NotNull(exception.Message);
            timeSlotSeatsRepository.Verify(s => s.DeleteAsync(It.IsAny<TimeSlotSeat>()), Times.Never);
        }

        [Fact]
        public async Task DeleteTicketIncorrectEmail()
        {
            //Arrange
            var ticket = new TimeSlotSeat
                         {
                             Id = 1,
                             Seats4MeUserId = 1
                         };

            var email = "rene@seats4me.com";

            var timeSlotsService = new Mock<ITimeSlotsService>();
            var timeSlotSeatsRepository = new Mock<ITimeSlotSeatsRepository>();
            var usersRepository = new Mock<IUsersRepository>();
            var seatsRepository = new Mock<ISeatsRepository>();

            usersRepository.Setup(s => s.GetUserAsync(It.IsAny<string>())).ReturnsAsync(default(Seats4MeUser));

            timeSlotSeatsRepository.Setup(s => s.GetAsync(It.IsAny<int>())).ReturnsAsync(ticket);

            timeSlotSeatsRepository.Setup(s => s.DeleteAsync(It.IsAny<TimeSlotSeat>())).Returns(Task.CompletedTask);

            var tickets = new TicketsService(timeSlotsService.Object, timeSlotSeatsRepository.Object, usersRepository.Object, seatsRepository.Object);

            var exception = default(Exception);

            //Act
            try
            {
                await tickets.DeleteAsync(1, email);

                //Assert
            }
            catch (Exception e)
            {
                exception = e;
            }

            Assert.IsType<UnauthorizedAccessException>(exception);
            Assert.NotNull(exception.Message);
            timeSlotSeatsRepository.Verify(s => s.DeleteAsync(It.IsAny<TimeSlotSeat>()), Times.Never);
        }

        [Fact]
        public async Task DeleteTicketNoTicket()
        {
            //Arrange
            var ticket = new TimeSlotSeat
                         {
                             Id = 1,
                             Seats4MeUserId = 1
                         };

            var user = new Seats4MeUser
                       {
                           Id = ticket.Seats4MeUserId
                       };

            var email = "rene@seats4me.com";

            var timeSlotsService = new Mock<ITimeSlotsService>();
            var timeSlotSeatsRepository = new Mock<ITimeSlotSeatsRepository>();
            var usersRepository = new Mock<IUsersRepository>();
            var seatsRepository = new Mock<ISeatsRepository>();

            usersRepository.Setup(s => s.GetUserAsync(It.IsAny<string>())).ReturnsAsync(user);

            timeSlotSeatsRepository.Setup(s => s.GetAsync(It.IsAny<int>())).ReturnsAsync(default(TimeSlotSeat));

            timeSlotSeatsRepository.Setup(s => s.DeleteAsync(It.IsAny<TimeSlotSeat>())).Returns(Task.CompletedTask);

            var tickets = new TicketsService(timeSlotsService.Object, timeSlotSeatsRepository.Object, usersRepository.Object, seatsRepository.Object);

            //Act
            await tickets.DeleteAsync(1, email);

            //Assert
            timeSlotSeatsRepository.Verify(s => s.DeleteAsync(It.IsAny<TimeSlotSeat>()), Times.Never);
        }

        [Fact]
        public async Task DeleteTicketSucceeds()
        {
            //Arrange
            var ticket = new TimeSlotSeat
                         {
                             Id = 1,
                             Seats4MeUserId = 1
                         };

            var user = new Seats4MeUser
                       {
                           Id = ticket.Seats4MeUserId
                       };

            var email = "rene@seats4me.com";

            var timeSlotsService = new Mock<ITimeSlotsService>();
            var timeSlotSeatsRepository = new Mock<ITimeSlotSeatsRepository>();
            var usersRepository = new Mock<IUsersRepository>();
            var seatsRepository = new Mock<ISeatsRepository>();

            usersRepository.Setup(s => s.GetUserAsync(It.IsAny<string>())).ReturnsAsync(user);

            timeSlotSeatsRepository.Setup(s => s.GetAsync(It.IsAny<int>())).ReturnsAsync(ticket);

            timeSlotSeatsRepository.Setup(s => s.DeleteAsync(It.IsAny<TimeSlotSeat>())).Returns(Task.CompletedTask);

            var tickets = new TicketsService(timeSlotsService.Object, timeSlotSeatsRepository.Object, usersRepository.Object, seatsRepository.Object);

            //Act
            await tickets.DeleteAsync(1, email);

            //Assert
            timeSlotSeatsRepository.Verify(s => s.DeleteAsync(It.IsAny<TimeSlotSeat>()), Times.Once);
        }

        [Fact]
        public async Task GetAsyncByTimeslotListSucceeds()
        {
            //Arrange
            var ticketModels = new List<TicketResult>
                               {
                                   new TicketResult(),
                                   new TicketResult()
                               };

            var timeSlotsService = new Mock<ITimeSlotsService>();
            var timeSlotSeatsRepository = new Mock<ITimeSlotSeatsRepository>();
            var usersRepository = new Mock<IUsersRepository>();
            var seatsRepository = new Mock<ISeatsRepository>();

            timeSlotSeatsRepository.Setup(s => s.GetTicketsByTimeSlotAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(ticketModels);

            var tickets = new TicketsService(timeSlotsService.Object, timeSlotSeatsRepository.Object, usersRepository.Object, seatsRepository.Object);

            //Act
            var result = await tickets.GetTicketsByTimeSlotAsync(1, 1);

            //Assert
            var listTickets = Assert.IsAssignableFrom<IEnumerable<TicketOutputModel>>(result);
            Assert.Equal(2, listTickets.Count());
            timeSlotSeatsRepository.Verify(s => s.GetTicketsByTimeSlotAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task GetAsyncByUserListSucceeds()
        {
            //Arrange
            var email = "rene@seats4me.com";
            var user = new Seats4MeUser();

            var ticketModels = new List<TicketResult>
                               {
                                   new TicketResult
                                   {
                                       TimeSlot = new TimeSlot
                                                  {
                                                      Show = new Show
                                                             {
                                                                 Name = "Hamlet"
                                                             }
                                                  },
                                       Seat = new Seat(),
                                       TimeSlotSeat = new TimeSlotSeat()
                                   },
                                   new TicketResult
                                   {
                                       TimeSlot = new TimeSlot
                                                  {
                                                      Show = new Show
                                                             {
                                                                 Name = "Hamlet vs Hamlet"
                                                             }
                                                  },
                                       Seat = new Seat(),
                                       TimeSlotSeat = new TimeSlotSeat()
                                   }
                               };

            var timeSlotsService = new Mock<ITimeSlotsService>();
            var timeSlotSeatsRepository = new Mock<ITimeSlotSeatsRepository>();
            var usersRepository = new Mock<IUsersRepository>();
            var seatsRepository = new Mock<ISeatsRepository>();

            timeSlotSeatsRepository.Setup(s => s.GetTicketsByUserAsync(It.IsAny<int>())).ReturnsAsync(ticketModels);
            usersRepository.Setup(s => s.GetUserAsync(It.IsAny<string>())).ReturnsAsync(user);
            var tickets = new TicketsService(timeSlotsService.Object, timeSlotSeatsRepository.Object, usersRepository.Object, seatsRepository.Object);

            //Act
            var result = await tickets.GetTicketsByUserAsync(email);

            //Assert
            var listTickets = Assert.IsAssignableFrom<IEnumerable<TicketOutputModel>>(result);
            Assert.Equal(2, listTickets.Count());
            timeSlotSeatsRepository.Verify(s => s.GetTicketsByUserAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task GetAsyncSucceeds()
        {
            //Arrange
            var ticket = new TicketResult
                         {
                             TimeSlot = new TimeSlot
                                        {
                                            Show = new Show
                                                   {
                                                       Name = "Hamlet"
                                                   }
                                        },
                             Seat = new Seat(),
                             TimeSlotSeat = new TimeSlotSeat()
                         };

            var timeSlotsService = new Mock<ITimeSlotsService>();
            var timeSlotSeatsRepository = new Mock<ITimeSlotSeatsRepository>();
            var usersRepository = new Mock<IUsersRepository>();
            var seatsRepository = new Mock<ISeatsRepository>();
            timeSlotSeatsRepository.Setup(s => s.GetTicketAsync(It.IsAny<int>())).ReturnsAsync(ticket);

            var tickets = new TicketsService(timeSlotsService.Object, timeSlotSeatsRepository.Object, usersRepository.Object, seatsRepository.Object);

            //Act
            var result = await tickets.GetTicketAsync(1);

            //Assert
            var ticketModel = Assert.IsType<TicketOutputModel>(result);
            Assert.Equal("Hamlet", ticketModel.Name);
            timeSlotSeatsRepository.Verify(s => s.GetTicketAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsyncDifferentUser()
        {
            //Arrange
            var ticket = new TimeSlotSeat
                         {
                             Id = 1,
                             Seats4MeUserId = 1,
                             Price = 0,
                             TimeSlot = new TimeSlot()
                         };

            var ticketInputModel = new TicketInputModel();
            var ticketResult = new TicketResult
                               {
                                   TimeSlot = new TimeSlot
                                              {
                                                  Show = new Show
                                                         {
                                                             Name = "Hamlet"
                                                         }
                                              },
                                   Seat = new Seat(),
                                   TimeSlotSeat = new TimeSlotSeat()
                               };

            var user = new Seats4MeUser
                       {
                           Id = ticket.Seats4MeUserId + 1
                       };

            var email = "rene@seats4me.com";
            var seat = new Seat();

            var timeSlotsService = new Mock<ITimeSlotsService>();
            var timeSlotSeatsRepository = new Mock<ITimeSlotSeatsRepository>();
            var usersRepository = new Mock<IUsersRepository>();
            var seatsRepository = new Mock<ISeatsRepository>();

            timeSlotsService.Setup(s => s.GetPriceAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>())).ReturnsAsync(10);
            timeSlotSeatsRepository.Setup(s => s.UpdateAsync(It.IsAny<TimeSlotSeat>())).ReturnsAsync(ticket);
            timeSlotSeatsRepository.Setup(s => s.GetAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(default(TimeSlotSeat));
            timeSlotSeatsRepository.Setup(s => s.GetAsync(It.IsAny<int>())).ReturnsAsync(ticket);

            timeSlotSeatsRepository.Setup(s => s.GetTicketAsync(It.IsAny<int>())).ReturnsAsync(ticketResult);

            usersRepository.Setup(s => s.GetUserAsync(It.IsAny<string>())).ReturnsAsync(user);

            seatsRepository.Setup(s => s.GetAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(seat);

            var tickets = new TicketsService(timeSlotsService.Object, timeSlotSeatsRepository.Object, usersRepository.Object, seatsRepository.Object);

            var exception = default(Exception);

            try
            {
                await tickets.UpdateAsync(1, ticketInputModel, email);
            }
            catch (Exception e)
            {
                exception = e;
            }

            Assert.IsType<UnauthorizedAccessException>(exception);
            Assert.NotNull(exception.Message);
            timeSlotSeatsRepository.Verify(s => s.UpdateAsync(It.IsAny<TimeSlotSeat>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsyncInvalidUser()
        {
            //Arrange
            var ticket = new TimeSlotSeat
                         {
                             Id = 1,
                             Seats4MeUserId = 1,
                             Price = 0,
                             TimeSlot = new TimeSlot()
                         };

            var ticketInputModel = new TicketInputModel();
            var ticketResult = new TicketResult
                               {
                                   TimeSlot = new TimeSlot
                                              {
                                                  Show = new Show
                                                         {
                                                             Name = "Hamlet"
                                                         }
                                              },
                                   Seat = new Seat(),
                                   TimeSlotSeat = new TimeSlotSeat()
                               };

            var email = "rene@seats4me.com";
            var seat = new Seat();

            var timeSlotsService = new Mock<ITimeSlotsService>();
            var timeSlotSeatsRepository = new Mock<ITimeSlotSeatsRepository>();
            var usersRepository = new Mock<IUsersRepository>();
            var seatsRepository = new Mock<ISeatsRepository>();

            timeSlotsService.Setup(s => s.GetPriceAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>())).ReturnsAsync(10);
            timeSlotSeatsRepository.Setup(s => s.UpdateAsync(It.IsAny<TimeSlotSeat>())).ReturnsAsync(ticket);
            timeSlotSeatsRepository.Setup(s => s.GetAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(default(TimeSlotSeat));
            timeSlotSeatsRepository.Setup(s => s.GetAsync(It.IsAny<int>())).ReturnsAsync(ticket);

            timeSlotSeatsRepository.Setup(s => s.GetTicketAsync(It.IsAny<int>())).ReturnsAsync(ticketResult);

            usersRepository.Setup(s => s.GetUserAsync(It.IsAny<string>())).ReturnsAsync(default(Seats4MeUser));

            seatsRepository.Setup(s => s.GetAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(seat);

            var tickets = new TicketsService(timeSlotsService.Object, timeSlotSeatsRepository.Object, usersRepository.Object, seatsRepository.Object);

            var exception = default(Exception);

            try
            {
                await tickets.UpdateAsync(1, ticketInputModel, email);
            }
            catch (Exception e)
            {
                exception = e;
            }

            Assert.IsType<UnauthorizedAccessException>(exception);
            Assert.NotNull(exception.Message);
            timeSlotSeatsRepository.Verify(s => s.UpdateAsync(It.IsAny<TimeSlotSeat>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsyncNoAvailableTicket()
        {
            //Arrange
            var ticket = new TimeSlotSeat
                         {
                             Id = 1,
                             Seats4MeUserId = 1,
                             Price = 0,
                             TimeSlot = new TimeSlot()
                         };

            var ticketInputModel = new TicketInputModel();
            var ticketResult = new TicketResult
                               {
                                   TimeSlot = new TimeSlot
                                              {
                                                  Show = new Show
                                                         {
                                                             Name = "Hamlet"
                                                         }
                                              },
                                   Seat = new Seat(),
                                   TimeSlotSeat = new TimeSlotSeat()
                               };

            var user = new Seats4MeUser
                       {
                           Id = ticket.Seats4MeUserId
                       };

            var email = "rene@seats4me.com";
            var seat = new Seat();

            var timeSlotsService = new Mock<ITimeSlotsService>();
            var timeSlotSeatsRepository = new Mock<ITimeSlotSeatsRepository>();
            var usersRepository = new Mock<IUsersRepository>();
            var seatsRepository = new Mock<ISeatsRepository>();

            timeSlotsService.Setup(s => s.GetPriceAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>())).ReturnsAsync(10);
            timeSlotSeatsRepository.Setup(s => s.UpdateAsync(It.IsAny<TimeSlotSeat>())).ReturnsAsync(ticket);
            timeSlotSeatsRepository.Setup(s => s.GetAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(ticket);
            timeSlotSeatsRepository.Setup(s => s.GetAsync(It.IsAny<int>())).ReturnsAsync(ticket);

            timeSlotSeatsRepository.Setup(s => s.GetTicketAsync(It.IsAny<int>())).ReturnsAsync(ticketResult);

            usersRepository.Setup(s => s.GetUserAsync(It.IsAny<string>())).ReturnsAsync(user);

            seatsRepository.Setup(s => s.GetAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(seat);

            var tickets = new TicketsService(timeSlotsService.Object, timeSlotSeatsRepository.Object, usersRepository.Object, seatsRepository.Object);

            var exception = default(Exception);

            try
            {
                await tickets.UpdateAsync(1, ticketInputModel, email);
            }
            catch (Exception e)
            {
                exception = e;
            }

            Assert.IsType<ArgumentException>(exception);
            Assert.NotNull(exception.Message);
            timeSlotSeatsRepository.Verify(s => s.UpdateAsync(It.IsAny<TimeSlotSeat>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsyncNoSave()
        {
            //Arrange
            var ticket = new TimeSlotSeat
                         {
                             Id = 1,
                             Seats4MeUserId = 1,
                             Price = 0,
                             TimeSlot = new TimeSlot()
                         };

            var ticketInputModel = new TicketInputModel();
            var ticketResult = new TicketResult
                               {
                                   TimeSlot = new TimeSlot
                                              {
                                                  Show = new Show
                                                         {
                                                             Name = "Hamlet"
                                                         }
                                              },
                                   Seat = new Seat(),
                                   TimeSlotSeat = new TimeSlotSeat()
                               };

            var user = new Seats4MeUser
                       {
                           Id = ticket.Seats4MeUserId
                       };

            var email = "rene@seats4me.com";
            var seat = new Seat();

            var timeSlotsService = new Mock<ITimeSlotsService>();
            var timeSlotSeatsRepository = new Mock<ITimeSlotSeatsRepository>();
            var usersRepository = new Mock<IUsersRepository>();
            var seatsRepository = new Mock<ISeatsRepository>();

            timeSlotsService.Setup(s => s.GetPriceAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>())).ReturnsAsync(10);
            timeSlotSeatsRepository.Setup(s => s.UpdateAsync(It.IsAny<TimeSlotSeat>())).ReturnsAsync(default(TimeSlotSeat));
            timeSlotSeatsRepository.Setup(s => s.GetAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(default(TimeSlotSeat));
            timeSlotSeatsRepository.Setup(s => s.GetAsync(It.IsAny<int>())).ReturnsAsync(ticket);
            timeSlotSeatsRepository.Setup(s => s.GetTicketAsync(It.IsAny<int>())).ReturnsAsync(ticketResult);

            usersRepository.Setup(s => s.GetUserAsync(It.IsAny<string>())).ReturnsAsync(user);

            seatsRepository.Setup(s => s.GetAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(seat);

            var tickets = new TicketsService(timeSlotsService.Object, timeSlotSeatsRepository.Object, usersRepository.Object, seatsRepository.Object);

            var result = await tickets.UpdateAsync(1, ticketInputModel, email);

            Assert.Null(result);
            timeSlotSeatsRepository.Verify(s => s.UpdateAsync(It.IsAny<TimeSlotSeat>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsyncNoSeat()
        {
            //Arrange
            var ticket = new TimeSlotSeat
                         {
                             Id = 1,
                             Seats4MeUserId = 1,
                             Price = 0,
                             TimeSlot = new TimeSlot()
                         };

            var ticketInputModel = new TicketInputModel();
            var ticketResult = new TicketResult
                               {
                                   TimeSlot = new TimeSlot
                                              {
                                                  Show = new Show
                                                         {
                                                             Name = "Hamlet"
                                                         }
                                              },
                                   Seat = new Seat(),
                                   TimeSlotSeat = new TimeSlotSeat()
                               };

            var user = new Seats4MeUser
                       {
                           Id = ticket.Seats4MeUserId
                       };

            var email = "rene@seats4me.com";

            var timeSlotsService = new Mock<ITimeSlotsService>();
            var timeSlotSeatsRepository = new Mock<ITimeSlotSeatsRepository>();
            var usersRepository = new Mock<IUsersRepository>();
            var seatsRepository = new Mock<ISeatsRepository>();

            timeSlotsService.Setup(s => s.GetPriceAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>())).ReturnsAsync(10);
            timeSlotSeatsRepository.Setup(s => s.UpdateAsync(It.IsAny<TimeSlotSeat>())).ReturnsAsync(ticket);
            timeSlotSeatsRepository.Setup(s => s.GetAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(default(TimeSlotSeat));
            timeSlotSeatsRepository.Setup(s => s.GetAsync(It.IsAny<int>())).ReturnsAsync(ticket);
            timeSlotSeatsRepository.Setup(s => s.GetTicketAsync(It.IsAny<int>())).ReturnsAsync(ticketResult);

            usersRepository.Setup(s => s.GetUserAsync(It.IsAny<string>())).ReturnsAsync(user);

            seatsRepository.Setup(s => s.GetAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(default(Seat));

            var tickets = new TicketsService(timeSlotsService.Object, timeSlotSeatsRepository.Object, usersRepository.Object, seatsRepository.Object);

            var exception = default(Exception);

            try
            {
                await tickets.UpdateAsync(1, ticketInputModel, email);
            }
            catch (Exception e)
            {
                exception = e;
            }

            Assert.IsType<ArgumentException>(exception);
            Assert.NotNull(exception.Message);
            timeSlotSeatsRepository.Verify(s => s.UpdateAsync(It.IsAny<TimeSlotSeat>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsyncNoTicket()
        {
            //Arrange
            var ticket = new TimeSlotSeat
                         {
                             Id = 1,
                             Seats4MeUserId = 1,
                             Price = 0,
                             TimeSlot = new TimeSlot()
                         };

            var ticketInputModel = new TicketInputModel();
            var ticketResult = new TicketResult
                               {
                                   TimeSlot = new TimeSlot
                                              {
                                                  Show = new Show
                                                         {
                                                             Name = "Hamlet"
                                                         }
                                              },
                                   Seat = new Seat(),
                                   TimeSlotSeat = new TimeSlotSeat()
                               };

            var user = new Seats4MeUser
                       {
                           Id = ticket.Seats4MeUserId + 1
                       };

            var email = "rene@seats4me.com";
            var seat = new Seat();

            var timeSlotsService = new Mock<ITimeSlotsService>();
            var timeSlotSeatsRepository = new Mock<ITimeSlotSeatsRepository>();
            var usersRepository = new Mock<IUsersRepository>();
            var seatsRepository = new Mock<ISeatsRepository>();

            timeSlotsService.Setup(s => s.GetPriceAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>())).ReturnsAsync(10);
            timeSlotSeatsRepository.Setup(s => s.UpdateAsync(It.IsAny<TimeSlotSeat>())).ReturnsAsync(ticket);
            timeSlotSeatsRepository.Setup(s => s.GetAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(default(TimeSlotSeat));
            timeSlotSeatsRepository.Setup(s => s.GetAsync(It.IsAny<int>())).ReturnsAsync(default(TimeSlotSeat));
            timeSlotSeatsRepository.Setup(s => s.GetTicketAsync(It.IsAny<int>())).ReturnsAsync(ticketResult);

            usersRepository.Setup(s => s.GetUserAsync(It.IsAny<string>())).ReturnsAsync(user);

            seatsRepository.Setup(s => s.GetAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(seat);

            var tickets = new TicketsService(timeSlotsService.Object, timeSlotSeatsRepository.Object, usersRepository.Object, seatsRepository.Object);

            var exception = default(Exception);

            try
            {
                await tickets.UpdateAsync(1, ticketInputModel, email);
            }
            catch (Exception e)
            {
                exception = e;
            }

            Assert.IsType<ArgumentException>(exception);
            Assert.NotNull(exception.Message);
            timeSlotSeatsRepository.Verify(s => s.UpdateAsync(It.IsAny<TimeSlotSeat>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsyncSucceeds()
        {
            //Arrange
            var ticket = new TimeSlotSeat
                         {
                             Id = 1,
                             Seats4MeUserId = 1,
                             Price = 0,
                             TimeSlot = new TimeSlot()
                         };

            var ticketInputModel = new TicketInputModel();
            var ticketResult = new TicketResult
                               {
                                   TimeSlot = new TimeSlot
                                              {
                                                  Show = new Show
                                                         {
                                                             Name = "Hamlet"
                                                         }
                                              },
                                   Seat = new Seat(),
                                   TimeSlotSeat = new TimeSlotSeat()
                               };

            var user = new Seats4MeUser
                       {
                           Id = ticket.Seats4MeUserId
                       };

            var email = "rene@seats4me.com";
            var seat = new Seat();

            var timeSlotsService = new Mock<ITimeSlotsService>();
            var timeSlotSeatsRepository = new Mock<ITimeSlotSeatsRepository>();
            var usersRepository = new Mock<IUsersRepository>();
            var seatsRepository = new Mock<ISeatsRepository>();

            timeSlotsService.Setup(s => s.GetPriceAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>())).ReturnsAsync(10);
            timeSlotSeatsRepository.Setup(s => s.UpdateAsync(It.IsAny<TimeSlotSeat>())).ReturnsAsync(ticket);
            timeSlotSeatsRepository.Setup(s => s.GetAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(default(TimeSlotSeat));
            timeSlotSeatsRepository.Setup(s => s.GetAsync(It.IsAny<int>())).ReturnsAsync(ticket);
            timeSlotSeatsRepository.Setup(s => s.GetTicketAsync(It.IsAny<int>())).ReturnsAsync(ticketResult);

            usersRepository.Setup(s => s.GetUserAsync(It.IsAny<string>())).ReturnsAsync(user);

            seatsRepository.Setup(s => s.GetAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(seat);

            var tickets = new TicketsService(timeSlotsService.Object, timeSlotSeatsRepository.Object, usersRepository.Object, seatsRepository.Object);

            var result = await tickets.UpdateAsync(1, ticketInputModel, email);

            Assert.IsType<TicketOutputModel>(result);
            timeSlotSeatsRepository.Verify(s => s.UpdateAsync(It.IsAny<TimeSlotSeat>()), Times.Once);
        }
    }
}