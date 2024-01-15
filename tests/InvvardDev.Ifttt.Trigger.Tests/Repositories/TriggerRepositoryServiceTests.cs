using FluentAssertions;
using InvvardDev.Ifttt.Trigger.Attributes;
using InvvardDev.Ifttt.Trigger.Contracts;
using InvvardDev.Ifttt.Trigger.Models;
using InvvardDev.Ifttt.Trigger.Repositories;
using Moq;

namespace InvvardDev.Ifttt.Trigger.Tests.Repositories;

public class TriggerRepositoryServiceTests
{
    [Fact(DisplayName = "When MapTriggerTypes is called, Then it should register all annotated types")]
    public void MapTriggerTypes_ShouldRegisterAllAnnotatedTypes()
    {
        // Arrange
        var triggerAttributeLookupMock = new Mock<IAttributeLookup>();
        triggerAttributeLookupMock.Setup(x => x.GetAnnotatedTypes())
                                  .Returns(new[] { typeof(Trigger1), typeof(Trigger2) });

        var sut = new TriggerRepositoryService();

        // Act
        //sut.MapTriggerTypes();

        // Assert
        sut.GetTriggerProcessorInstance(nameof(Trigger1))
           .Should()
           .BeOfType<Trigger1>();
        sut.GetTriggerProcessorInstance(nameof(Trigger2))
           .Should()
           .BeOfType<Trigger2>();
    }

    [Fact(DisplayName = "When MapTriggerFields is called, then it should register all annotated types that match a trigger type")]
    public void MapTriggerFields_ShouldRegisterAllAnnotatedTypes()
    {
        // Arrange
        var triggerAttributeLookupMock = new Mock<IAttributeLookup>();
        triggerAttributeLookupMock.Setup(x => x.GetAnnotatedTypes())
                                  .Returns(new[] { typeof(Trigger1) });

        var triggerFieldsAttributeLookupMock = new Mock<IAttributeLookup>();
        triggerFieldsAttributeLookupMock.Setup(x => x.GetAnnotatedTypes())
                                        .Returns(new[] { typeof(Trigger1Fields) });

        var sut = new TriggerRepositoryService();

        // Act
        //sut.MapTriggerTypes().MapTriggerFields();

        // Assert
        sut.GetTriggerFieldsType(nameof(Trigger1)).Should().Be<Trigger1Fields>();
    }

    [Fact(DisplayName
                 = "When MapTriggerFields is called, when TriggerFields has no matching trigger, then it should not register type")]
    public void MapTriggerFields_WhenTriggerFieldsHasNoMatchingTrigger_ShouldNotRegisterType()
    {
        // Arrange
        var triggerAttributeLookupMock = new Mock<IAttributeLookup>();
        triggerAttributeLookupMock.Setup(x => x.GetAnnotatedTypes())
                                  .Returns(new[] { typeof(Trigger2) });

        var triggerFieldsAttributeLookupMock = new Mock<IAttributeLookup>();
        triggerFieldsAttributeLookupMock.Setup(x => x.GetAnnotatedTypes())
                                        .Returns(new[] { typeof(Trigger1Fields) });

        var sut = new TriggerRepositoryService();

        // Act
        //sut.MapTriggerTypes().MapTriggerFields();

        // Assert
        sut.GetTriggerFieldsType(nameof(Trigger1Fields)).Should().BeNull();
    }
}

[Trigger(nameof(Trigger1))]
internal class Trigger1 : ITrigger
{
    public Task ExecuteAsync(TriggerRequest triggerRequest, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}

[Trigger(nameof(Trigger2))]
internal class Trigger2 : ITrigger
{
    public Task ExecuteAsync(TriggerRequest triggerRequest, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}

[TriggerFields(nameof(Trigger1))]
internal class Trigger1Fields;