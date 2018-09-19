using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Moq;

using Seats4Me.API.Controllers;
using Seats4Me.API.Models.Input;
using Seats4Me.API.Services;

using Xunit;

namespace Seats4Me.API.Tests.Controllers
{
    public class LoginControllerTests
    {
        [Fact]
        public async Task ValidUserLogin()
        {
            //Arrange
            var loginModel = new LoginInputModel()
                             {
                                 Email = "test@tests.nl",
                                 Password = "*****"
                             };

            var service = new Mock<IUsersService>();
            service.Setup(s => s.GetTokenAsync(It.IsAny<LoginInputModel>())).ReturnsAsync("validtoken");
            var loginController = new LoginController(service.Object);

            //Act
            var result = await loginController.PostAsync(loginModel);

            //Assert
            var payload = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("validtoken", payload.Value);
            service.Verify(s => s.GetTokenAsync(It.IsAny<LoginInputModel>()), Times.Once);
        }

        [Fact]
        public async Task InvalidUserLogin()
        {
            //Arrange
            var loginModel = new LoginInputModel()
                             {
                                 Email = "test@tests.nl",
                                 Password = "*****"
                             };

            var service = new Mock<IUsersService>();
            service.Setup(s => s.GetTokenAsync(It.IsAny<LoginInputModel>())).ReturnsAsync(default(string));
            var loginController = new LoginController(service.Object);

            //Act
            var result = await loginController.PostAsync(loginModel);

            //Assert
            Assert.IsType<BadRequestResult>(result);
            service.Verify(s => s.GetTokenAsync(It.IsAny<LoginInputModel>()), Times.Once);
        }
    }
}