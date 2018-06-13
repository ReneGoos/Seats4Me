using Seats4Me.Data.Common;
using Xunit;

namespace Seats4Me.Data.Tests
{
    public class StringExtensionsTests
    {
        [Fact]
        public void EmptyStringReturnsEmptyString()
        {
            //Arrange
            var test = default(string);
            //Act
            var result = test.InitCap();
            //Assert
            Assert.True(result == null);
        }

        [Fact]
        public void InitCappedStringReturnsSameString()
        {
            //Arrange
            var test = "Test";
            //Act
            var result = test.InitCap();
            //Assert
            Assert.Equal(test, result);
        }

        [Fact]
        public void LowerCappedStringReturnsUpperCappedString()
        {
            //Arrange
            var test = "test";
            //Act
            var result = test.InitCap();
            //Assert
            Assert.NotEqual(test, result);
            Assert.Equal("Test", result);
        }
    }
}
