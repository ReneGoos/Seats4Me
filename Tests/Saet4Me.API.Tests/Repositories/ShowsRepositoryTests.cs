using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Threading.Tasks;

using Seats4Me.API.Repositories;
using Seats4Me.Data.Model;

using Xunit;

namespace Seats4Me.API.Tests.Repositories
{
    public class ShowsRepositoryTests
    {
        [Fact]
        public async Task AddAsyncSucceeds()
        {
            var context = TheatreContextInit.InitializeContextInMemoryDb(MethodBase.GetCurrentMethod().DeclaringType.GUID.ToString());

            var showsRepository = new ShowsRepository(context);
            var show = new Show
                       {
                           Name = "Hamlet vs Hamlet",
                           TimeSlots = new List<TimeSlot>
                                       {
                                           new TimeSlot
                                           {
                                               Day = new DateTime(2018, 5, 31, 18, 0, 0),
                                               Hours = 2
                                           },

                                           new TimeSlot
                                           {
                                               Day = new DateTime(2018, 6, 2, 18, 0, 0),
                                               Hours = 2
                                           },
                                           new TimeSlot
                                           {
                                               Day = new DateTime(2018, 6, 9, 18, 0, 0),
                                               Hours = 3
                                           }
                                       }
                       };


            //Act
            var result = await showsRepository.AddAsync(show);

            //Assert
            Assert.IsType<Show>(result);
            Assert.Equal("Hamlet vs Hamlet", result.Name);
        }

        [Fact]
        public async Task AddAsyncTwiceSucceeds()
        {
            var context = TheatreContextInit.InitializeContextInMemoryDb(MethodBase.GetCurrentMethod().DeclaringType.GUID.ToString());

            var showsRepository = new ShowsRepository(context);
            var show = new Show
                       {
                           Id = 1,
                           Name = "Hamlet vs Hamlet",
                           TimeSlots = new List<TimeSlot>
                                       {
                                           new TimeSlot
                                           {
                                               Day = new DateTime(2018, 5, 31, 18, 0, 0),
                                               Hours = 2
                                           },

                                           new TimeSlot
                                           {
                                               Day = new DateTime(2018, 6, 2, 18, 0, 0),
                                               Hours = 2
                                           },
                                           new TimeSlot
                                           {
                                               Day = new DateTime(2018, 6, 9, 18, 0, 0),
                                               Hours = 3
                                           }
                                       }
                       };
            context.Shows.Add(show);
            await context.SaveChangesAsync();

            //Act
            try
            {
                var result = await showsRepository.AddAsync(show);
            }
            catch (Exception e)
            {
                //Assert
                Assert.IsType<DataException>(e);
            }
        }

        [Fact]
        public async Task DeleteAsyncSucceeds()
        {
            var context = TheatreContextInit.InitializeContextInMemoryDb(MethodBase.GetCurrentMethod().DeclaringType.GUID.ToString());

            var showsRepository = new ShowsRepository(context);
            var show = new Show
                       {
                           Name = "Hamlet vs Hamlet",
                           TimeSlots = new List<TimeSlot>
                                       {
                                           new TimeSlot
                                           {
                                               Day = new DateTime(2018, 5, 31, 18, 0, 0),
                                               Hours = 2
                                           },

                                           new TimeSlot
                                           {
                                               Day = new DateTime(2018, 6, 2, 18, 0, 0),
                                               Hours = 2
                                           },
                                           new TimeSlot
                                           {
                                               Day = new DateTime(2018, 6, 9, 18, 0, 0),
                                               Hours = 3
                                           }
                                       }
                       };
            context.Shows.Add(show);
            await context.SaveChangesAsync();

            //Act
            await showsRepository.DeleteAsync(show);

            //Assert
            Assert.Null(context.Shows.Find(show.Id));
        }

        [Fact]
        public async Task ExportCreateSucceeds()
        {
            var context = TheatreContextInit.InitializeContextInMemoryDb(MethodBase.GetCurrentMethod().DeclaringType.GUID.ToString());

            TheatreContextInit.AddTestData(context);
            await context.SaveChangesAsync();
            var showsRepository = new ShowsRepository(context);

            //Act
            var export = await showsRepository.GetExportAsync();

            //Assert
            Assert.Contains("Hamlet", export);
        }

        [Fact]
        public async Task GetAsyncByWeekSucceeds()
        {
            //Arrange
            var context = TheatreContextInit.InitializeContextInMemoryDb(MethodBase.GetCurrentMethod().DeclaringType.GUID.ToString());
            var firstDay = new DateTime(2018, 6, 2);
            var lastDay = firstDay.AddDays(7);

            context.Shows.AddRange(new List<Show>
                                   {
                                       new Show
                                       {
                                           Name = "Hamlet vs Hamlet",
                                           TimeSlots = new List<TimeSlot>
                                                       {
                                                           new TimeSlot
                                                           {
                                                               Day = new DateTime(2018, 5, 31, 18, 0, 0),
                                                               Hours = 2
                                                           },

                                                           new TimeSlot
                                                           {
                                                               Day = new DateTime(2018, 6, 2, 18, 0, 0),
                                                               Hours = 2
                                                           },
                                                           new TimeSlot
                                                           {
                                                               Day = new DateTime(2018, 6, 9, 18, 0, 0),
                                                               Hours = 3
                                                           }
                                                       }
                                       },
                                       new Show
                                       {
                                           Name = "Branden",
                                           TimeSlots = new List<TimeSlot>
                                                       {
                                                           new TimeSlot
                                                           {
                                                               Day = new DateTime(2018, 6, 3, 18, 0, 0),
                                                               Hours = 1.5
                                                           },
                                                           new TimeSlot
                                                           {
                                                               Day = new DateTime(2018, 6, 4, 18, 0, 0),
                                                               Hours = 1.5
                                                           }
                                                       }
                                       }
                                   });

            await context.SaveChangesAsync();
            var showsRepository = new ShowsRepository(context);

            //Act
            var listShows = await showsRepository.GetAsync(firstDay, lastDay, false);

            //Assert
            Assert.Equal(2, listShows.Count);
            Assert.Contains(listShows, s => s.Name.Equals("Branden"));
        }

        [Fact]
        public async Task GetAsyncByIdSucceeds()
        {
            //Arrange
            var context = TheatreContextInit.InitializeContextInMemoryDb(MethodBase.GetCurrentMethod().DeclaringType.GUID.ToString());
            var firstDay = new DateTime(2018, 6, 2);
            var lastDay = firstDay.AddDays(7);

            var showEntity = context.Shows.Add(new Show
                                       {
                                           Name = "Hamlet vs Hamlet",
                                           TimeSlots = new List<TimeSlot>
                                                       {
                                                           new TimeSlot
                                                           {
                                                               Day = new DateTime(2018, 5, 31, 18, 0, 0),
                                                               Hours = 2
                                                           },

                                                           new TimeSlot
                                                           {
                                                               Day = new DateTime(2018, 6, 2, 18, 0, 0),
                                                               Hours = 2
                                                           },
                                                           new TimeSlot
                                                           {
                                                               Day = new DateTime(2018, 6, 9, 18, 0, 0),
                                                               Hours = 3
                                                           }
                                                       }
                                       });

            await context.SaveChangesAsync();
            var showsRepository = new ShowsRepository(context);

            //Act
            var show = await showsRepository.GetAsync(showEntity.Entity.Id);

            //Assert
            Assert.Equal(3, show.TimeSlots.Count);
            Assert.Equal("Hamlet vs Hamlet", show.Name);
        }

        [Fact]
        public async Task UpdateAsyncSucceeds()
        {
            var context = TheatreContextInit.InitializeContextInMemoryDb(MethodBase.GetCurrentMethod().DeclaringType.GUID.ToString());

            var showsRepository = new ShowsRepository(context);
            var show = new Show
                       {
                           Name = "Hamlet vs Hamlet",
                           TimeSlots = new List<TimeSlot>
                                       {
                                           new TimeSlot
                                           {
                                               Day = new DateTime(2018, 5, 31, 18, 0, 0),
                                               Hours = 2
                                           },

                                           new TimeSlot
                                           {
                                               Day = new DateTime(2018, 6, 2, 18, 0, 0),
                                               Hours = 2
                                           },
                                           new TimeSlot
                                           {
                                               Day = new DateTime(2018, 6, 9, 18, 0, 0),
                                               Hours = 3
                                           }
                                       }
                       };
            context.Shows.Add(show);
            await context.SaveChangesAsync();

            var showUpdate = show;
            show.Name = "Hamlet vs Hamlet!";

            //Act
            var result = await showsRepository.UpdateAsync(showUpdate);

            //Assert
            Assert.Equal("Hamlet vs Hamlet!", result.Name);
        }
    }
}