using FluentAssertions;
using InvvardDev.Ifttt.Trigger.Contracts;
using InvvardDev.Ifttt.Trigger.Reflection;
using InvvardDev.Ifttt.Trigger.Tests.Factories;
using Moq;

namespace InvvardDev.Ifttt.Trigger.Tests.Reflection;

public class AttributeLookupTests
{
    [Fact(DisplayName = "TriggerAttributeLookup.GetAnnotatedTypes should return all matching Trigger types")]
    public void TriggerAttributeLookup_GetAnnotatedTypes_ShouldReturnAllMatchingTriggerTypes()
    {
        // Arrange
        var missingInterface = TriggerClassFactory.MissingITriggerInterface();
        var missingAttribute = TriggerClassFactory.MissingTriggerAttribute();
        var matchingClass = TriggerClassFactory.MatchingClass();

        var assemblyAccessor = Mock.Of<IAssemblyAccessor>(m => m.GetApplicationAssemblies() == new[]
                                                              {
                                                                  matchingClass.Assembly
                                                              });

        var sut = new TriggerAttributeLookup(assemblyAccessor);

        // Act
        var types = sut.GetAnnotatedTypes();

        // Assert
        types.Should().ContainSingle().Which.FullName.Should().Be(matchingClass.FullName);
    }
    
    [Fact(DisplayName = "TriggerFieldsAttributeLookup.GetAnnotatedTypes should return all matching TriggerFields types")]
    public void TriggerFieldsAttributeLookup_GetAnnotatedTypes_ShouldReturnAllMatchingTriggerTypes()
    {
        // Arrange
        var missingTriggerFieldsAttribute = TriggerClassFactory.MissingTriggerFieldsAttribute();
        var missingAttribute = TriggerClassFactory.MissingTriggerAttribute();
        var matchingClass = TriggerClassFactory.MatchingClass();

        var assemblyAccessor = Mock.Of<IAssemblyAccessor>(m => m.GetApplicationAssemblies() == new[]
                                                              {
                                                                  matchingClass.Assembly
                                                              });

        var sut = new TriggerAttributeLookup(assemblyAccessor);

        // Act
        var types = sut.GetAnnotatedTypes();

        // Assert
        types.Should().ContainSingle().Which.FullName.Should().Be(matchingClass.FullName);
    }
}