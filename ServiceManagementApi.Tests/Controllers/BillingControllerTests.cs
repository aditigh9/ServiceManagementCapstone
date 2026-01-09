using Xunit;
using Moq;
using FluentAssertions;
using ServiceManagementApi.Controllers;
using ServiceManagementApi.Services.Interfaces;

public class BillingControllerTests
{
    [Fact]
    public void BillingController_Should_Be_Created()
    {
        // Arrange
        var billingServiceMock = new Mock<IBillingService>();

        // Act
        var controller = new BillingController(billingServiceMock.Object);

        // Assert
        controller.Should().NotBeNull();
    }
}
