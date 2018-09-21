using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using Moq;

using Seats4Me.API.Models.Input;
using Seats4Me.API.Models.Output;
using Seats4Me.API.Repositories;
using Seats4Me.API.Services;
using Seats4Me.Data.Model;

using Xunit;

namespace Seats4Me.API.Tests.Services
{
    public class TimeSlotsServicesTests : MapperTest
    {
        [Fact]
        public async Task AddAsyncSucceeds()
        {
            //Arrange
            var timeSlotInputModel = new TimeSlotInputModel();
            var timeSlot = Mapper.Map<TimeSlot>(timeSlotInputModel);

            var timeSlotsRepository = new Mock<ITimeSlotsRepository>();
            timeSlotsRepository.Setup(s => s.AddAsync(It.IsAny<TimeSlot>())).ReturnsAsync(timeSlot);

            var timeSlots = new TimeSlotsService(timeSlotsRepository.Object);

            //Act
            var result = await timeSlots.AddAsync(1, timeSlotInputModel);

            //Assert
            var timeSlotOutput = Assert.IsAssignableFrom<TimeSlotOutputModel>(result);
            timeSlotsRepository.Verify(s => s.AddAsync(It.IsAny<TimeSlot>()), Times.Once);
        }

        [Fact]
        public async Task DeleteEmptyTimeSlotSucceeds()
        {
            //Arrange
            var timeSlotsRepository = new Mock<ITimeSlotsRepository>();
            timeSlotsRepository.Setup(s => s.GetAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(default(TimeSlot));
            timeSlotsRepository.Setup(s => s.DeleteAsync(It.IsAny<TimeSlot>())).Returns(Task.CompletedTask);

            var timeSlots = new TimeSlotsService(timeSlotsRepository.Object);

            //Act
            await timeSlots.DeleteAsync(1, 1);

            //Assert
            timeSlotsRepository.Verify(s => s.GetAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            timeSlotsRepository.Verify(s => s.DeleteAsync(It.IsAny<TimeSlot>()), Times.Never);
        }

        [Fact]
        public async Task DeleteTimeSlotSucceeds()
        {
            //Arrange
            var timeSlot = new TimeSlot
                           {
                               Id = 1
                           };

            var timeSlotsRepository = new Mock<ITimeSlotsRepository>();
            timeSlotsRepository.Setup(s => s.GetAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(timeSlot);
            timeSlotsRepository.Setup(s => s.DeleteAsync(It.IsAny<TimeSlot>())).Returns(Task.CompletedTask);

            var timeSlots = new TimeSlotsService(timeSlotsRepository.Object);

            //Act
            await timeSlots.DeleteAsync(1, 1);

            //Assert
            timeSlotsRepository.Verify(s => s.GetAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            timeSlotsRepository.Verify(s => s.DeleteAsync(It.IsAny<TimeSlot>()), Times.Once);
        }

        [Fact]
        public async Task ExistsAsyncFails()
        {
            //Arrange
            var timeSlotsRepository = new Mock<ITimeSlotsRepository>();
            timeSlotsRepository.Setup(s => s.GetAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(default(TimeSlot));

            var timeSlots = new TimeSlotsService(timeSlotsRepository.Object);

            //Act
            var result = await timeSlots.ExistsAsync(1, 1);

            //Assert
            Assert.False(result);
            timeSlotsRepository.Verify(s => s.GetAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task ExistsAsyncSucceeds()
        {
            //Arrange
            var timeSlotsRepository = new Mock<ITimeSlotsRepository>();
            timeSlotsRepository.Setup(s => s.GetAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new TimeSlot());

            var timeSlots = new TimeSlotsService(timeSlotsRepository.Object);

            //Act
            var result = await timeSlots.ExistsAsync(1, 1);

            //Assert
            Assert.True(result);
            timeSlotsRepository.Verify(s => s.GetAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task GetAsyncListSucceeds()
        {
            //Arrange
            var timeSlotModels = new List<TimeSlot>
                                 {
                                     new TimeSlot
                                     {
                                         Day = DateTime.Now,
                                         Id = 1
                                     },
                                     new TimeSlot
                                     {
                                         Day = DateTime.Now,
                                         Id = 2
                                     }
                                 };

            var timeSlotsRepository = new Mock<ITimeSlotsRepository>();
            timeSlotsRepository.Setup(s => s.GetAsync(It.IsAny<int>())).ReturnsAsync(timeSlotModels);

            var timeSlots = new TimeSlotsService(timeSlotsRepository.Object);

            //Act
            var result = await timeSlots.GetAsync(1);

            //Assert
            var listTimeSlots = Assert.IsAssignableFrom<IEnumerable<TimeSlotOutputModel>>(result);
            Assert.Equal(2, listTimeSlots.Count());
            timeSlotsRepository.Verify(s => s.GetAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task GetAsyncSucceeds()
        {
            //Arrange
            var timeSlot = new TimeSlot
                           {
                               Day = DateTime.Now,
                               Id = 1,
                               Hours = 1
                           };

            var timeSlotsRepository = new Mock<ITimeSlotsRepository>();
            timeSlotsRepository.Setup(s => s.GetAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(timeSlot);

            var timeSlots = new TimeSlotsService(timeSlotsRepository.Object);

            //Act
            var result = await timeSlots.GetAsync(1, 1);

            //Assert
            var listTimeSlots = Assert.IsAssignableFrom<TimeSlotOutputModel>(result);
            Assert.Equal(1, result.Hours);
            timeSlotsRepository.Verify(s => s.GetAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task GetPriceAsyncSucceeds()
        {
            var timeSlot = new TimeSlot
                           {
                               PromoPrice = 10,
                               Show = new Show
                                      {
                                          RegularDiscountPrice = 11,
                                          RegularPrice = 10
                                      }
                           };

            //Arrange
            var timeSlotsRepository = new Mock<ITimeSlotsRepository>();
            timeSlotsRepository.Setup(s => s.GetAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(timeSlot);

            var timeSlots = new TimeSlotsService(timeSlotsRepository.Object);

            //Act
            var result = await timeSlots.GetPriceAsync(1, 1, true);

            //Assert
            Assert.IsType<decimal>(result);
            timeSlotsRepository.Verify(s => s.GetAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsyncSucceeds()
        {
            //Arrange
            var timeSlotInputModel = new TimeSlotInputModel();
            var timeSlot = new TimeSlot
                           {
                               PromoPrice = 10,
                               Show = new Show
                                      {
                                          RegularDiscountPrice = 11,
                                          RegularPrice = 10
                                      },
                               Id = 1
                           };

            var timeSlotsRepository = new Mock<ITimeSlotsRepository>();
            timeSlotsRepository.Setup(s => s.UpdateAsync(It.IsAny<TimeSlot>())).ReturnsAsync(timeSlot);

            var timeSlots = new TimeSlotsService(timeSlotsRepository.Object);

            //Act
            var result = await timeSlots.UpdateAsync(1, 1, timeSlotInputModel);

            //Assert
            var timeSlotOutputModel = Assert.IsAssignableFrom<TimeSlotOutputModel>(result);
            timeSlotsRepository.Verify(s => s.UpdateAsync(It.IsAny<TimeSlot>()), Times.Once);
        }
    }
}