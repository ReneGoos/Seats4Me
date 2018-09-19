
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using Microsoft.AspNetCore.Mvc;

using Moq;

using Seats4Me.API.Controllers;
using Seats4Me.API.Models;
using Seats4Me.API.Models.Input;
using Seats4Me.API.Models.Output;
using Seats4Me.API.Models.Search;
using Seats4Me.API.Repositories;
using Seats4Me.API.Services;
using Seats4Me.Data.Model;

using Xunit;

namespace Seats4Me.API.Tests.Services
{
    public class ShowsServicesTests
    {
        private void Setup()
        {
            Mapper.Reset();
            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.AddProfile(new Seats4MeProfile()));
        }

        [Fact]
        public async Task AddAsyncSucceeds()
        {
            Setup();
            //Arrange
            var showInputModel = new ShowInputModel()
                                        {
                                            Name = "Hamlet vs Hamlet",
                                            Title = "Hamlet vs. Hamlet",
                                            Description =
                                                "Guy Cassiers, Tom Lanoye en het stuk der stukken: Shakespeares Hamlet. Cassiers regisseerde deze bewerking met een grote cast, samengesteld uit de ensembles van Toneelhuis en Toneelgroep Amsterdam.",
                                            RegularPrice = 27.50M,
                                            RegularDiscountPrice = 15.0M
                                        };
            var show = Mapper.Map<Show>(showInputModel);

            var showsRepository = new Mock<IShowsRepository>();
            showsRepository.Setup(s => s.AddAsync(It.IsAny<Show>())).ReturnsAsync(show);

            var shows = new ShowsService(showsRepository.Object);

            //Act
            var result = await shows.AddAsync(showInputModel);

            //Assert
            var showOutput = Assert.IsAssignableFrom<ShowOutputModel>(result);
            showsRepository.Verify(s => s.AddAsync(It.IsAny<Show>()), Times.Once);
        }

        [Fact]
        public async Task DeleteShowSucceeds()
        {
            //Arrange
            var show = new Show()
                       {
                           Id = 1
                       };
            var showsRepository = new Mock<IShowsRepository>();
            showsRepository.Setup(s => s.GetAsync(It.IsAny<int>())).ReturnsAsync(show);
            showsRepository.Setup(s => s.DeleteAsync(It.IsAny<Show>())).Returns(Task.CompletedTask);

            var shows = new ShowsService(showsRepository.Object);

            //Act
            await shows.DeleteAsync(1);

            //Assert
            showsRepository.Verify(s => s.GetAsync(It.IsAny<int>()), Times.Once);
            showsRepository.Verify(s => s.DeleteAsync(It.IsAny<Show>()), Times.Once);
        }

        [Fact]
        public async Task DeleteEmptyShowSucceeds()
        {
            //Arrange
            var showsRepository = new Mock<IShowsRepository>();
            showsRepository.Setup(s => s.GetAsync(It.IsAny<int>())).ReturnsAsync(default(Show));
            showsRepository.Setup(s => s.DeleteAsync(It.IsAny<Show>())).Returns(Task.CompletedTask);

            var shows = new ShowsService(showsRepository.Object);

            //Act
            await shows.DeleteAsync(1);

            //Assert
            showsRepository.Verify(s => s.GetAsync(It.IsAny<int>()), Times.Once);
            showsRepository.Verify(s => s.DeleteAsync(It.IsAny<Show>()), Times.Never);
        }

        [Fact]
        public async Task ExistsAsyncSucceeds()
        {
            //Arrange
            var showsRepository = new Mock<IShowsRepository>();
            showsRepository.Setup(s => s.GetAsync(It.IsAny<int>())).ReturnsAsync(new Show());

            var shows = new ShowsService(showsRepository.Object);

            //Act
            var result = await shows.ExistsAsync(1);

            //Assert
            Assert.True(result);
            showsRepository.Verify(s => s.GetAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task ExistsAsyncFails()
        {
            //Arrange
            var showsRepository = new Mock<IShowsRepository>();
            showsRepository.Setup(s => s.GetAsync(It.IsAny<int>())).ReturnsAsync(default(Show));

            var shows = new ShowsService(showsRepository.Object);

            //Act
            var result = await shows.ExistsAsync(1);

            //Assert
            Assert.False(result);
            showsRepository.Verify(s => s.GetAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task GetAsyncListSucceeds()
        {
            //Arrange
            Setup();
            var showModels = new List<Show>
                             {
                                 new Show
                                 {
                                     Name = "Hamlet",
                                     Id = 1
                                 },
                                 new Show
                                 {
                                     Name = "Snorro",
                                     Id = 2
                                 }
                             };

            var showSearch = new ShowSearchModel();
            var showsRepository = new Mock<IShowsRepository>();
            showsRepository.Setup(s => s.GetAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<bool>())).ReturnsAsync(showModels);

            var shows = new ShowsService(showsRepository.Object);

            //Act
            var result = await shows.GetAsync(showSearch);

            //Assert
            var listShows = Assert.IsAssignableFrom<IEnumerable<ShowOutputModel>>(result);
            Assert.Equal(2, listShows.Count());
            showsRepository.Verify(s => s.GetAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<bool>()), Times.Once);
        }

        [Fact]
        public async Task GetAsyncSucceeds()
        {
            //Arrange
            Setup();
            var show = new Show
            {
                Name = "Hamlet",
                Id = 1
            };

            var showsRepository = new Mock<IShowsRepository>();
            showsRepository.Setup(s => s.GetAsync(It.IsAny<int>())).ReturnsAsync(show);

            var shows = new ShowsService(showsRepository.Object);

            //Act
            var result = await shows.GetAsync(1);

            //Assert
            var listShows = Assert.IsAssignableFrom<ShowOutputModel>(result);
            Assert.Equal("Hamlet", result.Name);
            showsRepository.Verify(s => s.GetAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsyncSucceeds()
        {
            //Arrange
            Setup();
            var showInputModel = new ShowInputModel();
            var show = new Show
                       {
                           Name = "Hamlet",
                           Id = 1
                       };

            var showsRepository = new Mock<IShowsRepository>();
            showsRepository.Setup(s => s.UpdateAsync(It.IsAny<Show>())).ReturnsAsync(show);

            var shows = new ShowsService(showsRepository.Object);

            //Act
            var result = await shows.UpdateAsync(1, showInputModel);

            //Assert
            var showOutputModel = Assert.IsAssignableFrom<ShowOutputModel>(result);
            showsRepository.Verify(s => s.UpdateAsync(It.IsAny<Show>()), Times.Once);
        }

        [Fact]
        public async Task GetExportAsyncSucceeds()
        {
            //Arrange
            var showsRepository = new Mock<IShowsRepository>();
            showsRepository.Setup(s => s.GetExportAsync()).ReturnsAsync("export");

            var shows = new ShowsService(showsRepository.Object);

            //Act
            var result = await shows.GetExportAsync();

            //Assert
            Assert.IsType<string>(result);
            showsRepository.Verify(s => s.GetExportAsync(), Times.Once);
        }
    }
}