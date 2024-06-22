using FluentAssertions;
using InvvardDev.Ifttt.Models.Trigger;
using InvvardDev.Ifttt.Services;

namespace InvvardDev.Ifttt.Tests.Services;

public class ProcessorRepositoryTests
{
    [Fact(DisplayName = "AddProcessor adds processor successfully")]
    public async Task AddProcessor_AddsProcessorSuccessfully()
    {
        // Arrange
        var processorTree = Given.A<ProcessorTree>();
        var sut = new ProcessorRepository();

        // Act
        await sut.AddProcessor(processorTree);

        // Assert
        (await sut.Exists(processorTree.Key)).Should().BeTrue();
    }

    [Fact(DisplayName = "UpdateProcessor when processor exists updates processor successfully")]
    public async Task UpdateProcessor_WhenProcessorExists_UpdatesProcessorSuccessfully()
    {
        // Arrange
        var sut = new ProcessorRepository();
        var processorTree = Given.A<ProcessorTree>();
        await sut.AddProcessor(processorTree);

        var updatedProcessorTree = Given.A<ProcessorTree>()with { ProcessorSlug = processorTree.ProcessorSlug, Kind = processorTree.Kind };

        // Act
        await sut.UpdateProcessor(updatedProcessorTree);

        // Assert
        var retrievedProcessor = await sut.GetProcessorByKey(processorTree.Key);
        retrievedProcessor.Should().BeEquivalentTo(updatedProcessorTree);
    }

    [Fact(DisplayName = "UpdateProcessor when processor does not exist should add the processor")]
    public async Task UpdateProcessor_WhenProcessorDoesNotExist_ShouldAddProcessor()
    {
        // Arrange
        var processorTree = Given.A<ProcessorTree>();
        var sut = new ProcessorRepository();

        // Act
        await sut.UpdateProcessor(processorTree);

        // Assert
        var retrievedProcessor = await sut.GetProcessorByKey(processorTree.Key);
        retrievedProcessor.Should().Be(processorTree);
    }

    [Fact(DisplayName = "Exists returns true when processor exists")]
    public async Task Exists_ReturnsTrue_WhenProcessorExists()
    {
        // Arrange
        var sut = new ProcessorRepository();
        var processorTree = Given.A<ProcessorTree>();
        await sut.AddProcessor(processorTree);

        // Act
        var exists = await sut.Exists(processorTree.Key);

        // Assert
        exists.Should().BeTrue();
    }

    [Fact(DisplayName = "Exists returns false when processor does not exist")]
    public async Task Exists_ReturnsFalse_WhenProcessorDoesNotExist()
    {
        // Arrange
        var sut = new ProcessorRepository();

        // Act
        var exists = await sut.Exists("nonExistentKey");

        // Assert
        exists.Should().BeFalse();
    }

    [Fact(DisplayName = "GetProcessorByKey returns processor when processor exists")]
    public async Task GetProcessorByKey_ReturnsProcessor_WhenProcessorExists()
    {
        // Arrange
        var sut = new ProcessorRepository();
        var processorTree = Given.A<ProcessorTree>();
        await sut.AddProcessor(processorTree);

        // Act
        var retrievedProcessor = await sut.GetProcessorByKey(processorTree.Key);

        // Assert
        retrievedProcessor.Should().BeEquivalentTo(processorTree);
    }

    [Fact(DisplayName = "GetProcessorByKey returns null when processor does not exist")]
    public async Task GetProcessorByKey_ReturnsNull_WhenProcessorDoesNotExist()
    {
        // Arrange
        var sut = new ProcessorRepository();

        // Act
        var retrievedProcessor = await sut.GetProcessorByKey("nonExistentKey");

        // Assert
        retrievedProcessor.Should().BeNull();
    }

    [Fact(DisplayName = "FilterProcessors returns processors that match the predicate")]
    public async Task FilterProcessors_ReturnsProcessors_ThatMatchPredicate()
    {
        // Arrange
        var sut = new ProcessorRepository();
        var processorTree1 = Given.A<ProcessorTree>();
        var processorTree2 = Given.A<ProcessorTree>();
        await sut.AddProcessor(processorTree1);
        await sut.AddProcessor(processorTree2);

        // Act
        var filteredProcessors = await sut.FilterProcessors(p => p.Key == processorTree1.Key);

        // Assert
        filteredProcessors.Should().ContainSingle().Which.Should().BeEquivalentTo(processorTree1);
    }

    [Fact(DisplayName = "FilterProcessors returns empty when no processors match the predicate")]
    public async Task FilterProcessors_ReturnsEmpty_WhenNoProcessorsMatchPredicate()
    {
        // Arrange
        var sut = new ProcessorRepository();
        var processorTree1 = Given.A<ProcessorTree>();
        var processorTree2 = Given.A<ProcessorTree>();
        await sut.AddProcessor(processorTree1);
        await sut.AddProcessor(processorTree2);

        // Act
        var filteredProcessors = await sut.FilterProcessors(p => p.Key == "nonExistentKey");

        // Assert
        filteredProcessors.Should().BeEmpty();
    }

    [Fact(DisplayName = "GetAllProcessors returns all processors")]
    public async Task GetAllProcessors_ReturnsAllProcessors()
    {
        // Arrange
        var sut = new ProcessorRepository();
        var processorTree1 = Given.A<ProcessorTree>();
        var processorTree2 = Given.A<ProcessorTree>();
        await sut.AddProcessor(processorTree1);
        await sut.AddProcessor(processorTree2);

        // Act
        var allProcessors = await sut.GetAllProcessors();

        // Assert
        allProcessors.Should().HaveCount(2).And.Contain(new[] { processorTree1, processorTree2 });
    }

    [Fact(DisplayName = "GetAllProcessors returns empty when no processors are added")]
    public async Task GetAllProcessors_ReturnsEmpty_WhenNoProcessorsAreAdded()
    {
        // Arrange
        var sut = new ProcessorRepository();

        // Act
        var allProcessors = await sut.GetAllProcessors();

        // Assert
        allProcessors.Should().BeEmpty();
    }
}
