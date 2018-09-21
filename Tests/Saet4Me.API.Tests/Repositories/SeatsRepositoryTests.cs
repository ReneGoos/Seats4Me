using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

using Seats4Me.API.Repositories;
using Seats4Me.Data.Model;

using Xunit;

namespace Seats4Me.API.Tests.Repositories
{
    public class SeatsRepositoryTests
    {
        [Fact]
        public async Task GetSeatByIdSucceeds()
        {
            //Arrange
            var context = TheatreContextInit.InitializeContextInMemoryDb(MethodBase.GetCurrentMethod().DeclaringType.GUID.ToString());
            var SeatEntity = await context.Seats.AddAsync(new Seat
                                                          {
                                                              Row = 1,
                                                              Chair = 1
                                                          });

            await context.SaveChangesAsync();
            var repository = new SeatsRepository(context);

            //Act
            var seat = await repository.GetAsync(SeatEntity.Entity.Id);

            //Assert
            Assert.NotNull(seat);
            Assert.Equal(1, seat.Row);
        }

        [Fact]
        public async Task GetSeatsSucceeds()
        {
            //Arrange
            var context = TheatreContextInit.InitializeContextInMemoryDb(MethodBase.GetCurrentMethod().DeclaringType.GUID.ToString());
            context.Seats.AddRange(new List<Seat>
                                   {
                                       new Seat
                                       {
                                           Row = 1,
                                           Chair = 1
                                       },
                                       new Seat
                                       {
                                           Row = 1,
                                           Chair = 2
                                       }
                                   });

            await context.SaveChangesAsync();
            var repository = new SeatsRepository(context);

            //Act
            var seats = await repository.GetAsync();

            //Assert
            Assert.NotEmpty(seats);
            Assert.Equal(2, seats.Count);
        }

        [Fact]
        public async Task GetSeatSucceeds()
        {
            //Arrange
            var context = TheatreContextInit.InitializeContextInMemoryDb(MethodBase.GetCurrentMethod().DeclaringType.GUID.ToString());
            await context.Seats.AddAsync(new Seat
                                         {
                                             Row = 1,
                                             Chair = 1
                                         });

            await context.SaveChangesAsync();
            var repository = new SeatsRepository(context);

            //Act
            var seat = await repository.GetAsync(1, 1);

            //Assert
            Assert.NotNull(seat);
            Assert.Equal(1, seat.Row);
        }
    }
}