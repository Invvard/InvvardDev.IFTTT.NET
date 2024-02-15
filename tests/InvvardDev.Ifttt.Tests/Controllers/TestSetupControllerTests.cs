using System.Text.Json;
using FluentAssertions;
using InvvardDev.Ifttt.Contracts;
using InvvardDev.Ifttt.Controllers;
using InvvardDev.Ifttt.Toolkit.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
// ReSharper disable NullableWarningSuppressionIsUsed

namespace InvvardDev.Ifttt.Tests.Controllers;

public class TestSetupControllerTests
{
    [Fact(DisplayName = "TestSetupController when ITestSetup has no registered implementation service should throw")]
    public void TestSetupController_WhenITestSetupHasNoRegisteredImplementationService_ShouldThrow()
    {
        // Arrange
        var logger = new Mock<ILogger<TestSetupController>>();

        // Act
        var act = () => new TestSetupController(default!, logger.Object);

        // Assert
        act.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact(DisplayName = "SetupTest when there is no processor, should return 200OK with empty Samples")]
    public async Task SetupTest_WhenThereIsNoProcessor_ShouldReturn200OKWithEmptySamples()
    {
        // Arrange
        var testSetup = Mock.Of<ITestSetup>();
        Mock.Get(testSetup)
            .Setup(x => x.PrepareSetupListing())
            .ReturnsAsync(new Samples());

        var logger = Mock.Of<ILogger<TestSetupController>>();

        var sut = new TestSetupController(testSetup, logger);

        var expectedBody = new TopLevelMessageModel<Samples>(new Samples());
        var expectedBodyJson = JsonSerializer.Serialize(expectedBody);
        
        // Act
        var result = await sut.SetupTest();

        // Assert
        result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(expectedBodyJson);
        var actualBody = JsonSerializer.Deserialize<TopLevelMessageModel<Samples>>(((OkObjectResult)result).Value!.ToString());
        var okResult = result as OkObjectResult;
        okResult.Value.Should().BeOfType<string>();
        okResult.Value.Should().Be("{\"data\":{},\"errors\":null}");
    }
}