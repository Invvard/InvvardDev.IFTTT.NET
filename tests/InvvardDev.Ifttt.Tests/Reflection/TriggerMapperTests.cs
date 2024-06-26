using InvvardDev.Ifttt.Contracts;
using InvvardDev.Ifttt.Models.Core;
using InvvardDev.Ifttt.Models.Trigger;
using InvvardDev.Ifttt.Reflection;
using InvvardDev.Ifttt.TestFactories.Triggers;
using InvvardDev.Ifttt.Toolkit.Attributes;
using Microsoft.Extensions.Logging;

namespace InvvardDev.Ifttt.Tests.Reflection;

public class TriggerMapperTests
{
    [Fact(DisplayName = "MapTriggerProcessors when a new trigger processor is found should register trigger")]
    public async Task MapTriggerProcessors_WhenNewTriggerProcessorIsFound_ShouldRegisterTrigger()
    {
        // Arrange
        const string triggerSlug = "trigger_slug";
        var triggerType = TriggerClassFactory.MatchingClass(triggerSlug: triggerSlug);
        var notTriggerType = TriggerClassFactory.MissingTriggerAttribute();
        var triggerAttributeLookup = Mock.Of<IAttributeLookup>(x => x.GetAnnotatedTypes() == new[] { triggerType, notTriggerType });

        var triggerFieldsAttributeLookup = Mock.Of<IAttributeLookup>();
        var triggerService = Mock.Of<IProcessorService>();
        var logger = Mock.Of<ILogger<TriggerMapper>>();

        var sut = new TriggerMapper(triggerService, triggerAttributeLookup, triggerFieldsAttributeLookup, logger);

        // Act
        await sut.MapTriggerProcessors(default);

        // Assert
        Mock.Get(triggerService)
            .Verify(x => x.AddOrUpdateProcessor(It.IsAny<ProcessorTree>()), Times.Once);
        Mock.Get(triggerService)
            .Verify(x => x.AddOrUpdateProcessor(It.Is<ProcessorTree>(t => t.ProcessorSlug == triggerSlug && t.ProcessorType == triggerType && t.Kind == ProcessorKind.Trigger)), Times.Once);
    }

    [Fact(DisplayName = "MapTriggerProcessors when Cancellation is requested should log and stop processing")]
    public async Task MapTriggerProcessors_WhenCancellationIsRequested_ShouldLogAndStopProcessing()
    {
        // Arrange
        const string triggerSlug = "trigger_slug";
        var triggerType = TriggerClassFactory.MatchingClass(triggerSlug: triggerSlug);
        var triggerAttributeLookup = Mock.Of<IAttributeLookup>(x => x.GetAnnotatedTypes() == new[] { triggerType });
        using var cancellationTokenSource = new CancellationTokenSource();
        var token = cancellationTokenSource.Token;

        var triggerFieldsAttributeLookup = Mock.Of<IAttributeLookup>();
        var triggerService = Mock.Of<IProcessorService>();
        Mock.Get(triggerService)
            .Setup(t => t.AddOrUpdateProcessor(It.IsAny<ProcessorTree>()))
            .Returns(async () => await Task.Delay(10_000, token));
        var logger = Mock.Of<ILogger<TriggerMapper>>();

        var sut = new TriggerMapper(triggerService, triggerAttributeLookup, triggerFieldsAttributeLookup, logger);

        // Act
        Task[] tasks = [sut.MapTriggerProcessors(token), cancellationTokenSource.CancelAsync()];
        await Task.WhenAny(tasks);

        // Assert
        logger.VerifyInformationContains($"Mapping for attribute '{typeof(TriggerAttribute)}' was cancelled.", Times.Once);
    }

    [Fact(DisplayName = "MapTriggerProcessors when a new trigger processor with data fields is found should register trigger fields")]
    public async Task MapTriggerProcessors_WhenNewTriggerProcessorWithTriggerFieldsIsFound_ShouldRegisterTriggerFields()
    {
        // Arrange
        const string triggerSlug = "trigger_slug";
        var (triggerType, expectedDataFieldSlugs) = TriggerClassFactory.MatchingClassWithDataFields(triggerSlug: triggerSlug, dataFieldCount: 3);
        var notTriggerType = TriggerClassFactory.MissingTriggerAttribute();
        var triggerAttributeLookup = Mock.Of<IAttributeLookup>(x => x.GetAnnotatedTypes() == new[] { triggerType, notTriggerType });

        var triggerFieldsAttributeLookup = Mock.Of<IAttributeLookup>();
        var triggerService = Mock.Of<IProcessorService>();
        List<string> actualDataFieldSlugs = [];
        Mock.Get(triggerService)
            .Setup(x => x.AddDataField(It.IsAny<string>(), Capture.In(actualDataFieldSlugs), It.IsAny<Type>()));
        Mock.Get(triggerService)
            .Setup(x => x.GetProcessor(It.IsAny<string>()))
            .ReturnsAsync(new ProcessorTree(triggerSlug, triggerType, ProcessorKind.Trigger));

        var logger = Mock.Of<ILogger<TriggerMapper>>();

        var sut = new TriggerMapper(triggerService, triggerAttributeLookup, triggerFieldsAttributeLookup, logger);

        // Act
        await sut.MapTriggerProcessors(default);

        // Assert
        Mock.Get(triggerService)
            .Verify(x => x.AddOrUpdateProcessor(It.IsAny<ProcessorTree>()), Times.Never);
        Mock.Get(triggerService)
            .Verify(x => x.AddDataField(triggerSlug, It.IsAny<string>(), typeof(string)), Times.Exactly(3));
        actualDataFieldSlugs.Should().BeEquivalentTo(expectedDataFieldSlugs);
    }

    [Fact(DisplayName = "MapTriggerProcessors when a trigger processor is already registered with same type should not update trigger processor")]
    public async Task MapTriggerProcessors_WhenTriggerProcessorIsAlreadyRegisteredWithSameType_ShouldNotUpdateProcessor()
    {
        // Arrange
        const string triggerSlug = "trigger_slug";
        var triggerType = TriggerClassFactory.MatchingClass(triggerSlug: triggerSlug);
        var triggerAttributeLookup = Mock.Of<IAttributeLookup>(x => x.GetAnnotatedTypes() == new[] { triggerType });

        var triggerFieldsAttributeLookup = Mock.Of<IAttributeLookup>();
        var triggerService = Mock.Of<IProcessorService>();
        Mock.Get(triggerService)
            .Setup(x => x.GetProcessor(triggerSlug))
            .ReturnsAsync(new ProcessorTree(triggerSlug, triggerType, ProcessorKind.Trigger));

        var logger = Mock.Of<ILogger<TriggerMapper>>();

        var sut = new TriggerMapper(triggerService, triggerAttributeLookup, triggerFieldsAttributeLookup, logger);

        // Act
        await sut.MapTriggerProcessors(default);

        // Assert
        Mock.Get(triggerService)
            .Verify(x => x.AddOrUpdateProcessor(It.IsAny<ProcessorTree>()), Times.Never);
    }

    [Fact(DisplayName = "MapTriggerProcessors when a trigger processor is already registered with a different type should throw")]
    public async Task MapTriggerProcessors_WhenTriggerProcessorIsAlreadyRegisteredWithADifferentType_ShouldThrow()
    {
        // Arrange
        const string triggerSlug = "trigger_slug";
        var triggerType = TriggerClassFactory.MatchingClass(triggerSlug: triggerSlug);
        var anyType = TriggerClassFactory.MissingTriggerAttribute();
        var triggerAttributeLookup = Mock.Of<IAttributeLookup>(x => x.GetAnnotatedTypes() == new[] { triggerType });

        var triggerFieldsAttributeLookup = Mock.Of<IAttributeLookup>();
        var triggerService = Mock.Of<IProcessorService>();
        Mock.Get(triggerService)
            .Setup(x => x.GetProcessor(It.IsAny<string>()))
            .ReturnsAsync(new ProcessorTree(triggerSlug, anyType, ProcessorKind.Trigger));

        var logger = Mock.Of<ILogger<TriggerMapper>>();

        var sut = new TriggerMapper(triggerService, triggerAttributeLookup, triggerFieldsAttributeLookup, logger);

        var expectedLogMsg = $"Mapping for attribute '{typeof(TriggerAttribute)}' has failed.";
        var expectedExceptionMsg = $"Conflict: 'Trigger' processor with slug '{triggerSlug}' already exists (Type is '{anyType}').";

        // Act
        await sut.MapTriggerProcessors(default);

        // Assert
        logger.VerifyErrorContains<InvalidOperationException>(expectedLogMsg,
                                                              expectedExceptionMsg,
                                                              Times.Once);
    }

    [Fact(DisplayName = "MapTriggerFields when a new data fields model matching a processor is found should update trigger processor")]
    public async Task MapTriggerFields_WhenNewTriggerFieldsModelMatchingProcessorIsFound_ShouldUpdateTriggerProcessor()
    {
        // Arrange
        const string triggerSlug = "trigger_slug";
        const string triggerFieldSlug = "trigger_field_slug";
        var triggerFieldsType = TriggerFieldsClassFactory.MatchingTriggerFieldsModel(triggerSlug: triggerSlug, triggerFieldSlug: triggerFieldSlug);
        var triggerAttributeLookup = Mock.Of<IAttributeLookup>();
        var triggerFieldsAttributeLookup = Mock.Of<IAttributeLookup>(x => x.GetAnnotatedTypes() == new[] { triggerFieldsType });

        var triggerService = Mock.Of<IProcessorService>();
        Mock.Get(triggerService)
            .Setup(x => x.Exists(It.IsAny<string>()))
            .ReturnsAsync(true);

        var logger = Mock.Of<ILogger<TriggerMapper>>();

        var sut = new TriggerMapper(triggerService, triggerAttributeLookup, triggerFieldsAttributeLookup, logger);

        // Act
        await sut.MapTriggerFields(default);

        // Assert
        Mock.Get(triggerService)
            .Verify(x => x.AddDataField(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Type>()), Times.Once);
        Mock.Get(triggerService)
            .Verify(x => x.AddDataField(triggerSlug, triggerFieldSlug, typeof(string)), Times.Once);
    }

    [Fact(DisplayName = "MapTriggerFields when data fields model does not match any processor should throw")]
    public void MapTriggerFields_WhenTriggerFieldsModelDoesNotMatchProcessor_ShouldThrow()
    {
        // Arrange
        const string triggerSlug = "trigger_slug";
        const string triggerFieldSlug = "trigger_field_slug";
        var triggerFieldsType = TriggerFieldsClassFactory.MatchingTriggerFieldsModel(triggerSlug: triggerSlug, triggerFieldSlug: triggerFieldSlug);
        var triggerAttributeLookup = Mock.Of<IAttributeLookup>();
        var triggerFieldsAttributeLookup = Mock.Of<IAttributeLookup>(x => x.GetAnnotatedTypes() == new[] { triggerFieldsType });

        var triggerService = Mock.Of<IProcessorService>();
        Mock.Get(triggerService)
            .Setup(x => x.Exists(It.IsAny<string>()))
            .ReturnsAsync(false);

        var logger = Mock.Of<ILogger<TriggerMapper>>();

        var sut = new TriggerMapper(triggerService, triggerAttributeLookup, triggerFieldsAttributeLookup, logger);

        // Act
        var act = () => sut.MapTriggerFields(default);

        // Assert
        act.Should().ThrowExactlyAsync<InvalidOperationException>().WithMessage($"There is no trigger with slug '{triggerSlug}' registered.");
        Mock.Get(triggerService)
            .Verify(x => x.AddDataField(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Type>()), Times.Never);
    }
}
