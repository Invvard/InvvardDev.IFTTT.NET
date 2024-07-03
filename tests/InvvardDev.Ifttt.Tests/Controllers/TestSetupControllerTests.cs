using System.Text.Json;
using InvvardDev.Ifttt.Controllers;
using InvvardDev.Ifttt.Toolkit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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

        // Act
        var result = await sut.SetupTest();

        // Assert
        result.Should().NotBeNull().And.Subject.Should().BeOfType<OkObjectResult>().Subject.Value.Should().BeEquivalentTo(expectedBody);
    }

    [Fact(DisplayName = "SetupTest when processor has no data field, should return 200OK with empty Samples")]
    public async Task SetupTest_WhenProcessorHasNoDataField_ShouldReturn200OKWithEmptySamples()
    {
        // Arrange
        var testSetup = Mock.Of<ITestSetup>();
        Mock.Get(testSetup)
            .Setup(x => x.PrepareSetupListing())
            .ReturnsAsync(new ProcessorPayload() { Triggers = new Processors().AddProcessor("test") });

        var logger = Mock.Of<ILogger<TestSetupController>>();

        var sut = new TestSetupController(testSetup, logger);

        var expectedBody = new TopLevelMessageModel<SamplesPayload>(new SamplesPayload());
        expectedBody.Data.SkimEmptyProcessors();

        // Act
        var result = await sut.SetupTest();

        // Assert
        result.Should().NotBeNull().And.Subject.Should().BeOfType<OkObjectResult>().Subject.Value.Should().BeEquivalentTo(expectedBody);
    }

    [Fact(DisplayName = "SetupTest when there is an exception, should log an error and return 500InternalServerError")]
    public async Task SetupTest_WhenThereIsAnException_ShouldReturn500InternalServerError()
    {
        // Arrange
        const string expectedErrorMessage = "Error while setting up test";
        const string exceptionMessage = "exception message";
        var testSetup = Mock.Of<ITestSetup>();
        Mock.Get(testSetup)
            .Setup(x => x.PrepareSetupListing())
            .ThrowsAsync(new Exception(exceptionMessage));

        var logger = Mock.Of<ILogger<TestSetupController>>();

        var sut = new TestSetupController(testSetup, logger);
        
        var expectedError = new TopLevelErrorModel(new[] { new ErrorMessage($"Error while setting up test: {exceptionMessage}") });
        var expectedErrorJson = JsonSerializer.Serialize(expectedError, JsonSerializerOptions);

        // Act
        var result = await sut.SetupTest();

        // Assert
        Mock.Get(logger).VerifyLog(l => l.LogError(It.IsAny<Exception>(), expectedErrorMessage), Times.Once());
        
        result.Should()
              .NotBeNull()
              .And.Subject.Should()
              .BeOfType<ObjectResult>()
              .Subject.StatusCode.Should()
              .Be(StatusCodes.Status500InternalServerError);

        result.As<ObjectResult>()
              .Value.Should().BeOfType<ProblemDetails>()
              .Which.Detail.Should().Be(expectedErrorJson);
    }
    
    [Fact(DisplayName = "SetupTest when processor has data field, should return 200OK with samples processor payload")]
    public async Task SetupTest_WhenProcessorHasDataFields_ShouldReturn200OKWithSamplesProcessorPayload()
    {
        // Arrange
        var testSetup = Mock.Of<ITestSetup>();
        var triggerProcessor = new Processors().AddProcessor("test").AddDataField("test", "data_field_slug", "testData");
        var expectedProcessorPayload = new ProcessorPayload() { Triggers = triggerProcessor };
        Mock.Get(testSetup)
            .Setup(x => x.PrepareSetupListing())
            .ReturnsAsync(expectedProcessorPayload);

        var logger = Mock.Of<ILogger<TestSetupController>>();

        var sut = new TestSetupController(testSetup, logger);

        var expectedBody = new TopLevelMessageModel<SamplesPayload>(new SamplesPayload(expectedProcessorPayload));

        // Act
        var result = await sut.SetupTest();

        // Assert
        result.Should().NotBeNull().And.Subject.Should().BeOfType<OkObjectResult>().Subject.Value.Should().BeEquivalentTo(expectedBody);
    }
}