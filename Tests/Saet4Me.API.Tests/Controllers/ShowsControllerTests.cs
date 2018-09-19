using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Moq;

using Seats4Me.API.Controllers;
using Seats4Me.API.Models.Input;
using Seats4Me.API.Models.Output;
using Seats4Me.API.Models.Search;
using Seats4Me.API.Services;

using Xunit;

namespace Seats4Me.API.Tests.Controllers
{
    public class ShowsControllerTests
    {
        [Fact]
        public async Task DeleteShowSucceeds()
        {
            //Arrange
            var showsService = new Mock<IShowsService>();
            showsService.Setup(s => s.DeleteAsync(It.IsAny<int>())).Returns(Task.CompletedTask);

            var shows = new ShowsController(showsService.Object);

            //Act
            var result = await shows.DeleteAsync(1);

            //Assert
            Assert.IsAssignableFrom<NoContentResult>(result);
            showsService.Verify(s => s.DeleteAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task GetAsyncListSucceeds()
        {
            //Arrange
            var showModels = new List<ShowOutputModel>
                             {
                                 new ShowOutputModel
                                 {
                                     Name = "Hamlet",
                                     Id = 1
                                 },

                                 new ShowOutputModel
                                 {
                                     Name = "Snorro",
                                     Id = 2
                                 }
                             };

            var showSearch = new ShowSearchModel();
            var showsService = new Mock<IShowsService>();
            showsService.Setup(s => s.GetAsync(It.IsAny<ShowSearchModel>())).ReturnsAsync(showModels);

            var shows = new ShowsController(showsService.Object);

            //Act
            var result = await shows.GetAsync(showSearch);

            //Assert
            var actionResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var listShows = Assert.IsAssignableFrom<IEnumerable<ShowOutputModel>>(actionResult.Value);
            Assert.Equal(2, listShows.Count());
            showsService.Verify(s => s.GetAsync(It.IsAny<ShowSearchModel>()), Times.Once);
        }

        [Fact]
        public async Task GetAsyncSucceeds()
        {
            //Arrange
            var showOutputModel = new ShowOutputModel
                                 {
                                     Name = "Hamlet",
                                     Id = 1
                                 };

            var showSearch = new ShowSearchModel();
            var showsService = new Mock<IShowsService>();
            showsService.Setup(s => s.GetAsync(It.IsAny<int>())).ReturnsAsync(showOutputModel);

            var shows = new ShowsController(showsService.Object);

            //Act
            var result = await shows.GetAsync(1);

            //Assert
            var actionResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var listShows = Assert.IsAssignableFrom<ShowOutputModel>(actionResult.Value);
            showsService.Verify(s => s.GetAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task PostAsyncSucceeds()
        {
            //Arrange
            var showInputModel = new ShowInputModel();
            var showOutputModel = new ShowOutputModel();

            var showsService = new Mock<IShowsService>();
            showsService.Setup(s => s.AddAsync(It.IsAny<ShowInputModel>())).ReturnsAsync(showOutputModel);

            var shows = new ShowsController(showsService.Object);

            //Act
            var result = await shows.PostAsync(showInputModel);

            //Assert
            var actionResult = Assert.IsAssignableFrom<CreatedAtRouteResult>(result);
            var show = Assert.IsAssignableFrom<ShowOutputModel>(actionResult.Value);
            showsService.Verify(s => s.AddAsync(It.IsAny<ShowInputModel>()), Times.Once);
        }

        [Fact]
        public async Task PostAsyncAddFails()
        {
            //Arrange
            var showInputModel = new ShowInputModel();
            var showsService = new Mock<IShowsService>();
            showsService.Setup(s => s.AddAsync(It.IsAny<ShowInputModel>())).ReturnsAsync(default(ShowOutputModel));

            var shows = new ShowsController(showsService.Object);

            //Act
            var result = await shows.PostAsync(showInputModel);

            //Assert
            var actionResult = Assert.IsAssignableFrom<BadRequestObjectResult>(result);
            Assert.IsType<string>(actionResult.Value);
            showsService.Verify(s => s.AddAsync(It.IsAny<ShowInputModel>()), Times.Once);
        }

        [Fact]
        public async Task PostAsyncInvalidInput()
        {
            //Arrange
            var showInputModel = default(ShowInputModel);
            var showOutputModel = new ShowOutputModel();
            var showsService = new Mock<IShowsService>();
            showsService.Setup(s => s.AddAsync(It.IsAny<ShowInputModel>())).ReturnsAsync(showOutputModel);

            var shows = new ShowsController(showsService.Object);

            //Act
            var result = await shows.PostAsync(showInputModel);

            //Assert
            var actionResult = Assert.IsAssignableFrom<BadRequestObjectResult>(result);
            Assert.IsType<string>(actionResult.Value);
            showsService.Verify(s => s.AddAsync(It.IsAny<ShowInputModel>()), Times.Never);
        }

        [Fact]
        public async Task PutAsyncSucceeds()
        {
            //Arrange
            var showInputModel = new ShowInputModel();
            var showOutputModel = new ShowOutputModel();

            var showsService = new Mock<IShowsService>();
            showsService.Setup(s => s.UpdateAsync(It.IsAny<int>(), It.IsAny<ShowInputModel>())).ReturnsAsync(showOutputModel);

            var shows = new ShowsController(showsService.Object);

            //Act
            var result = await shows.PutAsync(1, showInputModel);

            //Assert
            var actionResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var show = Assert.IsAssignableFrom<ShowOutputModel>(actionResult.Value);
            showsService.Verify(s => s.UpdateAsync(It.IsAny<int>(), It.IsAny<ShowInputModel>()), Times.Once);
        }

        [Fact]
        public async Task PutAsyncAddFails()
        {
            //Arrange
            var showInputModel = new ShowInputModel();
            var showsService = new Mock<IShowsService>();
            showsService.Setup(s => s.UpdateAsync(It.IsAny<int>(), It.IsAny<ShowInputModel>())).ReturnsAsync(default(ShowOutputModel));

            var shows = new ShowsController(showsService.Object);

            //Act
            var result = await shows.PutAsync(1, showInputModel);

            //Assert
            var actionResult = Assert.IsAssignableFrom<BadRequestObjectResult>(result);
            Assert.IsType<string>(actionResult.Value);
            showsService.Verify(s => s.UpdateAsync(It.IsAny<int>(), It.IsAny<ShowInputModel>()), Times.Once);
        }

        [Fact]
        public async Task PutAsyncInvalidInput()
        {
            //Arrange
            var showInputModel = default(ShowInputModel);
            var showOutputModel = new ShowOutputModel();
            var showsService = new Mock<IShowsService>();
            showsService.Setup(s => s.UpdateAsync(It.IsAny<int>(), It.IsAny<ShowInputModel>())).ReturnsAsync(showOutputModel);

            var shows = new ShowsController(showsService.Object);

            //Act
            var result = await shows.PutAsync(1, showInputModel);

            //Assert
            var actionResult = Assert.IsAssignableFrom<BadRequestObjectResult>(result);
            Assert.IsType<string>(actionResult.Value);
            showsService.Verify(s => s.UpdateAsync(It.IsAny<int>(), It.IsAny<ShowInputModel>()), Times.Never);
        }
    }
}