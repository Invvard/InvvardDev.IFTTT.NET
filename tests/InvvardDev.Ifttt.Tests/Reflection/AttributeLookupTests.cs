using InvvardDev.Ifttt.Contracts;
using InvvardDev.Ifttt.Reflection;
using InvvardDev.Ifttt.TestFactories.Triggers;

namespace InvvardDev.Ifttt.Tests.Reflection;

public class AttributeLookupTests
{
    [Fact(DisplayName = "TriggerAttributeLookup.GetAnnotatedTypes should return all matching Trigger types")]
    public void TriggerAttributeLookup_GetAnnotatedTypes_ShouldReturnAllMatchingTriggerTypes()
    {
        // Arrange
        TriggerClassFactory.MissingITriggerInterface();
        TriggerClassFactory.MissingTriggerAttribute();
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
        TriggerFieldsClassFactory.MissingTriggerFieldsAttribute();
        TriggerFieldsClassFactory.MissingTriggerFieldProperty();
        var matchingTriggerFieldsClass = TriggerFieldsClassFactory.MatchingTriggerFieldsModel();

        var assemblyAccessor = Mock.Of<IAssemblyAccessor>(m => m.GetApplicationAssemblies() == new[]
                                                              {
                                                                  matchingTriggerFieldsClass.Assembly
                                                              });

        var sut = new TriggerFieldsAttributeLookup(assemblyAccessor);

        // Act
        var types = sut.GetAnnotatedTypes();

        // Assert
        types.Should().ContainSingle().Which.FullName.Should().Be(matchingTriggerFieldsClass.FullName);
    }
}