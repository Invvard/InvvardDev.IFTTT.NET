using FluentAssertions;
using InvvardDev.Ifttt.TestFactories.Triggers;
using InvvardDev.Ifttt.Trigger.Models;
using InvvardDev.Ifttt.Trigger.Services;

namespace InvvardDev.Ifttt.Trigger.Tests.Services;

public class TriggerRepositoryTests
{
    [Fact(DisplayName = "UpsertProcessor, when a new Trigger is added, should register new type")]
    public void UUpsertProcessor_WhenNewTriggerIsAdded_ShouldRegisterNewType()
    {
        // Arrange
        const string expectedTriggerSlug = "trigger1";
        var expectedTriggerType = TriggerClassFactory.MatchingClass(typeName: "Trigger1", triggerSlug: expectedTriggerSlug);
        var expectedTriggerMap = new TriggerMap(expectedTriggerSlug, expectedTriggerType);

        var sut = new TriggerRepository();
        sut.GetProcessor(expectedTriggerSlug).Should().BeNull();

        // Act
        sut.UpsertProcessor(expectedTriggerSlug, expectedTriggerMap);

        // Assert
        sut.GetProcessor(expectedTriggerSlug).Should().BeEquivalentTo(expectedTriggerMap);
    }

    [Fact(DisplayName = "UpsertProcessor, when a Trigger is already registered, should update TriggerType")]
    public void UpsertProcessor_WhenTriggerAlreadyRegistered_ShouldUpdateTriggerType()
    {
        // Arrange
        const string expectedTriggerSlug = "trigger1";
        var anyType = TriggerClassFactory.MatchingClass(triggerSlug: expectedTriggerSlug);
        var expectedTriggerType = TriggerClassFactory.MatchingClass(triggerSlug: expectedTriggerSlug);
        var expectedTriggerMap = new TriggerMap(expectedTriggerSlug, expectedTriggerType);

        var sut = new TriggerRepository();
        sut.UpsertProcessor(expectedTriggerSlug, new TriggerMap(expectedTriggerSlug, anyType));
        sut.GetProcessor(expectedTriggerSlug)
           .Should()
           .NotBeNull()
           .And
           .Subject
           .As<TriggerMap>()
           .TriggerType
           .Should()
           .Be(anyType);

        // Act
        sut.UpsertProcessor(expectedTriggerSlug, new TriggerMap(expectedTriggerSlug, expectedTriggerType));

        // Assert
        sut.GetProcessor(expectedTriggerSlug).Should().BeEquivalentTo(expectedTriggerMap);
    }

    [Fact(DisplayName = "UpsertDataField, when TriggerFields has no matching trigger, then it returns null")]
    public void UpsertDataField_WhenTriggerFieldsHasNoMatchingTrigger_ShouldBeNull()
    {
        // Arrange
        const string unknownTriggerSlug = "unknown_trigger_slug";
        const string newTriggerFieldSlug = "new_trigger_field_slug";
        var newTriggerFieldType = TriggerFieldsClassFactory.MatchingTriggerFieldsClass();
        var sut = new TriggerRepository();

        // Act
        sut.UpsertDataField(unknownTriggerSlug, newTriggerFieldSlug, newTriggerFieldType);

        // Assert
        sut.GetDataFieldType(unknownTriggerSlug, newTriggerFieldSlug).Should().BeNull();
    }

    [Fact(DisplayName = "UpsertDataField, when TriggerFields has a matching trigger, then it should register new type")]
    public void UpsertDataField_WhenTriggerFieldsHasMatchingTrigger_ShouldRegister()
    {
        // Arrange
        const string expectedTriggerSlug = "trigger1";
        const string expectedTriggerFieldSlug = "trigger_field_slug";
        var triggerType = TriggerClassFactory.MatchingClass(triggerSlug: expectedTriggerSlug);
        var expectedTriggerFieldsType = TriggerFieldsClassFactory.MatchingTriggerFieldsClass(triggerSlug: expectedTriggerSlug,
         triggerFieldSlug: expectedTriggerFieldSlug);

        var sut = new TriggerRepository();
        sut.UpsertProcessor(expectedTriggerSlug, new TriggerMap(expectedTriggerSlug, triggerType));

        // Act
        sut.UpsertDataField(expectedTriggerSlug, expectedTriggerFieldSlug, expectedTriggerFieldsType);

        // Assert
        sut.GetDataFieldType(expectedTriggerSlug, expectedTriggerFieldSlug)
           .Should()
           .NotBeNull()
           .And
           .Be(expectedTriggerFieldsType);
    }
}