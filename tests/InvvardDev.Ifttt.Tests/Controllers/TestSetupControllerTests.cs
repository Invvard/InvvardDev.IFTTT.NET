using System.Text.Json;
using FluentAssertions;
using InvvardDev.Ifttt.Contracts;
using InvvardDev.Ifttt.Controllers;
using InvvardDev.Ifttt.Toolkit.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace InvvardDev.Ifttt.Tests.Controllers;

public class TestSetupControllerTests
{
    private static JsonSerializerOptions JsonSerializerOptions => new() { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower, DictionaryKeyPolicy = JsonNamingPolicy.SnakeCaseLower, };

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
            .ReturnsAsync(new ProcessorPayload());

        var logger = Mock.Of<ILogger<TestSetupController>>();

        var sut = new TestSetupController(testSetup, logger);

        var expectedBody = new TopLevelMessageModel<SamplesPayload>(new SamplesPayload());
        expectedBody.Data.SkimEmptyProcessors();
        var expectedBodyJson = JsonSerializer.Serialize(expectedBody, JsonSerializerOptions);

        // Act
        var result = await sut.SetupTest();

        // Assert
        result.Should().NotBeNull().And.Subject.Should().BeOfType<OkObjectResult>().Subject.Value.Should().Be(expectedBodyJson);
    }
}
