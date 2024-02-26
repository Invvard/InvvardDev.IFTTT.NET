using InvvardDev.Ifttt.Toolkit;
using InvvardDev.Ifttt.Toolkit.Attributes;
using InvvardDev.Ifttt.Trigger.UpdatedNuget.Triggers;

namespace InvvardDev.Ifttt.Trigger.UpdatedNuget.Models;

[TriggerFields(NugetPackageUpdatedTrigger.TriggerSlug)]
public class WatchedNugetTriggerFields : TriggerFieldsBase
{   
    [DataField("nuget_package_name")]
    public string NugetPackageName { get; init; } = default!;

    [DataField("updated_version")]
    public string UpdatedVersion { get; init; } = default!;

    [DataField("updated_date")]
    public DateTime UpdatedDate { get; init; }
}