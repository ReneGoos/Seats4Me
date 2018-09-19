using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

using Seats4Me.API.Repositories;
using Seats4Me.Data.Model;

using Xunit;

namespace Seats4Me.API.Tests.Repositories
{
    public class TimeSlotsRepositoryTests
    {
        [Fact]
        public async Task AddAsyncSucceeds()
        {
            var context = TheatreContextInit.InitializeContextInMemoryDb(MethodBase.GetCurrentMethod().DeclaringType.GUID.ToString());

            var timeSlotsRepository = new TimeSlotsRepository(context);
            var timeSlot = new TimeSlot
                               {
                                   Hours = 12,
                                   PromoPrice = 1
                               };

            //Act
            var result = await timeSlotsRepository.AddAsync(timeSlot);

            //Assert
            Assert.IsType<TimeSlot>(result);
            Assert.Equal(1, result.PromoPrice);
        }

        [Fact]
        public async Task DeleteAsyncSucceeds()
        {
            var context = TheatreContextInit.InitializeContextInMemoryDb(MethodBase.GetCurrentMethod().DeclaringType.GUID.ToString());

            var timeSlotsRepository = new TimeSlotsRepository(context);
            var timeSlot = new TimeSlot
                           {
                               Hours = 12,
                               PromoPrice = 1
                           };

            var timeSlotEntity = context.TimeSlots.Add(timeSlot);
            await context.SaveChangesAsync();

            //Act
            await timeSlotsRepository.DeleteAsync(timeSlot);

            //Assert
            Assert.Null(context.TimeSlots.Find(timeSlotEntity.Entity.Id));
        }

        [Fact]
        public async Task GetAsyncByShowIdSucceeds()
        {
            //Arrange
            var context = TheatreContextInit.InitializeContextInMemoryDb(MethodBase.GetCurrentMethod().DeclaringType.GUID.ToString());
            var timeSlots = new List<TimeSlot>()
                           {
                               new TimeSlot
                               {
                                   ShowId = 1,
                                   Hours = 12,
                                   PromoPrice = 1
                               },
                               new TimeSlot
                               {
                                   ShowId = 2,
                                   Hours = 12,
                                   PromoPrice = 1
                               }
                           };

            context.TimeSlots.AddRange(timeSlots);

            await context.SaveChangesAsync();
            var timeSlotsRepository = new TimeSlotsRepository(context);

            //Act
            var listTimeSlots = await timeSlotsRepository.GetAsync(1);

            //Assert
            Assert.Single(listTimeSlots);
        }

        [Fact]
        public async Task GetAsyncByShowIdAndIdSucceeds()
        {
            //Arrange
            var context = TheatreContextInit.InitializeContextInMemoryDb(MethodBase.GetCurrentMethod().DeclaringType.GUID.ToString());
            var timeSlots = new List<TimeSlot>()
                            {
                                new TimeSlot
                                {
                                    Id = 1,
                                    ShowId = 1,
                                    Hours = 12,
                                    PromoPrice = 1
                                },
                                new TimeSlot
                                {
                                    Id = 2,
                                    ShowId = 2,
                                    Hours = 12,
                                    PromoPrice = 1
                                }
                            };

            context.TimeSlots.AddRange(timeSlots);

            await context.SaveChangesAsync();
            var timeSlotsRepository = new TimeSlotsRepository(context);

            //Act
            var listTimeSlot = await timeSlotsRepository.GetAsync(1, 1);

            //Assert
            Assert.Equal(12, listTimeSlot.Hours);
        }

        [Fact]
        public async Task UpdateAsyncSucceeds()
        {
            var context = TheatreContextInit.InitializeContextInMemoryDb(MethodBase.GetCurrentMethod().DeclaringType.GUID.ToString());

            var timeSlotsRepository = new TimeSlotsRepository(context);
            var timeSlot = new TimeSlot
            {
                Hours = 12,
                PromoPrice = 1,
                Day = new DateTime(2018, 9, 12, 20, 0, 0)
            };

            context.TimeSlots.Add(timeSlot);
            await context.SaveChangesAsync();

            var timeSlotUpdate = timeSlot;
            timeSlot.Day = new DateTime(2018, 9, 13, 20, 0, 0);

            //Act
            var result = await timeSlotsRepository.UpdateAsync(timeSlotUpdate);

            //Assert
            Assert.Equal(new DateTime(2018, 9, 13, 20, 0, 0), result.Day);
        }
    }
}