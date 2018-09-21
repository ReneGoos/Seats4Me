using System.Reflection;
using System.Threading.Tasks;

using Seats4Me.API.Repositories;
using Seats4Me.Data.Model;

using Xunit;

namespace Seats4Me.API.Tests.Repositories
{
    public class UsersRepositoryTests
    {
        [Fact]
        public async Task GetTokenSucceeds()
        {
            //Arrange
            var signingIssuer = "http://www.seats4me.com";
            var signingKey = "MMMHHHAHD77979p3ejeljwiur97";
            var context = TheatreContextInit.InitializeContextInMemoryDb(MethodBase.GetCurrentMethod().DeclaringType.GUID.ToString());
            var userEntity = await context.Seats4MeUsers.AddAsync(new Seats4MeUser
                                                                  {
                                                                      Name = "Test",
                                                                      Email = "test@TEST",
                                                                      Password = "password",
                                                                      Roles = "admin"
                                                                  });

            await context.SaveChangesAsync();
            var repository = new UsersRepository(context);

            //Act
            var token = repository.GetToken(userEntity.Entity, signingKey, signingIssuer);

            //Assert
            Assert.NotNull(token);
        }

        [Fact]
        public async Task GetUser()
        {
            //Arrange
            var context = TheatreContextInit.InitializeContextInMemoryDb(MethodBase.GetCurrentMethod().DeclaringType.GUID.ToString());
            await context.Seats4MeUsers.AddAsync(new Seats4MeUser
                                                 {
                                                     Email = "test@test",
                                                     Password = "password",
                                                     Roles = "admin"
                                                 });

            await context.SaveChangesAsync();
            var repository = new UsersRepository(context);

            //Act
            var seats4MeUser = await repository.GetAuthenticatedUserAsync("test@test", "password");

            //Assert
            Assert.NotNull(seats4MeUser);
            Assert.Equal("admin", seats4MeUser.Roles);
        }

        [Fact]
        public async Task GetUserFailsOnInvalidPassword()
        {
            //Arrange
            var context = TheatreContextInit.InitializeContextInMemoryDb(MethodBase.GetCurrentMethod().DeclaringType.GUID.ToString());
            await context.Seats4MeUsers.AddAsync(new Seats4MeUser
                                                 {
                                                     Email = "test@test",
                                                     Password = "password",
                                                     Roles = "admin"
                                                 });

            await context.SaveChangesAsync();
            var repository = new UsersRepository(context);

            //Act
            var seats4MeUser = await repository.GetAuthenticatedUserAsync("test@test", "P!ssword?");

            //Assert
            Assert.Null(seats4MeUser);
        }

        [Fact]
        public async Task GetUserOnDifferentCaseEmailSucceeds()
        {
            //Arrange
            var context = TheatreContextInit.InitializeContextInMemoryDb(MethodBase.GetCurrentMethod().DeclaringType.GUID.ToString());
            await context.Seats4MeUsers.AddAsync(new Seats4MeUser
                                                 {
                                                     Email = "test@TEST",
                                                     Password = "password",
                                                     Roles = "admin"
                                                 });

            await context.SaveChangesAsync();
            var repository = new UsersRepository(context);

            //Act
            var seats4MeUser = await repository.GetAuthenticatedUserAsync("TEST@test", "password");

            //Assert
            Assert.NotNull(seats4MeUser);
            Assert.Equal("admin", seats4MeUser.Roles);
        }

        [Fact]
        public async Task GetUserSucceeds()
        {
            //Arrange
            var context = TheatreContextInit.InitializeContextInMemoryDb(MethodBase.GetCurrentMethod().DeclaringType.GUID.ToString());
            await context.Seats4MeUsers.AddAsync(new Seats4MeUser
                                                 {
                                                     Email = "test@TEST",
                                                     Password = "password",
                                                     Roles = "admin"
                                                 });

            await context.SaveChangesAsync();
            var repository = new UsersRepository(context);

            //Act
            var seats4MeUser = await repository.GetUserAsync("test@TEST");

            //Assert
            Assert.NotNull(seats4MeUser);
            Assert.Equal("admin", seats4MeUser.Roles);
        }
    }
}