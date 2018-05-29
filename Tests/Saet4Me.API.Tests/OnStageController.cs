using System.Linq;
using System.Threading.Tasks;
using Seats4Me.API.Controllers;
using Xunit;

namespace Seats4Me.API.Tests
{
    public class OnStageController
    {
        [Fact]
        public async Task ListShowsWhenGetTheatre()
        {
            //Arrange
            var shows = new Controllers.OnStageController();
            //Act
            var listShows = await shows.GetAsync();
            //Assert
            Assert.True(listShows.Any());
        }
    }
}
