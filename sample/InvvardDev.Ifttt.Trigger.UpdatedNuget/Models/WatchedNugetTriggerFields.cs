using InvvardDev.Ifttt.Service.Api.Trigger.Attributes;
using InvvardDev.Ifttt.Service.Api.Trigger.Models;
using InvvardDev.Ifttt.Trigger.UpdatedNuget.Triggers;

namespace InvvardDev.Ifttt.Trigger.UpdatedNuget.Models;

[TriggerFields(NugetPackageUpdatedTrigger.TriggerSlug)]
public class WatchedNugetTriggerFields : TriggerFieldsBase
{
    [TriggerField("nuget_package_name")]
    public string NugetPackageName { get; init; }

    [TriggerField("updated_version")]
    public string UpdatedVersion { get; init; }

    [TriggerField("updated_date")]
    public DateTime UpdatedDate { get; init; }
}