using FluentAssertions;
using InvvardDev.Ifttt.Trigger.Attributes;
using InvvardDev.Ifttt.Trigger.Contracts;
using InvvardDev.Ifttt.Trigger.Models;
using InvvardDev.Ifttt.Trigger.Reflection;
using InvvardDev.Ifttt.Trigger.Tests.Factories;

namespace InvvardDev.Ifttt.Trigger.Tests.Reflection;

public class AttributeLookupTests
{
    [Fact]
    public void Test()
    {
        // Arrange
        var expectedSlug = "my_new_trigger_slug";
        
        // Act
        var actualSlug = TestClassFactory.GivenATrigger(expectedSlug);
        
        // Assert
        actualSlug.Should().Be(expectedSlug);
    }
    
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
        public Task ExecuteAsync(TriggerRequest triggerRequest, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }

    [Trigger("slug_2")]
    public class MissingInterface { }

    public class MissingAttribute : ITrigger
    {
        public Task ExecuteAsync(TriggerRequest triggerRequest, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}