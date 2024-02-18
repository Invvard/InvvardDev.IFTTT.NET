using System.Threading;
using System.Threading.Tasks;
using InvvardDev.Ifttt.Trigger.Attributes;
using InvvardDev.Ifttt.Trigger.Contracts;
using InvvardDev.Ifttt.Trigger.Models;
using InvvardDev.Ifttt.Trigger.UpdatedNuget.Models;

namespace InvvardDev.Ifttt.Trigger.UpdatedNuget.Triggers;

[Trigger(TriggerSlug)]
public class NugetPackageUpdatedTrigger : ITrigger
{
    internal const string TriggerSlug = "nuget_package_updated";

    public Task ExecuteAsync(TriggerRequest triggerRequest, CancellationToken cancellationToken = default)
    {
        var triggerFields = triggerRequest.TriggerFields.To<WatchedNugetTriggerFields>();
        return Task.CompletedTask;
    }
}