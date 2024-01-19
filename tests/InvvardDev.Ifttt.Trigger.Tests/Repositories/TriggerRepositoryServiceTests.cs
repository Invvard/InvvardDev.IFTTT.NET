using FluentAssertions;
using InvvardDev.Ifttt.Trigger.Repositories;
using InvvardDev.Ifttt.Trigger.Tests.Factories;

namespace InvvardDev.Ifttt.Trigger.Tests.Repositories;

public class TriggerRepositoryServiceTests
{
    [Fact(DisplayName = "AddOrUpdateTrigger, when a new Trigger is added, should register new type")]
    public void AddOrUpdateTrigger_WhenNewTriggerIsAdded_ShouldRegisterNewType()
    {
        // Arrange
        const string expectedTriggerSlug = "trigger1";
        var expectedTriggerType = TriggerClassFactory.MatchingClass(typeName: "Trigger1", triggerSlug: expectedTriggerSlug);

        var sut = new TriggerRepositoryService();
        sut.GetTriggerProcessorInstance(expectedTriggerSlug).Should().BeNull();

        // Act
        sut.AddOrUpdateTrigger(expectedTriggerSlug, expectedTriggerType);

        // Assert
        sut.GetTriggerProcessorInstance(expectedTriggerSlug)
           .Should()
           .NotBeNull()
           .And
           .BeOfType(expectedTriggerType);
    }

    [Fact(DisplayName = "AddOrUpdateTrigger, when a Trigger is already registered, should update TriggerType")]
    public void AddOrUpdateTrigger_WhenTriggerAlreadyRegistered_ShouldUpdateTriggerType()
    {
        // Arrange
        const string expectedTriggerSlug = "trigger1";
        var anyType = TriggerClassFactory.MatchingClass(triggerSlug: expectedTriggerSlug);
        var expectedTriggerType = TriggerClassFactory.MatchingClass(triggerSlug: expectedTriggerSlug);

        var sut = new TriggerRepositoryService();
        sut.AddOrUpdateTrigger(expectedTriggerSlug, anyType);
        sut.GetTriggerProcessorInstance(expectedTriggerSlug).Should().BeOfType(anyType);

        // Act
        sut.AddOrUpdateTrigger(expectedTriggerSlug, expectedTriggerType);

        // Assert
        sut.GetTriggerProcessorInstance(expectedTriggerSlug).Should().BeOfType(expectedTriggerType);
    }

    [Fact(DisplayName = "AddOrUpdateTriggerFields, when TriggerFields has no matching trigger, then it should throw")]
    public void AddOrUpdateTriggerFields_WhenTriggerFieldsHasNoMatchingTrigger_ShouldThrow()
    {
        // Arrange
        const string triggerSlug = "trigger1";
        const string unknownTriggerSlug = "unknown_trigger_slug";

        var triggerType = TriggerClassFactory.MatchingClass(triggerSlug: triggerSlug);
        var expectedTriggerFieldsType = TriggerFieldsClassFactory.MatchingTriggerFieldsClass(triggerSlug: unknownTriggerSlug);

        var sut = new TriggerRepositoryService();
        sut.AddOrUpdateTrigger(triggerSlug, triggerType);

        // Act
        var act = () => sut.AddOrUpdateTriggerFields(unknownTriggerSlug, expectedTriggerFieldsType);

        // Assert
        act.Should()
           .Throw<Exception>()
           .Which
           .Should()
           .BeOfType<InvalidOperationException>()
           .Which
           .Message
           .Should()
           .Be($"Trigger '{unknownTriggerSlug}' was not found.");

        sut.GetTriggerFieldsType(unknownTriggerSlug).Should().BeNull();
    }

    [Fact(DisplayName = "AddOrUpdateTriggerFields, when TriggerFields has a matching trigger, then it should register new type")]
    public void AddOrUpdateTriggerFields_WhenTriggerFieldsHasMatchingTrigger_ShouldRegister()
    {
        // Arrange
        const string expectedTriggerSlug = "trigger1";
        var triggerType = TriggerClassFactory.MatchingClass(triggerSlug: expectedTriggerSlug);
        var expectedTriggerFieldsType
            = TriggerFieldsClassFactory.MatchingTriggerFieldsClass(triggerSlug: expectedTriggerSlug);

        var sut = new TriggerRepositoryService();
        sut.AddOrUpdateTrigger(expectedTriggerSlug, triggerType);

        // Act
        sut.AddOrUpdateTriggerFields(expectedTriggerSlug, expectedTriggerFieldsType);

        // Assert
        sut.GetTriggerFieldsType(expectedTriggerSlug).Should().NotBeNull().And.Be(expectedTriggerFieldsType);
    }
}