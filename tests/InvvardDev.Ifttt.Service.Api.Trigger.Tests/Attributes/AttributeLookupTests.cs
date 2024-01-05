using FluentAssertions;
using InvvardDev.Ifttt.Service.Api.Trigger.Attributes;
using InvvardDev.Ifttt.Service.Api.Trigger.Contracts;
using InvvardDev.Ifttt.Service.Api.Trigger.Models;

namespace InvvardDev.Ifttt.Service.Trigger.Tests.Attributes;

public class AttributeLookupTests
{
    [Fact(DisplayName = "GetAnnotatedTypes should return all matching Trigger types")]
    public void GetAnnotatedTypes_ShouldReturnAllMatchingTriggerTypes()
    {
        // Arrange
        var sut = new TriggerAttributeLookup();
        var expectedAssemblyFullName = typeof(MatchingClass).FullName;

        // Act
        var types = sut.GetAnnotatedTypes();

        // Assert
        types.Should().ContainSingle().Which.FullName.Should().Be(expectedAssemblyFullName);
    }
    
    [Trigger("slug_1")]
    private class MatchingClass : ITrigger
    {
        public Task ExecuteAsync(TriggerRequestBase triggerRequest, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }

    [Trigger("slug_2")]
    public class MissingInterface { }

    public class MissingAttribute : ITrigger
    {
        public Task ExecuteAsync(TriggerRequestBase triggerRequest, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}