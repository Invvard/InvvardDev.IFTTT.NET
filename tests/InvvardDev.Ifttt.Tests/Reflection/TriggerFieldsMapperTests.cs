﻿using System.Globalization;
using InvvardDev.Ifttt.Toolkit;
using InvvardDev.Ifttt.Toolkit.Attributes;

namespace InvvardDev.Ifttt.Tests.Reflection;

public class TriggerFieldsMapperTests
{
    [Fact(DisplayName = "CreateTriggerFieldsAutoMapper should map trigger fields")]
    public void CreateTriggerFieldsAutoMapper_ShouldMapTriggerFields()
    {
        // Arrange
        var triggerFields = new Dictionary<string, string>
                            {
                                { "nuget_package_name", "InvvardDev.Ifttt" },
                                { "updated_version", "1.0.0" },
                                { "updated_date", "2021-01-01" },
                                { "another_property", "Should be ignored" }
                            };

        // Act
        var watchedNugetTriggerFields = triggerFields.To<WatchedNugetTriggerFields>();

        // Assert
        watchedNugetTriggerFields.NugetPackageName.Should().Be(triggerFields["nuget_package_name"]);
        watchedNugetTriggerFields.UpdatedVersion.Should().Be(triggerFields["updated_version"]);
        watchedNugetTriggerFields.UpdatedDate.Should().Be(DateTime.Parse(triggerFields["updated_date"], CultureInfo.InvariantCulture));
    }
    
    private class WatchedNugetTriggerFields
    {
        [DataField("nuget_package_name")]
        public string NugetPackageName { get; init; } = default!;

        [DataField("updated_version")]
        public string UpdatedVersion { get; init; } = default!;

        [DataField("updated_date")]
        public DateTime UpdatedDate { get; init; }
    }
}
