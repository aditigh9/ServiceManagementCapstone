using Xunit;
using Moq;
using FluentAssertions;
using ServiceManagementApi.Controllers;
using ServiceManagementApi.Services.Interfaces;

public class ServiceRequestsControllerTests
{
    [Fact]
    public void ServiceRequestsController_Should_Be_Created()
    {
        // Arrange
        var serviceMock = new Mock<IServiceRequestService>();

        // Act
        var controller = new ServiceRequestsController(serviceMock.Object);

        // Assert
        controller.Should().NotBeNull();
    }
}
