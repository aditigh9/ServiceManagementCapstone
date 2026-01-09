using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using ServiceManagementApi.Controllers;
using ServiceManagementApi.Services.Interfaces;

public class ReportsControllerTests
{
    private readonly Mock<IReportService> _reportServiceMock;
    private readonly ReportsController _controller;

    public ReportsControllerTests()
    {
        _reportServiceMock = new Mock<IReportService>();
        _controller = new ReportsController(_reportServiceMock.Object);
    }

    [Fact]
    public void GetServiceStatusReport_Should_Return_Ok()
    {
        _reportServiceMock.Setup(x => x.GetServiceStatusReport()).Returns(new());

        var result = _controller.GetServiceStatusReport();

        result.Should().BeOfType<OkObjectResult>();
    }
}
