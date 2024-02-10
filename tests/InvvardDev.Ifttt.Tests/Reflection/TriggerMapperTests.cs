using FluentAssertions;
using InvvardDev.Ifttt.Contracts;
using InvvardDev.Ifttt.Models.Trigger;
using InvvardDev.Ifttt.Reflection;
using InvvardDev.Ifttt.TestFactories.Triggers;
using Moq;

namespace InvvardDev.Ifttt.Tests.Reflection;

public class TriggerMapperTests
{
    [Fact(DisplayName = "MapTriggerProcessors when a new trigger processor is found should register trigger")]
    public void MapTriggerProcessors_WhenNewTriggerProcessorIsFound_ShouldRegisterTrigger()
    {
        // Arrange
        const string triggerSlug = "trigger_slug";
        var triggerType = TriggerClassFactory.MatchingClass(triggerSlug: triggerSlug);
        var notTriggerType = TriggerClassFactory.MissingTriggerAttribute();
        var triggerAttributeLookup = Mock.Of<IAttributeLookup>(x => x.GetAnnotatedTypes() == new[] { triggerType, notTriggerType });

        var triggerFieldsAttributeLookup = Mock.Of<IAttributeLookup>();
        var triggerRepository = Mock.Of<IProcessorRepository<TriggerMap>>();

        var sut = new TriggerMapper(triggerRepository, triggerAttributeLookup, triggerFieldsAttributeLookup);

        // Act
        sut.MapTriggerProcessors();

        // Assert
        Mock.Get(triggerRepository)
            .Verify(x => x.UpsertProcessor(It.IsAny<string>(), It.IsAny<TriggerMap>()), Times.Once);
        Mock.Get(triggerRepository)
            .Verify(x => x.UpsertProcessor(triggerSlug, It.Is<TriggerMap>(t => t.TriggerSlug == triggerSlug && t.TriggerType == triggerType)), Times.Once);
    }

    [Fact(DisplayName = "MapTriggerProcessors when a new trigger processor with data fields is found should register trigger fields")]
    public void MapTriggerProcessors_WhenNewTriggerProcessorWithTriggerFieldsIsFound_ShouldRegisterTriggerFields()
    {
        // Arrange
        const string triggerSlug = "trigger_slug";
        const int dataFieldCount = 3;
        var (triggerType, dataFieldSlugs) = TriggerClassFactory.MatchingClassWithDataFields(triggerSlug: triggerSlug, dataFieldCount: 3);
        var notTriggerType = TriggerClassFactory.MissingTriggerAttribute();
        var triggerAttributeLookup = Mock.Of<IAttributeLookup>(x => x.GetAnnotatedTypes() == new[] { triggerType, notTriggerType });

        var triggerFieldsAttributeLookup = Mock.Of<IAttributeLookup>();
        var triggerRepository = Mock.Of<IProcessorRepository<TriggerMap>>();

        var sut = new TriggerMapper(triggerRepository, triggerAttributeLookup, triggerFieldsAttributeLookup);

        // Act
        sut.MapTriggerProcessors();

        // Assert
        Mock.Get(triggerRepository)
            .Verify(x => x.UpsertProcessor(It.IsAny<string>(), It.IsAny<TriggerMap>()), Times.Once);
        Mock.Get(triggerRepository)
            .Verify(x => x.UpsertProcessor(triggerSlug,
                                           It.Is<TriggerMap>(t => t.TriggerSlug == triggerSlug
                                                                  && t.TriggerType == triggerType
                                                                  && t.TriggerFields.Count == dataFieldCount
                                                                  && t.TriggerFields
                                                                      .Select(f => f.TriggerFieldSlug)
                                                                      .HasSameElements(dataFieldSlugs))),
                    Times.Once);
    }

    [Fact(DisplayName = "MapTriggerProcessors when a trigger processor is already registered with same type should update trigger map")]
    public void MapTriggerProcessors_WhenTriggerProcessorIsAlreadyRegisteredWithSameType_ShouldUpdateTriggerMap()
    {
        // Arrange
        const string triggerSlug = "trigger_slug";
        var (triggerType, dataFieldSlugs) = TriggerClassFactory.MatchingClassWithDataFields(triggerSlug: triggerSlug);
        var triggerAttributeLookup = Mock.Of<IAttributeLookup>(x => x.GetAnnotatedTypes() == new[] { triggerType });

        var triggerFieldsAttributeLookup = Mock.Of<IAttributeLookup>();
        var triggerRepository = Mock.Of<IProcessorRepository<TriggerMap>>(r => r.GetProcessor(triggerSlug) == new TriggerMap(triggerSlug, triggerType));

        var sut = new TriggerMapper(triggerRepository, triggerAttributeLookup, triggerFieldsAttributeLookup);

        // Act
        sut.MapTriggerProcessors();

        // Assert
        Mock.Get(triggerRepository)
            .Verify(x => x.UpsertProcessor(It.IsAny<string>(), It.IsAny<TriggerMap>()), Times.Once);
        Mock.Get(triggerRepository)
            .Verify(x => x.UpsertProcessor(triggerSlug,
                                           It.Is<TriggerMap>(t => t.TriggerSlug == triggerSlug
                                                                  && t.TriggerType == triggerType
                                                                  && t.TriggerFields
                                                                      .Select(f => f.TriggerFieldSlug)
                                                                      .HasSameElements(dataFieldSlugs))),
                    Times.Once);
    }

    [Fact(DisplayName = "MapTriggerProcessors when a trigger processor is already registered should throw")]
    public void MapTriggerProcessors_WhenTriggerProcessorIsAlreadyRegistered_ShouldThrow()
    {
        // Arrange
        const string triggerSlug = "trigger_slug";
        var triggerType = TriggerClassFactory.MatchingClass(triggerSlug: triggerSlug);
        var anyType = TriggerClassFactory.MissingTriggerAttribute();
        var triggerAttributeLookup = Mock.Of<IAttributeLookup>(x => x.GetAnnotatedTypes() == new[] { triggerType });

        var triggerFieldsAttributeLookup = Mock.Of<IAttributeLookup>();
        var triggerRepository = Mock.Of<IProcessorRepository<TriggerMap>>(r => r.GetProcessor(triggerSlug) == new TriggerMap(triggerSlug, anyType));

        var sut = new TriggerMapper(triggerRepository, triggerAttributeLookup, triggerFieldsAttributeLookup);

        // Act
        var act = () => sut.MapTriggerProcessors();

        // Assert
        act.Should().Throw<InvalidOperationException>().WithMessage("Trigger has already been registered");
    }

    [Fact(DisplayName = "MapTriggerFields when a new trigger fields model matching a processor is found should update trigger processor")]
    public void MapTriggerFields_WhenNewTriggerFieldsModelMatchingProcessorIsFound_ShouldUpdateTriggerProcessor()
    {
        // Arrange
        const string triggerSlug = "trigger_slug";
        const string triggerFieldSlug = "trigger_field_slug";
        var triggerFieldsType = TriggerFieldsClassFactory.MatchingTriggerFieldsModel(triggerSlug: triggerSlug, triggerFieldSlug: triggerFieldSlug);
        var triggerAttributeLookup = Mock.Of<IAttributeLookup>();
        var triggerFieldsAttributeLookup = Mock.Of<IAttributeLookup>(x => x.GetAnnotatedTypes() == new[] { triggerFieldsType });

        var triggerMap = new TriggerMap(triggerSlug, TriggerClassFactory.MatchingClass(triggerSlug: triggerSlug));
        var triggerRepository = Mock.Of<IProcessorRepository<TriggerMap>>(x => x.GetProcessor(triggerSlug) == triggerMap);

        var sut = new TriggerMapper(triggerRepository, triggerAttributeLookup, triggerFieldsAttributeLookup);

        // Act
        sut.MapTriggerFields();

        // Assert
        Mock.Get(triggerRepository)
            .Verify(x => x.UpsertProcessor(It.IsAny<string>(), It.IsAny<TriggerMap>()), Times.Once);
        Mock.Get(triggerRepository)
            .Verify(x => x.UpsertProcessor(triggerSlug,
                                           It.Is<TriggerMap>(t => t.TriggerSlug == triggerSlug
                                                                  && t.TriggerFields.Count == 1
                                                                  && t.TriggerFields
                                                                      .Select(f => f.TriggerFieldSlug)
                                                                      .HasSameElements(new[] { triggerFieldSlug }))),
                    Times.Once);
    }
}

internal static class TestAssertionExtensions
{
    public static bool HasSameElements<T>(this IEnumerable<T> first, IEnumerable<T> second)
    {
        first.Should().BeEquivalentTo(second);
        return true;
    }
}