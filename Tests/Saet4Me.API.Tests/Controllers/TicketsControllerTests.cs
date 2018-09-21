using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Moq;

using Seats4Me.API.Controllers;
using Seats4Me.API.Models.Input;
using Seats4Me.API.Models.Output;
using Seats4Me.API.Services;

using Xunit;

namespace Seats4Me.API.Tests.Controllers
{
    public class TestPrincipal : ClaimsPrincipal
    {
        public TestPrincipal(params Claim[] claims)
            : base(new TestIdentity(claims))
        {
        }
    }

    public class TestIdentity : ClaimsIdentity
    {
        public TestIdentity(params Claim[] claims)
            : base(claims)
        {
        }
    }

    public class TicketsControllerTests
    {
        [Fact]
        public async Task DeleteTicketSucceeds()
        {
            //Arrange
            var ticketsService = new Mock<ITicketsService>();
            var timeSlotsService = new Mock<ITimeSlotsService>();
            var showsService = new Mock<IShowsService>();
            ticketsService.Setup(s => s.DeleteAsync(It.IsAny<int>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var email = "rene@seats4me.com";
            var tickets = new TicketsController(ticketsService.Object, timeSlotsService.Object, showsService.Object)
                          {
                              ControllerContext =
                              {
                                  HttpContext = new DefaultHttpContext
                                                {
                                                    User =
                                                        new TestPrincipal(new Claim(ClaimTypes.Email,
                                                                                    email))
                                                }
                              }
                          };

            //Act
            var result = await tickets.DeleteAsync(1);

            //Assert
            Assert.IsAssignableFrom<NoContentResult>(result);
            ticketsService.Verify(s => s.DeleteAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task GetTicketAsyncFails()
        {
            //Arrange
            var email = "rene@seats4me.com";

            var ticketModel = new TicketOutputModel
                              {
                                  Name = "Hamlet",
                                  Price = 1
                              };

            var ticketsService = new Mock<ITicketsService>();
            var timeSlotsService = new Mock<ITimeSlotsService>();
            var showsService = new Mock<IShowsService>();
            ticketsService.Setup(s => s.GetTicketAsync(It.IsAny<int>())).ReturnsAsync(ticketModel);
            showsService.Setup(s => s.ExistsAsync(It.IsAny<int>())).ReturnsAsync(false);

            var tickets = new TicketsController(ticketsService.Object, timeSlotsService.Object, showsService.Object)
                          {
                              ControllerContext =
                              {
                                  HttpContext = new DefaultHttpContext
                                                {
                                                    User =
                                                        new TestPrincipal(new Claim(ClaimTypes.Email,
                                                                                    email))
                                                }
                              }
                          };

            //Act
            var result = await tickets.GetAsync(1, 1, 1);

            //Assert
            Assert.IsAssignableFrom<NotFoundResult>(result);
            ticketsService.Verify(s => s.GetTicketAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task GetTicketAsyncSucceeds()
        {
            //Arrange
            var ticketModel = new TicketOutputModel
                              {
                                  Name = "Hamlet",
                                  Price = 1
                              };

            var ticketsService = new Mock<ITicketsService>();
            var timeSlotsService = new Mock<ITimeSlotsService>();
            var showsService = new Mock<IShowsService>();
            ticketsService.Setup(s => s.GetTicketAsync(It.IsAny<int>())).ReturnsAsync(ticketModel);
            showsService.Setup(s => s.ExistsAsync(It.IsAny<int>())).ReturnsAsync(true);

            var tickets = new TicketsController(ticketsService.Object, timeSlotsService.Object, showsService.Object);

            //Act
            var result = await tickets.GetAsync(1, 1, 1);

            //Assert
            var actionResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var ticket = Assert.IsAssignableFrom<TicketOutputModel>(actionResult.Value);
            Assert.NotNull(ticket);
            ticketsService.Verify(s => s.GetTicketAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task GetTicketsAsyncByShowIdFails()
        {
            //Arrange
            var ticketOutputModel = new List<TicketOutputModel>
                                    {
                                        new TicketOutputModel
                                        {
                                            Name = "Hamlet",
                                            Price = 1
                                        }
                                    };

            var ticketsService = new Mock<ITicketsService>();
            var timeSlotsService = new Mock<ITimeSlotsService>();
            var showsService = new Mock<IShowsService>();
            ticketsService.Setup(s => s.GetTicketsByTimeSlotAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(ticketOutputModel);
            showsService.Setup(s => s.ExistsAsync(It.IsAny<int>())).ReturnsAsync(false);

            var tickets = new TicketsController(ticketsService.Object, timeSlotsService.Object, showsService.Object);

            //Act
            var result = await tickets.GetTicketsAsync(1, 1);

            //Assert
            Assert.IsAssignableFrom<NotFoundResult>(result);
            ticketsService.Verify(s => s.GetTicketsByTimeSlotAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task GetTicketsAsyncByShowIdSucceeds()
        {
            //Arrange
            var email = "rene@seats4me.com";
            var ticketOutputModel = new List<TicketOutputModel>
                                    {
                                        new TicketOutputModel
                                        {
                                            Name = "Hamlet",
                                            Price = 1
                                        }
                                    };

            var ticketsService = new Mock<ITicketsService>();
            var timeSlotsService = new Mock<ITimeSlotsService>();
            var showsService = new Mock<IShowsService>();
            ticketsService.Setup(s => s.GetTicketsByTimeSlotAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(ticketOutputModel);
            showsService.Setup(s => s.ExistsAsync(It.IsAny<int>())).ReturnsAsync(true);

            var tickets = new TicketsController(ticketsService.Object, timeSlotsService.Object, showsService.Object)
                          {
                              ControllerContext =
                              {
                                  HttpContext = new DefaultHttpContext
                                                {
                                                    User =
                                                        new TestPrincipal(new Claim(ClaimTypes.Email,
                                                                                    email))
                                                }
                              }
                          };

            //Act
            var result = await tickets.GetTicketsAsync(1, 1);

            //Assert
            var actionResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            Assert.IsAssignableFrom<List<TicketOutputModel>>(actionResult.Value);
            ticketsService.Verify(s => s.GetTicketsByTimeSlotAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task GetTicketsByUserAsyncListSucceeds()
        {
            //Arrange
            var ticketModels = new List<TicketOutputModel>
                               {
                                   new TicketOutputModel
                                   {
                                       Name = "Hamlet",
                                       Price = 1
                                   },

                                   new TicketOutputModel
                                   {
                                       Name = "Snorro",
                                       Price = 2
                                   }
                               };

            var email = "rene@seats4me.com";

            var ticketsService = new Mock<ITicketsService>();
            var timeSlotsService = new Mock<ITimeSlotsService>();
            var showsService = new Mock<IShowsService>();
            ticketsService.Setup(s => s.GetTicketsByUserAsync(It.IsAny<string>())).ReturnsAsync(ticketModels);

            var tickets = new TicketsController(ticketsService.Object, timeSlotsService.Object, showsService.Object)
                          {
                              ControllerContext =
                              {
                                  HttpContext = new DefaultHttpContext
                                                {
                                                    User =
                                                        new TestPrincipal(new Claim(ClaimTypes.Email,
                                                                                    email))
                                                }
                              }
                          };

            //Act
            var result = await tickets.GetAsync();

            //Assert
            var actionResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var listTickets = Assert.IsAssignableFrom<IEnumerable<TicketOutputModel>>(actionResult.Value);
            Assert.Equal(2, listTickets.Count());
            ticketsService.Verify(s => s.GetTicketsByUserAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task PostAsyncNoSave()
        {
            //Arrange
            var email = "rene@seats4me.com";

            var ticketInputModel = new TicketInputModel();
            var ticketOutputModel = new TicketOutputModel();

            var ticketsService = new Mock<ITicketsService>();
            var timeSlotsService = new Mock<ITimeSlotsService>();
            var showsService = new Mock<IShowsService>();
            ticketsService.Setup(s => s.AddAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<TicketInputModel>(), It.IsAny<string>())).ReturnsAsync(default(TicketOutputModel));

            showsService.Setup(s => s.ExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            timeSlotsService.Setup(s => s.ExistsAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);

            var tickets = new TicketsController(ticketsService.Object, timeSlotsService.Object, showsService.Object)
                          {
                              ControllerContext =
                              {
                                  HttpContext = new DefaultHttpContext
                                                {
                                                    User =
                                                        new TestPrincipal(new Claim(ClaimTypes.Email,
                                                                                    email))
                                                }
                              }
                          };

            //Act
            var result = await tickets.PostAsync(1, 1, ticketInputModel);

            //Assert
            var actionResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<string>(actionResult.Value);
            ticketsService.Verify(s => s.AddAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<TicketInputModel>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task PostAsyncNoShow()
        {
            //Arrange
            var email = "rene@seats4me.com";

            var ticketInputModel = new TicketInputModel();
            var ticketOutputModel = new TicketOutputModel();

            var ticketsService = new Mock<ITicketsService>();
            var timeSlotsService = new Mock<ITimeSlotsService>();
            var showsService = new Mock<IShowsService>();
            ticketsService.Setup(s => s.AddAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<TicketInputModel>(), It.IsAny<string>())).ReturnsAsync(ticketOutputModel);

            showsService.Setup(s => s.ExistsAsync(It.IsAny<int>())).ReturnsAsync(false);
            timeSlotsService.Setup(s => s.ExistsAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);

            var tickets = new TicketsController(ticketsService.Object, timeSlotsService.Object, showsService.Object)
                          {
                              ControllerContext =
                              {
                                  HttpContext = new DefaultHttpContext
                                                {
                                                    User =
                                                        new TestPrincipal(new Claim(ClaimTypes.Email,
                                                                                    email))
                                                }
                              }
                          };

            //Act
            var result = await tickets.PostAsync(1, 1, ticketInputModel);

            //Assert
            var actionResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<string>(actionResult.Value);

            ticketsService.Verify(s => s.AddAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<TicketInputModel>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task PostAsyncNoSlot()
        {
            //Arrange
            var email = "rene@seats4me.com";

            var ticketInputModel = new TicketInputModel();
            var ticketOutputModel = new TicketOutputModel();

            var ticketsService = new Mock<ITicketsService>();
            var timeSlotsService = new Mock<ITimeSlotsService>();
            var showsService = new Mock<IShowsService>();
            ticketsService.Setup(s => s.AddAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<TicketInputModel>(), It.IsAny<string>())).ReturnsAsync(ticketOutputModel);

            showsService.Setup(s => s.ExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            timeSlotsService.Setup(s => s.ExistsAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(false);

            var tickets = new TicketsController(ticketsService.Object, timeSlotsService.Object, showsService.Object)
                          {
                              ControllerContext =
                              {
                                  HttpContext = new DefaultHttpContext
                                                {
                                                    User =
                                                        new TestPrincipal(new Claim(ClaimTypes.Email,
                                                                                    email))
                                                }
                              }
                          };

            //Act
            var result = await tickets.PostAsync(1, 1, ticketInputModel);

            //Assert
            var actionResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<string>(actionResult.Value);
            ticketsService.Verify(s => s.AddAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<TicketInputModel>(), It.IsAny<string>()), Times.Never);
        }

        [InlineData(true, true, false, typeof(BadRequestObjectResult), typeof(string))]
        [InlineData(true, false, true, typeof(BadRequestObjectResult), typeof(string))]
        [InlineData(false, true, true, typeof(BadRequestObjectResult), typeof(string))]
        [Fact]
        public async Task PostAsyncOk()
        {
            //Arrange
            var email = "rene@seats4me.com";

            var ticketInputModel = new TicketInputModel();
            var ticketOutputModel = new TicketOutputModel();

            var ticketsService = new Mock<ITicketsService>();
            var timeSlotsService = new Mock<ITimeSlotsService>();
            var showsService = new Mock<IShowsService>();
            ticketsService.Setup(s => s.AddAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<TicketInputModel>(), It.IsAny<string>())).ReturnsAsync(ticketOutputModel);

            showsService.Setup(s => s.ExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            timeSlotsService.Setup(s => s.ExistsAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);

            var tickets = new TicketsController(ticketsService.Object, timeSlotsService.Object, showsService.Object)
                          {
                              ControllerContext =
                              {
                                  HttpContext = new DefaultHttpContext
                                                {
                                                    User =
                                                        new TestPrincipal(new Claim(ClaimTypes.Email,
                                                                                    email))
                                                }
                              }
                          };

            //Act
            var result = await tickets.PostAsync(1, 1, ticketInputModel);

            //Assert
            var actionResult = Assert.IsType<CreatedAtRouteResult>(result);
            Assert.IsType<TicketOutputModel>(actionResult.Value);
            ticketsService.Verify(s => s.AddAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<TicketInputModel>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task PutAsyncFails()
        {
            //Arrange
            var email = "rene@seats4me.com";

            var ticketInputModel = new TicketInputModel();

            var ticketsService = new Mock<ITicketsService>();
            var timeSlotsService = new Mock<ITimeSlotsService>();
            var showsService = new Mock<IShowsService>();
            ticketsService.Setup(s => s.UpdateAsync(It.IsAny<int>(), It.IsAny<TicketInputModel>(), It.IsAny<string>())).ReturnsAsync(default(TicketOutputModel));

            var tickets = new TicketsController(ticketsService.Object, timeSlotsService.Object, showsService.Object)
                          {
                              ControllerContext =
                              {
                                  HttpContext = new DefaultHttpContext
                                                {
                                                    User =
                                                        new TestPrincipal(new Claim(ClaimTypes.Email,
                                                                                    email))
                                                }
                              }
                          };

            //Act
            var result = await tickets.PutAsync(1, ticketInputModel);

            //Assert
            var actionResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<string>(actionResult.Value);

            ticketsService.Verify(s => s.UpdateAsync(It.IsAny<int>(), It.IsAny<TicketInputModel>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task PutAsyncSucceeds()
        {
            //Arrange
            var email = "rene@seats4me.com";

            var ticketInputModel = new TicketInputModel();
            var ticketOutputModel = new TicketOutputModel();

            var ticketsService = new Mock<ITicketsService>();
            var timeSlotsService = new Mock<ITimeSlotsService>();
            var showsService = new Mock<IShowsService>();
            ticketsService.Setup(s => s.UpdateAsync(It.IsAny<int>(), It.IsAny<TicketInputModel>(), It.IsAny<string>())).ReturnsAsync(ticketOutputModel);

            var tickets = new TicketsController(ticketsService.Object, timeSlotsService.Object, showsService.Object)
                          {
                              ControllerContext =
                              {
                                  HttpContext = new DefaultHttpContext
                                                {
                                                    User =
                                                        new TestPrincipal(new Claim(ClaimTypes.Email,
                                                                                    email))
                                                }
                              }
                          };

            //Act
            var result = await tickets.PutAsync(1, ticketInputModel);

            //Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<TicketOutputModel>(actionResult.Value);

            ticketsService.Verify(s => s.UpdateAsync(It.IsAny<int>(), It.IsAny<TicketInputModel>(), It.IsAny<string>()), Times.Once);
        }
    }
}