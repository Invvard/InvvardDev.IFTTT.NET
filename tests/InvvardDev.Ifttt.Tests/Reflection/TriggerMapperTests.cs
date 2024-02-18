using FluentAssertions;
using InvvardDev.Ifttt.Contracts;
using InvvardDev.Ifttt.Models.Core;
using InvvardDev.Ifttt.Models.Trigger;
using InvvardDev.Ifttt.Reflection;
using InvvardDev.Ifttt.TestFactories.Triggers;
using Moq;

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
        var triggerRepository = Mock.Of<IProcessorService>();

        var sut = new TriggerMapper(triggerRepository, triggerAttributeLookup, triggerFieldsAttributeLookup);

        // Act
        await sut.MapTriggerProcessors();

        // Assert
        Mock.Get(triggerRepository)
            .Verify(x => x.AddOrUpdateProcessor(It.IsAny<ProcessorTree>()), Times.Once);
        Mock.Get(triggerRepository)
            .Verify(x => x.AddOrUpdateProcessor(It.Is<ProcessorTree>(t => t.Slug == triggerSlug && t.Type == triggerType && t.Kind == ProcessorKind.Trigger)), Times.Once);
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
        Mock.Get(triggerService).Setup(x => x.AddDataField(It.IsAny<string>(), Capture.In(actualDataFieldSlugs), It.IsAny<Type>()));
        Mock.Get(triggerService).Setup(x => x.GetProcessor(It.IsAny<string>())).ReturnsAsync(new ProcessorTree(triggerSlug, triggerType, ProcessorKind.Trigger));

        var sut = new TriggerMapper(triggerService, triggerAttributeLookup, triggerFieldsAttributeLookup);

        // Act
        await sut.MapTriggerProcessors();

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

        var sut = new TriggerMapper(triggerService, triggerAttributeLookup, triggerFieldsAttributeLookup);

        // Act
        await sut.MapTriggerProcessors();

        // Assert
        Mock.Get(triggerService)
            .Verify(x => x.AddOrUpdateProcessor(It.IsAny<ProcessorTree>()), Times.Never);
    }

    [Fact(DisplayName = "MapTriggerProcessors when a trigger processor is already registered with a different type should throw")]
    public void MapTriggerProcessors_WhenTriggerProcessorIsAlreadyRegisteredWithADifferentType_ShouldThrow()
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

        var sut = new TriggerMapper(triggerService, triggerAttributeLookup, triggerFieldsAttributeLookup);

        // Act
        var act = () => sut.MapTriggerProcessors();

        // Assert
        act.Should().ThrowExactlyAsync<InvalidOperationException>().WithMessage("Trigger has already been registered");
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

        var sut = new TriggerMapper(triggerService, triggerAttributeLookup, triggerFieldsAttributeLookup);

        // Act
        await sut.MapTriggerFields();

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

        var sut = new TriggerMapper(triggerService, triggerAttributeLookup, triggerFieldsAttributeLookup);

        // Act
        var act = () => sut.MapTriggerFields();

        // Assert
        act.Should().ThrowExactlyAsync<InvalidOperationException>().WithMessage($"There is no trigger with slug '{triggerSlug}' registered.");
        Mock.Get(triggerService)
            .Verify(x => x.AddDataField(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Type>()), Times.Never);
    }
}

internal static class TestAssertionExtensions
{
    public static bool HasSameElements<T>(this IEnumerable<T> first, IEnumerable<T> second)
    {
        first.Should().BeEquivalentTo(second);
        return true;
    }
}