using System.Threading;
using System.Threading.Tasks;
using InvvardDev.Ifttt.Toolkit;
using InvvardDev.Ifttt.Toolkit.Attributes;
using InvvardDev.Ifttt.Toolkit.Contracts;
using InvvardDev.Ifttt.Toolkit.Models;
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