using InvvardDev.Ifttt.Services;
using InvvardDev.Ifttt.TestFactories.ServiceBuilder;
using Microsoft.Extensions.Logging;

namespace InvvardDev.Ifttt.Tests.Services;

public class TriggerAutoMapperServiceTest
{
    [Fact(DisplayName = "ExecuteAsync Maps Trigger Processors And Fields")]
    public async Task ExecuteAsync_MapsTriggerProcessorsAndFields()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        var logger = Mock.Of<ILogger<TriggerAutoMapperService>>();
        var triggerMapper = GivenATriggerMapper.ThatDoesNothing;

        var sut = new TriggerAutoMapperBuilder().WithLogger(logger)
                                                .WithTriggerMapper(triggerMapper)
                                                .WithCancellationTokenSource(cts)
                                                .Build();

        // Act
        await sut.StartAsync(cts.Token);

        // Assert
        Mock.Get(triggerMapper).Verify(m => m.MapTriggerProcessors(It.IsAny<CancellationToken>()), Times.Once);
        Mock.Get(triggerMapper).Verify(m => m.MapTriggerFields(It.IsAny<CancellationToken>()), Times.Once);

        logger.VerifyInformationContains("Auto-mapping trigger is starting.", Times.Once);
        logger.VerifyInformationContains("Auto-mapping trigger is stopped.", Times.Once);
        logger.VerifyInformationContains("Auto-mapping trigger is stopping.", Times.Once);
    }

    [Fact(DisplayName = "ExecuteAsync Logs Exception")]
    public async Task ExecuteAsync_LogsException()
    {
        // Arrange
        const string expectedMessage = "foobar";
        using var cts = new CancellationTokenSource();
        var logger = Mock.Of<ILogger<TriggerAutoMapperService>>();
        var triggerMapper = GivenATriggerMapper.That
                                               .Throws(expectedMessage)
                                               .Provision();

        var sut = new TriggerAutoMapperBuilder().WithLogger(logger)
                                                .WithTriggerMapper(triggerMapper)
                                                .WithCancellationTokenSource(cts)
                                                .Build();

        // Act
        await sut.StartAsync(cts.Token);

        // Assert
        Mock.Get(triggerMapper).Verify(m => m.MapTriggerProcessors(It.IsAny<CancellationToken>()), Times.Once);
        Mock.Get(triggerMapper).VerifyNoOtherCalls();

        logger.VerifyErrorContains<Exception>("Auto-mapping trigger has failed.", expectedMessage, Times.Once);
    }

    [Fact(DisplayName = "ExecuteAsync when cancelled Logs Information and stops execution")]
    public async Task ExecuteAsync_WhenCancelled_LogsInformationAndStopsExecution()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        var logger = Mock.Of<ILogger<TriggerAutoMapperService>>();
        var triggerMapper = GivenATriggerMapper.That
                                               .TakesTooLong(cts.Token)
                                               .Provision();

        var sut = new TriggerAutoMapperBuilder().WithLogger(logger)
                                                .WithTriggerMapper(triggerMapper)
                                                .WithCancellationTokenSource(cts)
                                                .Build();

        // Act
        await Task.WhenAll(sut.StartAsync(cts.Token), CancelTask(cts));

        // Assert
        Mock.Get(triggerMapper).Verify(m => m.MapTriggerProcessors(It.IsAny<CancellationToken>()), Times.Once);
        Mock.Get(triggerMapper).VerifyNoOtherCalls();

        logger.VerifyInformationContains("Auto-mapping trigger was canceled.", Times.Once);
        logger.VerifyInformationContains("Auto-mapping trigger is stopped.", Times.Once);
        logger.VerifyInformationContains("Auto-mapping trigger is stopping.", Times.Once);
        return;

        async Task CancelTask(CancellationTokenSource cancellationTokenSource)
        {
            await Task.Delay(2_000, CancellationToken.None);
            await cancellationTokenSource.CancelAsync();
        }
    }
}
