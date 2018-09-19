using System;

using Seats4Me.API.Common;

using Xunit;

namespace Seats4Me.API.Tests.Common
{
    public class DateTimeExtensionsTests
    {
        [Fact]
        public void FirstDayOfYearWeek52()
        {
            //Arrange
            var firstDay = new DateTime(2022, 1, 1);

            //Act
            var week = firstDay.Week();

            //Assert
            Assert.Equal(52, week);
        }

        [Fact]
        public void FirstDayOfYearWeek53()
        {
            //Arrange
            var firstDay = new DateTime(2021, 1, 1);

            //Act
            var week = firstDay.Week();

            //Assert
            Assert.Equal(53, week);
        }

        [Fact]
        public void FirstDayOfYearWeekOne()
        {
            //Arrange
            var firstDay = new DateTime(2019, 1, 1);

            //Act
            var week = firstDay.Week();

            //Assert
            Assert.Equal(1, week);
        }

        [Fact]
        public void LastDayInWeek()
        {
            //Arrange
            var day = new DateTime(2018, 9, 23);
            var week = day.Week();

            //Act
            var firstDay = DateTimeExtensions.FirstDayOfWeek(week, day.Year);

            //Assert
            Assert.True(firstDay <= day);
            Assert.True(firstDay.AddDays(7) > day);
        }

        [Fact]
        public void WeeksAfterFirstDayOfYearWeek53()
        {
            //Arrange
            var day = new DateTime(2021, 10, 1);
            var week = day.Week();

            //Act
            var firstDay = DateTimeExtensions.FirstDayOfWeek(week, day.Year);

            //Assert
            Assert.True(firstDay <= day);
        }

        [Fact]
        public void WeeksAfterFirstDayOfYearWeekOne()
        {
            //Arrange
            var day = new DateTime(2019, 10, 1);
            var week = day.Week();

            //Act
            var firstDay = DateTimeExtensions.FirstDayOfWeek(week, day.Year);

            //Assert
            Assert.True(firstDay <= day);
        }
    }
}