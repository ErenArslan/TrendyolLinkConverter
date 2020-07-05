using FluentAssertions;
using TrendyolLinkConverter.Core.Exceptions;
using TrendyolLinkConverter.Core.Models;
using Xunit;

namespace TrendyolLinkConverter.UnitTests.Domain.Models
{
    
    public class RequestHistoryTests
    {
        [Fact]
        public void Initialize_WithValidData_ShouldCreateNewRequestHistory()
        {
            // Arrange
            const string request = "fake request";
            const string response = "fake response";


            // Act
            var requestHistory = new RequestHistory(request,response);

            // Assert
            requestHistory.Request.Should().Be(request);
            requestHistory.Request.Should().Be(request);

            requestHistory.Should().NotBeNull();
        }

        [Theory]
        [InlineData("","")]
        [InlineData(null,null)]
        public void Initialize_WithInvalidText_ShouldThrowDomainException(string request,string response)
        {
            // Arrange
            const string expectedExceptionMessage = "Property cannot be null or empty";


            LinkConverterDomainException actualException = Assert.Throws<LinkConverterDomainException>(() => new RequestHistory(request,response));

            // Assert
            actualException.Message.Should().Contain(expectedExceptionMessage);
        }
    }
}
