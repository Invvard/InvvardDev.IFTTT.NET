using FluentAssertions;
using InvvardDev.Ifttt.Contracts;
using InvvardDev.Ifttt.Models.Core;
using InvvardDev.Ifttt.Models.Trigger;
using InvvardDev.Ifttt.Services;
using InvvardDev.Ifttt.TestFactories.Triggers;
using InvvardDev.Ifttt.Toolkit;
using Moq;

namespace InvvardDev.Ifttt.Tests.Services;

public class ProcessorServiceTests
{
    [Fact(DisplayName = "AddOrUpdateProcessor when a new processor is added, should register new type")]
    public async Task AddOrUpdateProcessor_WhenNewProcessorIsAdded_ShouldRegisterNewType()
    {
        // Arrange
        const string expectedTriggerSlug = "trigger1";
        var expectedTriggerType = TriggerClassFactory.MatchingClass(typeName: "Trigger1", triggerSlug: expectedTriggerSlug);
        var expectedTriggerTree = new ProcessorTree(expectedTriggerSlug, expectedTriggerType, ProcessorKind.Trigger);
        var processorRepository = Mock.Of<IProcessorRepository>();
        Mock.Get(processorRepository)
            .Setup(r => r.GetProcessorByKey(It.IsAny<string>()))
            .ReturnsAsync((ProcessorTree?)null);

        var sut = new TriggerService(processorRepository);

        // Act
        await sut.AddOrUpdateProcessor(expectedTriggerTree);

        // Assert
        Mock.Get(processorRepository)
            .Verify(x => x.AddProcessor(It.Is<ProcessorTree>(t => t.ProcessorSlug == expectedTriggerSlug
                                                                  && t.ProcessorType == expectedTriggerType
                                                                  && t.Kind == ProcessorKind.Trigger)), Times.Once);
    }

    [Fact(DisplayName = "AddOrUpdateProcessor when a Trigger is already registered, should update TriggerType")]
    public async Task AddOrUpdateProcessor_WhenTriggerAlreadyRegistered_ShouldUpdateTriggerType()
    {
        // Arrange
        const string expectedTriggerSlug = "trigger1";
        const string expectedDataFieldSlug = "new_key";

        var anyType = TriggerClassFactory.MatchingClass(triggerSlug: expectedTriggerSlug);
        var expectedTriggerType = TriggerClassFactory.MatchingClass(triggerSlug: expectedTriggerSlug);
        var triggerTree = new ProcessorTree(expectedTriggerSlug, expectedTriggerType, ProcessorKind.Trigger);

        var processorRepository = Mock.Of<IProcessorRepository>();
        Mock.Get(processorRepository).Setup(r => r.GetProcessorByKey(It.IsAny<string>())).ReturnsAsync(triggerTree);

        var sut = new TriggerService(processorRepository);

        var expectedTriggerTree = new ProcessorTree(expectedTriggerSlug, expectedTriggerType, ProcessorKind.Trigger)
                                  {
                                      DataFields = { { expectedDataFieldSlug, anyType } }
                                  };

        // Act
        await sut.AddOrUpdateProcessor(expectedTriggerTree);

        // Assert
        Mock.Get(processorRepository).Verify(x => x.UpdateProcessor(It.IsAny<ProcessorTree>()), Times.Once);
        Mock.Get(processorRepository)
            .Verify(x => x.UpdateProcessor(It.Is<ProcessorTree>(t => t.ProcessorSlug == expectedTriggerSlug
                                                                     && t.ProcessorType == expectedTriggerType
                                                                     && t.Kind == ProcessorKind.Trigger
                                                                     && t.DataFields.Count == 1
                                                                     && t.DataFields.ContainsKey(expectedDataFieldSlug))), Times.Once);
    }

    [Fact(DisplayName = "AddOrUpdateProcessor when a Trigger is already registered with a different type, should throw")]
    public void AddOrUpdateProcessor_WhenTriggerAlreadyRegisteredWithADifferentType_ShouldThrow()
    {
        // Arrange
        const string expectedTriggerSlug = "trigger1";

        var anyType = TriggerClassFactory.MatchingClass(triggerSlug: expectedTriggerSlug);
        var expectedTriggerType = TriggerClassFactory.MatchingClass(triggerSlug: expectedTriggerSlug);
        var triggerTree = new ProcessorTree(expectedTriggerSlug, anyType, ProcessorKind.Trigger);

        var processorRepository = Mock.Of<IProcessorRepository>();
        Mock.Get(processorRepository).Setup(r => r.GetProcessorByKey(triggerTree.Key)).ReturnsAsync(triggerTree);

        var sut = new TriggerService(processorRepository);

        var expectedTriggerTree = new ProcessorTree(expectedTriggerSlug, expectedTriggerType, ProcessorKind.Trigger);

        // Act
        var act = () => sut.AddOrUpdateProcessor(expectedTriggerTree);

        // Assert
        act.Should()
           .ThrowAsync<InvalidOperationException>()
           .WithMessage($"Conflict: '{ProcessorKind.Trigger}' processor with slug '{expectedTriggerSlug}' already exists (Type is '{anyType}').");
    }

    [Fact(DisplayName = "AddDataField when TriggerFields has a matching trigger, then it should register new type")]
    public async Task AddDataField_WhenTriggerFieldsHasMatchingTrigger_ShouldRegister()
    {
        // Arrange
        const string expectedTriggerSlug = "trigger1";
        const string expectedTriggerFieldSlug = "trigger_field_slug";
        var triggerType = TriggerClassFactory.MatchingClass(triggerSlug: expectedTriggerSlug);
        var expectedTriggerFieldsType = TriggerFieldsClassFactory.MatchingTriggerFieldsModel(triggerSlug: expectedTriggerSlug,
                                                                                             triggerFieldSlug: expectedTriggerFieldSlug);

        var processorRepository = Mock.Of<IProcessorRepository>();
        Mock.Get(processorRepository)
            .Setup(r => r.GetProcessorByKey(It.IsAny<string>()))
            .ReturnsAsync(new ProcessorTree(expectedTriggerSlug, triggerType, ProcessorKind.Trigger));

        var sut = new TriggerService(processorRepository);

        // Act
        await sut.AddDataField(expectedTriggerSlug, expectedTriggerFieldSlug, expectedTriggerFieldsType);

        // Assert
        Mock.Get(processorRepository)
            .Verify(x => x.UpdateProcessor(It.Is<ProcessorTree>(t => t.ProcessorSlug == expectedTriggerSlug
                                                                     && t.ProcessorType == triggerType
                                                                     && t.Kind == ProcessorKind.Trigger
                                                                     && t.DataFields.Count == 1
                                                                     && t.DataFields.ContainsKey(expectedTriggerFieldSlug))), Times.Once);
    }

    [Fact(DisplayName = "AddDataField when same data field has already been registered, then it should not register")]
    public async Task AddDataField_WhenSameDataFieldAlreadyRegistered_ShouldNotRegister()
    {
        // Arrange
        const string expectedTriggerSlug = "trigger1";
        const string expectedTriggerFieldSlug = "trigger_field_slug";
        var triggerType = TriggerClassFactory.MatchingClass(triggerSlug: expectedTriggerSlug);

        var triggerTree = new ProcessorTree(expectedTriggerSlug, triggerType, ProcessorKind.Trigger)
                          {
                              DataFields = { { expectedTriggerFieldSlug, typeof(string) } }
                          };
        var processorRepository = Mock.Of<IProcessorRepository>();
        Mock.Get(processorRepository)
            .Setup(r => r.GetProcessorByKey(It.IsAny<string>()))
            .ReturnsAsync(triggerTree);

        var sut = new TriggerService(processorRepository);

        // Act
        await sut.AddDataField(expectedTriggerSlug, expectedTriggerFieldSlug, typeof(string));

        // Assert
        Mock.Get(processorRepository)
            .Verify(x => x.UpdateProcessor(It.IsAny<ProcessorTree>()), Times.Never);
    }

    [Fact(DisplayName = "AddDataField when DataField has been registered with a different type, then it should throw")]
    public void AddDataField_WhenDataFieldHasBeenRegisteredWithADifferentType_ShouldThrow()
    {
        // Arrange
        const string expectedTriggerSlug = "trigger1";
        const string expectedTriggerFieldSlug = "trigger_field_slug";

        var triggerType = TriggerClassFactory.MatchingClass(triggerSlug: expectedTriggerSlug);
        var expectedTriggerFieldsType = TriggerFieldsClassFactory.MatchingTriggerFieldsModel(triggerSlug: expectedTriggerSlug,
                                                                                             triggerFieldSlug: expectedTriggerFieldSlug);

        var triggerTree = new ProcessorTree(expectedTriggerSlug, triggerType, ProcessorKind.Trigger)
                          {
                              DataFields = { { expectedTriggerFieldSlug, typeof(StringReader) } }
                          };
        var processorRepository = Mock.Of<IProcessorRepository>();
        Mock.Get(processorRepository)
            .Setup(r => r.GetProcessorByKey(It.IsAny<string>()))
            .ReturnsAsync(triggerTree);

        var sut = new TriggerService(processorRepository);

        // Act
        var act = () => sut.AddDataField(expectedTriggerSlug, expectedTriggerFieldSlug, expectedTriggerFieldsType);

        // Assert
        Mock.Get(processorRepository).Verify(x => x.UpdateProcessor(It.IsAny<ProcessorTree>()), Times.Never);
        act.Should()
           .ThrowExactlyAsync<InvalidOperationException>()
           .WithMessage($"Conflict: '{ProcessorKind.Trigger}' processor with slug '{expectedTriggerSlug}' already has a data field with slug '{expectedTriggerFieldSlug}' with a different type '{typeof(StringReader)}'.");
    }

    [Fact(DisplayName = "AddDataField when TriggerFields has no matching trigger, then it throws")]
    public void AddDataField_WhenTriggerFieldsHasNoMatchingTrigger_ShouldThrow()
    {
        // Arrange
        const string unknownTriggerSlug = "unknown_trigger_slug";
        const string newTriggerFieldSlug = "new_trigger_field_slug";
        var newTriggerFieldType = TriggerFieldsClassFactory.MatchingTriggerFieldsModel();

        var processorRepository = Mock.Of<IProcessorRepository>();
        Mock.Get(processorRepository).Setup(r => r.GetProcessorByKey(It.IsAny<string>())).ReturnsAsync((ProcessorTree?)null);

        var sut = new TriggerService(processorRepository);

        // Act
        var act = () => sut.AddDataField(unknownTriggerSlug, newTriggerFieldSlug, newTriggerFieldType);

        // Assert
        act.Should().ThrowAsync<InvalidOperationException>().WithMessage($"Processor with slug '{unknownTriggerSlug}' does not exist.");
    }

    [Fact(DisplayName = "Exists when processor exists, then it should return true")]
    public async Task Exists_WhenProcessorExists_ShouldReturnTrue()
    {
        // Arrange
        const string expectedTriggerSlug = "trigger1";

        var processorRepository = Mock.Of<IProcessorRepository>();
        Mock.Get(processorRepository)
            .Setup(r => r.Exists(It.IsAny<string>()))
            .ReturnsAsync(true);

        var sut = new TriggerService(processorRepository);

        // Act
        var result = await sut.Exists(expectedTriggerSlug);

        // Assert
        result.Should().BeTrue();
    }

    [Fact(DisplayName = "Exists when processor does not exist, then it should return false")]
    public async Task Exists_WhenProcessorDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        const string expectedTriggerSlug = "trigger1";

        var processorRepository = Mock.Of<IProcessorRepository>();
        Mock.Get(processorRepository)
            .Setup(r => r.Exists(It.IsAny<string>()))
            .ReturnsAsync(false);

        var sut = new TriggerService(processorRepository);

        // Act
        var result = await sut.Exists(expectedTriggerSlug);

        // Assert
        result.Should().BeFalse();
    }

    [Fact(DisplayName = "GetDataFieldType when processor and data field exist, then it should return the type")]
    public async Task GetDataFieldType_WhenProcessorAndDataFieldExist_ShouldReturnType()
    {
        // Arrange
        const string expectedTriggerSlug = "trigger1";
        const string expectedDataFieldSlug = "data_field_slug";
        var expectedDataFieldType = typeof(string);

        var processorRepository = Mock.Of<IProcessorRepository>();
        Mock.Get(processorRepository)
            .Setup(r => r.GetProcessorByKey(It.IsAny<string>()))
            .ReturnsAsync(new ProcessorTree(expectedTriggerSlug, typeof(string), ProcessorKind.Trigger)
                          {
                              DataFields = { { expectedDataFieldSlug, expectedDataFieldType } }
                          });

        var sut = new TriggerService(processorRepository);

        // Act
        var result = await sut.GetDataFieldType(expectedTriggerSlug, expectedDataFieldSlug);

        // Assert
        result.Should().Be(expectedDataFieldType);
    }
    
    [Fact(DisplayName = "GetDataFieldType when processor does not exist, then it should return null")]
    public async Task GetDataFieldType_WhenProcessorDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        const string unknownTriggerSlug = "trigger1";
        const string expectedDataFieldSlug = "data_field_slug";

        var processorRepository = Mock.Of<IProcessorRepository>();
        Mock.Get(processorRepository)
            .Setup(r => r.GetProcessorByKey(It.IsAny<string>()))
            .ReturnsAsync((ProcessorTree?)null);

        var sut = new TriggerService(processorRepository);

        // Act
        var result = await sut.GetDataFieldType(unknownTriggerSlug, expectedDataFieldSlug);

        // Assert
        result.Should().BeNull();
    }
    
    [Fact(DisplayName = "GetDataFieldType when data field does not exist, then it should return null")]
    public async Task GetDataFieldType_WhenDataFieldDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        const string expectedTriggerSlug = "trigger1";
        const string unknownDataFieldSlug = "unknown_data_field_slug";

        var processorRepository = Mock.Of<IProcessorRepository>();
        Mock.Get(processorRepository)
            .Setup(r => r.GetProcessorByKey(It.IsAny<string>()))
            .ReturnsAsync(new ProcessorTree(expectedTriggerSlug, typeof(string), ProcessorKind.Trigger)
                          {
                              DataFields = { { "data_field_slug", typeof(string) } }
                          });

        var sut = new TriggerService(processorRepository);

        // Act
        var result = await sut.GetDataFieldType(expectedTriggerSlug, unknownDataFieldSlug);

        // Assert
        result.Should().BeNull();
    }
    
    [Fact(DisplayName = "GetProcessor when processor exists, then it should return the processor")]
    public async Task GetProcessor_WhenProcessorExists_ShouldReturnProcessor()
    {
        // Arrange
        const string expectedTriggerSlug = "trigger1";
        var expectedTriggerType = TriggerClassFactory.MatchingClass(triggerSlug: expectedTriggerSlug);
        var expectedTriggerTree = new ProcessorTree(expectedTriggerSlug, expectedTriggerType, ProcessorKind.Trigger);

        var processorRepository = Mock.Of<IProcessorRepository>();
        Mock.Get(processorRepository)
            .Setup(r => r.GetProcessorByKey(It.IsAny<string>()))
            .ReturnsAsync(expectedTriggerTree);

        var sut = new TriggerService(processorRepository);

        // Act
        var result = await sut.GetProcessor(expectedTriggerSlug);

        // Assert
        result.Should().Be(expectedTriggerTree);
    }
    
    [Fact(DisplayName = "GetProcessor when processor does not exist, then it should return null")]
    public async Task GetProcessor_WhenProcessorDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        const string unknownTriggerSlug = "unknown_trigger_slug";

        var processorRepository = Mock.Of<IProcessorRepository>();
        Mock.Get(processorRepository)
            .Setup(r => r.GetProcessorByKey(It.IsAny<string>()))
            .ReturnsAsync((ProcessorTree?)null);

        var sut = new TriggerService(processorRepository);

        // Act
        var result = await sut.GetProcessor(unknownTriggerSlug);

        // Assert
        result.Should().BeNull();
    }
    
    [Fact(DisplayName = "GetProcessorInstance when processor exists, then it should return the processor instance")]
    public async Task GetProcessorInstance_WhenProcessorExists_ShouldReturnProcessorInstance()
    {
        // Arrange
        const string expectedTriggerSlug = "trigger1";
        var expectedTriggerType = TriggerClassFactory.MatchingClass(triggerSlug: expectedTriggerSlug);
        var expectedTriggerTree = new ProcessorTree(expectedTriggerSlug, expectedTriggerType, ProcessorKind.Trigger);

        var processorRepository = Mock.Of<IProcessorRepository>();
        Mock.Get(processorRepository)
            .Setup(r => r.GetProcessorByKey(It.IsAny<string>()))
            .ReturnsAsync(expectedTriggerTree);

        var sut = new TriggerService(processorRepository);

        // Act
        var result = await sut.GetProcessorInstance<ITrigger>(expectedTriggerSlug);

        // Assert
        result.Should().NotBeNull();
    }
    
    [Fact(DisplayName = "GetProcessorInstance when processor does not exist, then it should return null")]
    public async Task GetProcessorInstance_WhenProcessorDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        const string unknownTriggerSlug = "unknown_trigger_slug";

        var processorRepository = Mock.Of<IProcessorRepository>();
        Mock.Get(processorRepository)
            .Setup(r => r.GetProcessorByKey(It.IsAny<string>()))
            .ReturnsAsync((ProcessorTree?)null);

        var sut = new TriggerService(processorRepository);

        // Act
        var result = await sut.GetProcessorInstance<ITrigger>(unknownTriggerSlug);

        // Assert
        result.Should().BeNull();
    }
    
    [Fact(DisplayName = "GetProcessorInstance when processor type does not match, then it should return null")]
    public async Task GetProcessorInstance_WhenProcessorTypeDoesNotMatch_ShouldReturnNull()
    {
        // Arrange
        const string expectedTriggerSlug = "trigger1";
        var expectedTriggerType = TriggerClassFactory.MatchingClass(triggerSlug: expectedTriggerSlug);
        var expectedTriggerTree = new ProcessorTree(expectedTriggerSlug, expectedTriggerType, ProcessorKind.Trigger);

        var processorRepository = Mock.Of<IProcessorRepository>();
        Mock.Get(processorRepository)
            .Setup(r => r.GetProcessorByKey(It.IsAny<string>()))
            .ReturnsAsync(expectedTriggerTree);

        var sut = new TriggerService(processorRepository);

        // Act
        var result = await sut.GetProcessorInstance<IProcessorRepository>(expectedTriggerSlug);

        // Assert
        result.Should().BeNull();
    }
}