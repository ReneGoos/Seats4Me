using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using AutoMapper;

using Seats4Me.API.Models;
using Seats4Me.API.Repositories;
using Seats4Me.Data.Model;

using Xunit;

namespace Seats4Me.API.Tests.Repositories
{
    public class TimeSlotSeatsRepositoryTests
    {
        private void Setup()
        {
            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.AddProfile(new Seats4MeProfile()));
        }

        [Fact]
        public async Task AddAsyncSucceeds()
        {
            Setup();
            var context = TheatreContextInit.InitializeContextInMemoryDb(MethodBase.GetCurrentMethod().DeclaringType.GUID.ToString());

            var timeSlotSeatsRepository = new TimeSlotSeatsRepository(context);
            var timeSlotSeat = new TimeSlotSeat
                               {
                                   Price = 12,
                                   Paid = true
                               };

            //Act
            var result = await timeSlotSeatsRepository.AddAsync(timeSlotSeat);

            //Assert
            Assert.IsType<TimeSlotSeat>(result);
            Assert.Equal(12, result.Price);
        }

        [Fact]
        public async Task DeleteAsyncSucceeds()
        {
            Setup();
            var context = TheatreContextInit.InitializeContextInMemoryDb(MethodBase.GetCurrentMethod().DeclaringType.GUID.ToString());

            var timeSlotSeatsRepository = new TimeSlotSeatsRepository(context);
            var timeSlotSeat = new TimeSlotSeat
                               {
                                   Price = 12,
                                   Paid = true
                               };

            var timeSlotSeatEntity = context.TimeSlotSeats.Add(timeSlotSeat);
            await context.SaveChangesAsync();

            //Act
            await timeSlotSeatsRepository.DeleteAsync(timeSlotSeat);

            //Assert
            Assert.Null(context.TimeSlotSeats.Find(timeSlotSeatEntity.Entity.Id));
        }

        [Fact]
        public async Task GetAsyncByIdSucceeds()
        {
            //Arrange
            Setup();
            var context = TheatreContextInit.InitializeContextInMemoryDb(MethodBase.GetCurrentMethod().DeclaringType.GUID.ToString());
            var timeSlotSeatEntity = context.TimeSlotSeats.Add(new TimeSlotSeat
                                                               {
                                                                   Price = 12,
                                                                   Paid = true
                                                               });

            await context.SaveChangesAsync();
            var timeSlotSeatsRepository = new TimeSlotSeatsRepository(context);

            //Act
            var timeSlotSeat = await timeSlotSeatsRepository.GetAsync(timeSlotSeatEntity.Entity.Id);

            //Assert
            Assert.True(timeSlotSeat.Paid);
        }

        [Fact]
        public async Task GetAsyncBySeatSucceeds()
        {
            //Arrange
            Setup();
            var context = TheatreContextInit.InitializeContextInMemoryDb(MethodBase.GetCurrentMethod().DeclaringType.GUID.ToString());
            context.TimeSlotSeats.Add(new TimeSlotSeat
                                      {
                                          Seat = new Seat
                                                 {
                                                     Row = 1,
                                                     Chair = 1
                                                 },
                                          TimeSlotId = 1,
                                          Price = 12,
                                          Paid = true
                                      });

            await context.SaveChangesAsync();
            var timeSlotSeatsRepository = new TimeSlotSeatsRepository(context);

            //Act
            var listTimeSlotSeat = await timeSlotSeatsRepository.GetAsync(1, 1, 1);

            //Assert
            Assert.True(listTimeSlotSeat.Paid);
        }

        [Fact]
        public async Task GetTicketAsyncByIdSucceeds()
        {
            //Arrange
            Setup();
            var context = TheatreContextInit.InitializeContextInMemoryDb(MethodBase.GetCurrentMethod().DeclaringType.GUID.ToString());
            context.TimeSlotSeats.Add(new TimeSlotSeat
                                      {
                                          Seat = new Seat
                                                 {
                                                     Row = 1,
                                                     Chair = 1
                                                 },
                                          TimeSlot = new TimeSlot
                                                     {
                                                         Id = 1,
                                                         Show = new Show()
                                                                {
                                                                    Id = 1
                                                                }
                                          },
                                          Id = 1,
                                          Price = 12,
                                          Paid = true
                                      });

            await context.SaveChangesAsync();
            var timeSlotSeatsRepository = new TimeSlotSeatsRepository(context);

            //Act
            var timeSlotSeat = await timeSlotSeatsRepository.GetTicketAsync(1);

            //Assert
            Assert.NotNull(timeSlotSeat);
            Assert.True(timeSlotSeat.TimeSlotSeat.Paid);
        }

        [Fact]
        public async Task GetAsyncBySlotSucceeds()
        {
            //Arrange
            Setup();
            var context = TheatreContextInit.InitializeContextInMemoryDb(MethodBase.GetCurrentMethod().DeclaringType.GUID.ToString());
            context.TimeSlotSeats.Add(new TimeSlotSeat
                                      {
                                          Seat = new Seat
                                                 {
                                                     Row = 1,
                                                     Chair = 1
                                                 },
                                          TimeSlot = new TimeSlot
                                                     {
                                                         Id = 1,
                                                         Show = new Show()
                                                                {
                                                                    Id = 1
                                                                }
                                          },

                                          Price = 12,
                                          Paid = true
                                      });

            await context.SaveChangesAsync();
            var timeSlotSeatsRepository = new TimeSlotSeatsRepository(context);

            //Act
            var listTimeSlotSeats = await timeSlotSeatsRepository.GetTicketsByTimeSlotAsync(1, 1);

            //Assert
            Assert.Single(listTimeSlotSeats);
            Assert.True(listTimeSlotSeats.First().TimeSlotSeat.Paid);
        }

        [Fact]
        public async Task GetAsyncByUserSucceeds()
        {
            //Arrange
            Setup();
            var context = TheatreContextInit.InitializeContextInMemoryDb(MethodBase.GetCurrentMethod().DeclaringType.GUID.ToString());
            context.TimeSlotSeats.Add(new TimeSlotSeat
                                      {
                                          Seat = new Seat
                                                 {
                                                     Row = 1,
                                                     Chair = 1
                                                 },
                                          TimeSlot = new TimeSlot
                                                     {
                                                         Id = 1,
                                                         Show = new Show()
                                                                {
                                                                    Id = 1
                                                                }
                                                     },
                                          Seats4MeUserId = 1,
                                          Price = 12,
                                          Paid = true
                                      });

            await context.SaveChangesAsync();
            var timeSlotSeatsRepository = new TimeSlotSeatsRepository(context);

            //Act
            var listTimeSlotSeats = await timeSlotSeatsRepository.GetTicketsByUserAsync(1);

            //Assert
            Assert.Single(listTimeSlotSeats);
            Assert.True(listTimeSlotSeats.First().TimeSlotSeat.Paid);
        }

        [Fact]
        public async Task UpdateAsyncSucceeds()
        {
            Setup();
            var context = TheatreContextInit.InitializeContextInMemoryDb(MethodBase.GetCurrentMethod().DeclaringType.GUID.ToString());

            var timeSlotSeatsRepository = new TimeSlotSeatsRepository(context);
            var timeSlotSeat = new TimeSlotSeat
                               {
                                   Price = 12,
                                   Paid = true,
                                   Reserved = false
                               };

            context.TimeSlotSeats.Add(timeSlotSeat);
            await context.SaveChangesAsync();

            var timeSlotSeatUpdate = timeSlotSeat;
            timeSlotSeat.Reserved = true;

            //Act
            var result = await timeSlotSeatsRepository.UpdateAsync(timeSlotSeatUpdate);

            //Assert
            Assert.True(result.Reserved);
        }
    }
}