using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Moq;

using Seats4Me.API.Controllers;
using Seats4Me.API.Models.Input;
using Seats4Me.API.Models.Output;
using Seats4Me.API.Services;

using Xunit;

namespace Seats4Me.API.Tests.Controllers
{
    public class TimeSlotsControllerTests
    {
        [Fact]
        public async Task DeleteTimeSlotNoShowSucceeds()
        {
            //Arrange
            var timeSlotsService = new Mock<ITimeSlotsService>();
            var showsService = new Mock<IShowsService>();
            timeSlotsService.Setup(s => s.DeleteAsync(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.CompletedTask);
            showsService.Setup(s => s.ExistsAsync(It.IsAny<int>())).ReturnsAsync(false);

            var timeSlots = new TimeSlotsController(timeSlotsService.Object, showsService.Object);

            //Act
            var result = await timeSlots.DeleteAsync(1, 1);

            //Assert
            Assert.IsAssignableFrom<NoContentResult>(result);
            timeSlotsService.Verify(s => s.DeleteAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task DeleteTimeSlotWithShowSucceeds()
        {
            //Arrange
            var timeSlotsService = new Mock<ITimeSlotsService>();
            var showsService = new Mock<IShowsService>();
            timeSlotsService.Setup(s => s.DeleteAsync(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.CompletedTask);
            showsService.Setup(s => s.ExistsAsync(It.IsAny<int>())).ReturnsAsync(true);

            var timeSlots = new TimeSlotsController(timeSlotsService.Object, showsService.Object);

            //Act
            var result = await timeSlots.DeleteAsync(1, 1);

            //Assert
            Assert.IsAssignableFrom<NoContentResult>(result);
            timeSlotsService.Verify(s => s.DeleteAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task GetTimeSlotAsyncFails()
        {
            //Arrange
            var timeSlotModel = new TimeSlotOutputModel
                                {
                                    Day = DateTime.Now,
                                    Hours = 1
                                };

            var timeSlotsService = new Mock<ITimeSlotsService>();
            var showsService = new Mock<IShowsService>();
            timeSlotsService.Setup(s => s.GetAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(timeSlotModel);
            showsService.Setup(s => s.ExistsAsync(It.IsAny<int>())).ReturnsAsync(false);

            var timeSlots = new TimeSlotsController(timeSlotsService.Object, showsService.Object);

            //Act
            var result = await timeSlots.GetAsync(1, 1);

            //Assert
            Assert.IsAssignableFrom<NotFoundResult>(result);
            timeSlotsService.Verify(s => s.GetAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task GetTimeSlotAsyncSucceeds()
        {
            //Arrange
            var timeSlotModel = new TimeSlotOutputModel
                                {
                                    Day = DateTime.Now,
                                    Hours = 1
                                };

            var timeSlotsService = new Mock<ITimeSlotsService>();
            var showsService = new Mock<IShowsService>();
            timeSlotsService.Setup(s => s.GetAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(timeSlotModel);
            showsService.Setup(s => s.ExistsAsync(It.IsAny<int>())).ReturnsAsync(true);

            var timeSlots = new TimeSlotsController(timeSlotsService.Object, showsService.Object);

            //Act
            var result = await timeSlots.GetAsync(1, 1);

            //Assert
            var actionResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var timeSlot = Assert.IsAssignableFrom<TimeSlotOutputModel>(actionResult.Value);
            Assert.NotNull(timeSlot);
            timeSlotsService.Verify(s => s.GetAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task GetTimeSlotsAsyncListFails()
        {
            //Arrange
            var timeSlotModels = new List<TimeSlotOutputModel>
                                 {
                                     new TimeSlotOutputModel
                                     {
                                         Day = DateTime.Now,
                                         Hours = 1
                                     },

                                     new TimeSlotOutputModel
                                     {
                                         Day = DateTime.Now,
                                         Hours = 1.5
                                     }
                                 };

            var timeSlotsService = new Mock<ITimeSlotsService>();
            var showsService = new Mock<IShowsService>();
            timeSlotsService.Setup(s => s.GetAsync(It.IsAny<int>())).ReturnsAsync(timeSlotModels);
            showsService.Setup(s => s.ExistsAsync(It.IsAny<int>())).ReturnsAsync(false);

            var timeSlots = new TimeSlotsController(timeSlotsService.Object, showsService.Object);

            //Act
            var result = await timeSlots.GetAsync(1);

            //Assert
            Assert.IsAssignableFrom<NotFoundResult>(result);
            timeSlotsService.Verify(s => s.GetAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task GetTimeSlotsAsyncListSucceeds()
        {
            //Arrange
            var timeSlotModels = new List<TimeSlotOutputModel>
                                 {
                                     new TimeSlotOutputModel
                                     {
                                         Day = DateTime.Now,
                                         Hours = 1
                                     },

                                     new TimeSlotOutputModel
                                     {
                                         Day = DateTime.Now,
                                         Hours = 1.5
                                     }
                                 };

            var timeSlotsService = new Mock<ITimeSlotsService>();
            var showsService = new Mock<IShowsService>();
            timeSlotsService.Setup(s => s.GetAsync(It.IsAny<int>())).ReturnsAsync(timeSlotModels);
            showsService.Setup(s => s.ExistsAsync(It.IsAny<int>())).ReturnsAsync(true);

            var timeSlots = new TimeSlotsController(timeSlotsService.Object, showsService.Object);

            //Act
            var result = await timeSlots.GetAsync(1);

            //Assert
            var actionResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var listTimeSlots = Assert.IsAssignableFrom<IEnumerable<TimeSlotOutputModel>>(actionResult.Value);
            Assert.Equal(2, listTimeSlots.Count());
            timeSlotsService.Verify(s => s.GetAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task PostAsyncNoShow()
        {
            //Arrange
            var timeSlotInputModel = new TimeSlotInputModel();
            var timeSlotOutputModel = new TimeSlotOutputModel();

            var timeSlotsService = new Mock<ITimeSlotsService>();
            var showsService = new Mock<IShowsService>();
            timeSlotsService.Setup(s => s.AddAsync(It.IsAny<int>(), It.IsAny<TimeSlotInputModel>())).ReturnsAsync(timeSlotOutputModel);

            showsService.Setup(s => s.ExistsAsync(It.IsAny<int>())).ReturnsAsync(false);

            var timeSlots = new TimeSlotsController(timeSlotsService.Object, showsService.Object);

            //Act
            var result = await timeSlots.PostAsync(1, timeSlotInputModel);

            //Assert
            var actionResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<string>(actionResult.Value);
            timeSlotsService.Verify(s => s.AddAsync(It.IsAny<int>(), It.IsAny<TimeSlotInputModel>()), Times.Never);
        }

        [Fact]
        public async Task PostAsyncNoTimeSlot()
        {
            //Arrange
            var timeSlotInputModel = new TimeSlotInputModel();
            var timeSlotOutputModel = new TimeSlotOutputModel();

            var timeSlotsService = new Mock<ITimeSlotsService>();
            var showsService = new Mock<IShowsService>();
            timeSlotsService.Setup(s => s.AddAsync(It.IsAny<int>(), It.IsAny<TimeSlotInputModel>())).ReturnsAsync(default(TimeSlotOutputModel));

            showsService.Setup(s => s.ExistsAsync(It.IsAny<int>())).ReturnsAsync(true);

            var timeSlots = new TimeSlotsController(timeSlotsService.Object, showsService.Object);

            //Act
            var result = await timeSlots.PostAsync(1, timeSlotInputModel);

            //Assert
            var actionResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<string>(actionResult.Value);
            timeSlotsService.Verify(s => s.AddAsync(It.IsAny<int>(), It.IsAny<TimeSlotInputModel>()), Times.Once);
        }

        [Fact]
        public async Task PostAsyncSucceeds()
        {
            //Arrange
            var timeSlotInputModel = new TimeSlotInputModel();
            var timeSlotOutputModel = new TimeSlotOutputModel();

            var timeSlotsService = new Mock<ITimeSlotsService>();
            var showsService = new Mock<IShowsService>();
            timeSlotsService.Setup(s => s.AddAsync(It.IsAny<int>(), It.IsAny<TimeSlotInputModel>())).ReturnsAsync(timeSlotOutputModel);

            showsService.Setup(s => s.ExistsAsync(It.IsAny<int>())).ReturnsAsync(true);

            var timeSlots = new TimeSlotsController(timeSlotsService.Object, showsService.Object);

            //Act
            var result = await timeSlots.PostAsync(1, timeSlotInputModel);

            //Assert
            var actionResult = Assert.IsType<CreatedAtRouteResult>(result);
            Assert.IsType<TimeSlotOutputModel>(actionResult.Value);

            timeSlotsService.Verify(s => s.AddAsync(It.IsAny<int>(), It.IsAny<TimeSlotInputModel>()), Times.Once);
        }

        [Fact]
        public async Task PutAsyncNoShowFails()
        {
            //Arrange

            var timeSlotInputModel = new TimeSlotInputModel();
            var timeSlotOutputModel = new TimeSlotOutputModel();

            var timeSlotsService = new Mock<ITimeSlotsService>();
            var showsService = new Mock<IShowsService>();
            timeSlotsService.Setup(s => s.UpdateAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<TimeSlotInputModel>())).ReturnsAsync(timeSlotOutputModel);
            showsService.Setup(s => s.ExistsAsync(It.IsAny<int>())).ReturnsAsync(false);

            var timeSlots = new TimeSlotsController(timeSlotsService.Object, showsService.Object);

            //Act
            var result = await timeSlots.PutAsync(1, 1, timeSlotInputModel);

            //Assert
            var actionResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<string>(actionResult.Value);

            timeSlotsService.Verify(s => s.UpdateAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<TimeSlotInputModel>()), Times.Never);
        }

        [Fact]
        public async Task PutAsyncShowFails()
        {
            //Arrange

            var timeSlotInputModel = new TimeSlotInputModel();

            var timeSlotsService = new Mock<ITimeSlotsService>();
            var showsService = new Mock<IShowsService>();
            timeSlotsService.Setup(s => s.UpdateAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<TimeSlotInputModel>())).ReturnsAsync(default(TimeSlotOutputModel));
            showsService.Setup(s => s.ExistsAsync(It.IsAny<int>())).ReturnsAsync(true);

            var timeSlots = new TimeSlotsController(timeSlotsService.Object, showsService.Object);

            //Act
            var result = await timeSlots.PutAsync(1, 1, timeSlotInputModel);

            //Assert
            var actionResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<string>(actionResult.Value);

            timeSlotsService.Verify(s => s.UpdateAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<TimeSlotInputModel>()), Times.Once);
        }

        [Fact]
        public async Task PutAsyncSucceeds()
        {
            //Arrange
            var timeSlotInputModel = new TimeSlotInputModel();
            var timeSlotOutputModel = new TimeSlotOutputModel();

            var timeSlotsService = new Mock<ITimeSlotsService>();
            var showsService = new Mock<IShowsService>();
            timeSlotsService.Setup(s => s.UpdateAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<TimeSlotInputModel>())).ReturnsAsync(timeSlotOutputModel);
            showsService.Setup(s => s.ExistsAsync(It.IsAny<int>())).ReturnsAsync(true);

            var timeSlots = new TimeSlotsController(timeSlotsService.Object, showsService.Object);

            //Act
            var result = await timeSlots.PutAsync(1, 1, timeSlotInputModel);

            //Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<TimeSlotOutputModel>(actionResult.Value);

            timeSlotsService.Verify(s => s.UpdateAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<TimeSlotInputModel>()), Times.Once);
        }
    }
}