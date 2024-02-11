using FluentAssertions;
using InvvardDev.Ifttt.Toolkit;
using InvvardDev.Ifttt.Toolkit.Attributes;
using InvvardDev.Ifttt.Toolkit.Models;

namespace InvvardDev.Ifttt.Tests.Reflection;

public class TriggerFieldsMapperTests
{
    [Fact]
    public void CreateTriggerFieldsAutoMapper_ShouldMapTriggerFields()
    {
        // Arrange
        var triggerFields = new Dictionary<string, string>
                            {
                                { "nuget_package_name", "InvvardDev.Ifttt" },
                                { "updated_version", "1.0.0" },
                                { "updated_date", "2021-01-01" },
                                { "another_property", "Should go to metadata" }
                            };

        // Act
        var watchedNugetTriggerFields = triggerFields.To<WatchedNugetTriggerFields>();

        // Assert
        watchedNugetTriggerFields.NugetPackageName.Should().Be(triggerFields["nuget_package_name"]);
        watchedNugetTriggerFields.UpdatedVersion.Should().Be(triggerFields["updated_version"]);
        watchedNugetTriggerFields.UpdatedDate.Should().Be(DateTime.Parse(triggerFields["updated_date"]));
        watchedNugetTriggerFields.Metadata
                                 .Should()
                                 .ContainSingle()
                                 .And
                                 .ContainKey("another_property")
                                 .WhoseValue
                                 .Should()
                                 .Be(triggerFields["another_property"]);
    }
    
    private class WatchedNugetTriggerFields : TriggerFieldsBase
    {
        [TriggerField("nuget_package_name")]
        public string NugetPackageName { get; init; } = default!;

        [TriggerField("updated_version")]
        public string UpdatedVersion { get; init; } = default!;

        [TriggerField("updated_date")]
        public DateTime UpdatedDate { get; init; }
    }
}