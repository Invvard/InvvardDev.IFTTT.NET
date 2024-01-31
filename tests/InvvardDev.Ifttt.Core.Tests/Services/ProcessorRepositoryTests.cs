using FluentAssertions;
using InvvardDev.Ifttt.Core.Services;
using InvvardDev.Ifttt.TestFactories.Triggers;
using InvvardDev.Ifttt.Trigger.Contracts;

namespace InvvardDev.Ifttt.Core.Tests.Services;

public class ProcessorRepositoryTests
{
    [Fact(DisplayName = "UpsertProcessorType, when a new Trigger is added, should register new type")]
    public void UpsertProcessorType_WhenNewTriggerIsAdded_ShouldRegisterNewType()
    {
        // Arrange
        const string expectedTriggerSlug = "trigger1";
        var expectedTriggerType = TriggerClassFactory.MatchingClass(typeName: "Trigger1", triggerSlug: expectedTriggerSlug);

        var sut = new ProcessorRepository();
        sut.GetProcessorInstance<ITrigger>(expectedTriggerSlug).Should().BeNull();

        // Act
        sut.UpsertProcessorType(expectedTriggerSlug, expectedTriggerType);

        // Assert
        sut.GetProcessorInstance<ITrigger>(expectedTriggerSlug)
           .Should()
           .NotBeNull()
           .And
           .BeOfType(expectedTriggerType);
    }

    [Fact(DisplayName = "UpsertProcessorType, when a Trigger is already registered, should update TriggerType")]
    public void UpsertProcessorType_WhenTriggerAlreadyRegistered_ShouldUpdateTriggerType()
    {
        // Arrange
        const string expectedTriggerSlug = "trigger1";
        var anyType = TriggerClassFactory.MatchingClass(triggerSlug: expectedTriggerSlug);
        var expectedTriggerType = TriggerClassFactory.MatchingClass(triggerSlug: expectedTriggerSlug);

        var sut = new ProcessorRepository();
        sut.UpsertProcessorType(expectedTriggerSlug, anyType);
        sut.GetProcessorInstance<ITrigger>(expectedTriggerSlug).Should().BeOfType(anyType);

        // Act
        sut.UpsertProcessorType(expectedTriggerSlug, expectedTriggerType);

        // Assert
        sut.GetProcessorInstance<ITrigger>(expectedTriggerSlug).Should().BeOfType(expectedTriggerType);
    }

    [Fact(DisplayName = "UpsertDataFieldsType, when TriggerFields has no matching trigger, then it should throw")]
    public void UpsertDataFieldsType_WhenTriggerFieldsHasNoMatchingTrigger_ShouldThrow()
    {
        // Arrange
        const string triggerSlug = "trigger1";
        const string unknownTriggerSlug = "unknown_trigger_slug";

        var triggerType = TriggerClassFactory.MatchingClass(triggerSlug: triggerSlug);
        var expectedTriggerFieldsType = TriggerFieldsClassFactory.MatchingTriggerFieldsClass(triggerSlug: unknownTriggerSlug);

        var sut = new ProcessorRepository();
        sut.UpsertProcessorType(triggerSlug, triggerType);

        // Act
        var act = () => sut.UpsertDataFieldsType(unknownTriggerSlug, expectedTriggerFieldsType);

        // Assert
        act.Should()
           .Throw<Exception>()
           .Which
           .Should()
           .BeOfType<InvalidOperationException>()
           .Which
           .Message
           .Should()
           .Be($"Data type '{unknownTriggerSlug}' was not found.");

        sut.GetDataFieldsType(unknownTriggerSlug).Should().BeNull();
    }

    [Fact(DisplayName = "UpsertDataFieldsType, when TriggerFields has a matching trigger, then it should register new type")]
    public void UpsertDataFieldsType_WhenTriggerFieldsHasMatchingTrigger_ShouldRegister()
    {
        // Arrange
        const string expectedTriggerSlug = "trigger1";
        var triggerType = TriggerClassFactory.MatchingClass(triggerSlug: expectedTriggerSlug);
        var expectedTriggerFieldsType
            = TriggerFieldsClassFactory.MatchingTriggerFieldsClass(triggerSlug: expectedTriggerSlug);

        var sut = new ProcessorRepository();
        sut.UpsertProcessorType(expectedTriggerSlug, triggerType);

        // Act
        sut.UpsertDataFieldsType(expectedTriggerSlug, expectedTriggerFieldsType);

        // Assert
        sut.GetDataFieldsType(expectedTriggerSlug).Should().NotBeNull().And.Be(expectedTriggerFieldsType);
    }
}