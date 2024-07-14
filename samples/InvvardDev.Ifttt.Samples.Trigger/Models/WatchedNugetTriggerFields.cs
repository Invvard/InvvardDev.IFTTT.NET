using InvvardDev.Ifttt.Samples.Trigger.Triggers;
using InvvardDev.Ifttt.Toolkit.Attributes;

namespace InvvardDev.Ifttt.Samples.Trigger.Models;

[TriggerFields(NugetPackageUpdatedTrigger.TriggerSlug)]
public class WatchedNugetTriggerFields
{   
    [DataField("nuget_package_to_watch")]
    public string Name { get; init; } = default!;
}
