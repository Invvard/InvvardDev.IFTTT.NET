using FluentAssertions;
using InvvardDev.Ifttt.Service.Api.Trigger.Attributes;
using InvvardDev.Ifttt.Service.Api.Trigger.Contracts;
using InvvardDev.Ifttt.Service.Api.Trigger.Models;
using InvvardDev.Ifttt.Service.Api.Trigger.Repositories;
using Moq;

namespace InvvardDev.Ifttt.Service.Trigger.Tests.Repositories;

public class TriggerRepositoryServiceTests
{
    // Create a test that verifies that the TriggerRepositoryService.MapTriggerTypes method register the types and their instance returned by IAttributeLookup.GetAnnotatedTypes
    [Fact]
    public void MapTriggerTypes_ShouldRegisterAllAnnotatedTypes()
    {
        // Arrange
        var attributeLookupMock = new Mock<IAttributeLookup>();
        attributeLookupMock.Setup(x => x.GetAnnotatedTypes())
                           .Returns(new[] { typeof(Trigger1), typeof(Trigger2) });

        var sut = new TriggerRepositoryService(attributeLookupMock.Object);

        // Act
        sut.MapTriggerTypes();

        // Assert
        sut.GetTriggerProcessorInstance(nameof(Trigger1)).Should().BeOfType<Trigger1>();
        sut.GetTriggerProcessorInstance(nameof(Trigger2)).Should().BeOfType<Trigger2>();
    }
}

[Trigger(nameof(Trigger1))]
internal class Trigger1 : ITrigger
{
    public Task ExecuteAsync(TriggerRequestBase triggerRequest, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}

[Trigger(nameof(Trigger2))]
internal class Trigger2 : ITrigger
{
    public Task ExecuteAsync(TriggerRequestBase triggerRequest, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}