using FluentAssertions;
using InvvardDev.Ifttt.TestFactories.Triggers;
using InvvardDev.Ifttt.Trigger.Models.Contracts;
using Moq;

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
        sut.GetInstance<ITrigger>(expectedTriggerSlug).Should().BeNull();

        // Act
        sut.UpsertType(expectedTriggerSlug, expectedTriggerType);

        // Assert
        sut.GetInstance<ITrigger>(expectedTriggerSlug)
           .Should()
           .NotBeNull()
           .And
           .BeOfType(expectedTriggerType);
    }

    [Fact(DisplayName = "UpsertType, when a Trigger is already registered, should update TriggerType")]
    public void UpsertProcessorType_WhenTriggerAlreadyRegistered_ShouldUpdateTriggerType()
    {
        // Arrange
        const string expectedTriggerSlug = "trigger1";
        var anyType = TriggerClassFactory.MatchingClass(triggerSlug: expectedTriggerSlug);
        var expectedTriggerType = TriggerClassFactory.MatchingClass(triggerSlug: expectedTriggerSlug);

        var sut = new ProcessorRepository();
        sut.UpsertType(expectedTriggerSlug, anyType);
        sut.GetInstance<ITrigger>(expectedTriggerSlug).Should().BeOfType(anyType);

        // Act
        sut.UpsertType(expectedTriggerSlug, expectedTriggerType);

        // Assert
        sut.GetInstance<ITrigger>(expectedTriggerSlug).Should().BeOfType(expectedTriggerType);
    }

    [Fact(DisplayName = "UpsertType, when TriggerFields has no matching trigger, then it should throw")]
    public void UpsertDataFieldsType_WhenTriggerFieldsHasNoMatchingTrigger_ShouldThrow()
    {
        // Arrange
        const string triggerSlug = "trigger1";
        const string unknownTriggerSlug = "unknown_trigger_slug";

        var triggerType = TriggerClassFactory.MatchingClass(triggerSlug: triggerSlug);
        var expectedTriggerFieldsType = TriggerFieldsClassFactory.MatchingTriggerFieldsClass(triggerSlug: unknownTriggerSlug);

        var triggerRepository = Mock.Of<IRepository>(x => x.GetDataType(triggerSlug) == triggerType);
        var sut = new DataFieldsRepository(triggerRepository);

        // Act
        var act = () => sut.UpsertType(unknownTriggerSlug, expectedTriggerFieldsType);

        // Assert
        act.Should()
           .Throw<Exception>()
           .Which
           .Should()
           .BeOfType<InvalidOperationException>()
           .Which
           .Message
           .Should()
           .Be($"Unable to find a processor with '{unknownTriggerSlug}' to attach Data Fields to.");

        sut.GetDataType(unknownTriggerSlug).Should().BeNull();
    }

    [Fact(DisplayName = "UpsertType, when TriggerFields has a matching trigger, then it should register new type")]
    public void UpsertDataFieldsType_WhenTriggerFieldsHasMatchingTrigger_ShouldRegister()
    {
        // Arrange
        const string expectedTriggerSlug = "trigger1";
        var triggerType = TriggerClassFactory.MatchingClass(triggerSlug: expectedTriggerSlug);
        var expectedTriggerFieldsType = TriggerFieldsClassFactory.MatchingTriggerFieldsClass(triggerSlug: expectedTriggerSlug);

        var triggerRepository = Mock.Of<IRepository>(x => x.GetDataType(expectedTriggerSlug) == triggerType);
        var sut = new DataFieldsRepository(triggerRepository);

        // Act
        sut.UpsertType(expectedTriggerSlug, expectedTriggerFieldsType);

        // Assert
        sut.GetDataType(expectedTriggerSlug).Should().NotBeNull().And.Be(expectedTriggerFieldsType);
    }
}