using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;

using Moq;

using Seats4Me.API.Models.Input;
using Seats4Me.API.Repositories;
using Seats4Me.API.Services;
using Seats4Me.Data.Model;

using Xunit;

namespace Seats4Me.API.Tests.Services
{
    public class UsersServicesTests
    {
        [Fact]
        public async Task GetAsyncNoUserSucceeds()
        {
            //Arrange
            var loginInputModel = new LoginInputModel();
            var configurationRoot = new Mock<IConfigurationRoot>();
            configurationRoot.SetupGet(x => x[It.IsAny<string>()]).Returns("keyOrIssuer");

            var usersRepository = new Mock<IUsersRepository>();
            usersRepository.Setup(s => s.GetToken(It.IsAny<Seats4MeUser>(), It.IsAny<string>(), It.IsAny<string>())).Returns("validtoken");
            usersRepository.Setup(s => s.GetAuthenticatedUserAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(default(Seats4MeUser));

            var users = new UsersService(usersRepository.Object, configurationRoot.Object);

            //Act
            var result = await users.GetTokenAsync(loginInputModel);

            //Assert
            Assert.Null(result);
            usersRepository.Verify(s => s.GetToken(It.IsAny<Seats4MeUser>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task GetAsyncSucceeds()
        {
            //Arrange
            var user = new Seats4MeUser();
            var loginInputModel = new LoginInputModel();
            var configurationRoot = new Mock<IConfigurationRoot>();
            configurationRoot.SetupGet(x => x[It.IsAny<string>()]).Returns("keyOrIssuer");

            var usersRepository = new Mock<IUsersRepository>();
            usersRepository.Setup(s => s.GetToken(It.IsAny<Seats4MeUser>(), It.IsAny<string>(), It.IsAny<string>())).Returns("validtoken");
            usersRepository.Setup(s => s.GetAuthenticatedUserAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(user);

            var users = new UsersService(usersRepository.Object, configurationRoot.Object);

            //Act
            var result = await users.GetTokenAsync(loginInputModel);

            //Assert
            Assert.NotNull(result);
            usersRepository.Verify(s => s.GetToken(It.IsAny<Seats4MeUser>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}