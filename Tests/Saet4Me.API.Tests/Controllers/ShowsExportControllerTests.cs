using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Moq;

using Seats4Me.API.Controllers;
using Seats4Me.API.Services;

using Xunit;

namespace Seats4Me.API.Tests.Controllers
{
    public class ShowsExportControllerTests
    {
        [Fact]
        public async Task GetAsyncSucceeds()
        {
            //Arrange
            var showsService = new Mock<IShowsService>();
            showsService.Setup(s => s.GetExportAsync()).ReturnsAsync("string");

            var showExport = new ShowsExportController(showsService.Object);

            //Act
            var result = await showExport.GetAsync();

            //Assert
            var actionResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            Assert.IsType<string>(actionResult.Value);
            showsService.Verify(s => s.GetExportAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAsyncFails()
        {
            //Arrange
            var showsService = new Mock<IShowsService>();
            showsService.Setup(s => s.GetExportAsync()).ReturnsAsync(default(string));

            var showExport = new ShowsExportController(showsService.Object);

            //Act
            var result = await showExport.GetAsync();

            //Assert
            var actionResult = Assert.IsAssignableFrom<NotFoundResult>(result);
            showsService.Verify(s => s.GetExportAsync(), Times.Once);
        }
    }
}