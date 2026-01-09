using Xunit;
using Moq;
using FluentAssertions;
using ServiceManagementApi.Controllers;
using ServiceManagementApi.Services.Interfaces;

public class AuthControllerTests
{
    [Fact]
    public void AuthController_Should_Be_Created()
    {
        // Arrange
        var authServiceMock = new Mock<IAuthService>();

        // Act
        var controller = new AuthController(authServiceMock.Object);

        // Assert
        controller.Should().NotBeNull();
    }
}
