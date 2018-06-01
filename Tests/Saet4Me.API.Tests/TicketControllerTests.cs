using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Seats4Me.API.Data;
using Seats4Me.API.Model;
using Seats4Me.Data.Model;
using Xunit;

namespace Seats4Me.API.Tests
{
    public class TicketControllerTests
    {
        [Fact]
        public async Task OrderTicketsSucceeds()
        {
            //Arrange
            var context = TheatreContextInit.InitializeContextInMemoryDb();

            var show = await context.Shows.AddAsync(new Show()
            {
                Name = "De Woef side story",
                TimeSlots = new List<TimeSlot>()
                {
                    new TimeSlot()
                    {
                        Start = Convert.ToDateTime("01-06-2018 14:00", CultureInfo.CurrentCulture),
                        Length = 1.5
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

            var ticketsRepository = new TicketsRepository(context);
            var ticketsCtrl = new Controllers.TicketsController(ticketsRepository);
            //Act
            var result = await ticketsCtrl.GetAsync(show.Entity.TimeSlots.First().TimeSlotId);
            //Assert
            var errorResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var tickets = Assert.IsAssignableFrom<IEnumerable<Ticket>>(errorResult.Value);
            Assert.NotEmpty(tickets);
        }
    }
}
