using InvvardDev.Ifttt.Trigger.Attributes;
using InvvardDev.Ifttt.Trigger.Models;
using InvvardDev.Ifttt.Trigger.UpdatedNuget.Triggers;

namespace InvvardDev.Ifttt.Trigger.UpdatedNuget.Models;

[TriggerFields(NugetPackageUpdatedTrigger.TriggerSlug)]
public class WatchedNugetTriggerFields : TriggerFieldsBase
{
    [TriggerField("nuget_package_name")]
    public string NugetPackageName { get; init; } = default!;

    [TriggerField("updated_version")]
    public string UpdatedVersion { get; init; } = default!;

    [TriggerField("updated_date")]
    public DateTime UpdatedDate { get; init; }
}