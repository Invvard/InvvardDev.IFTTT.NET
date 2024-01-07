using InvvardDev.Ifttt.Service.Api.Trigger.Attributes;
using InvvardDev.Ifttt.Trigger.UpdatedNuget.Triggers;

namespace InvvardDev.Ifttt.Trigger.UpdatedNuget.Models;

[TriggerFields(NugetPackageUpdatedTrigger.TriggerSlug)]
public class WatchedNugetModel
{
    [TriggerField("nuget_package_name")]
    public string NugetPackageName { get; set; }
}