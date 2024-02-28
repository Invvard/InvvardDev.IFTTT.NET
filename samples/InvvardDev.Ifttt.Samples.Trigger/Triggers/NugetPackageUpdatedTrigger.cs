using System.Threading;
using System.Threading.Tasks;
using InvvardDev.Ifttt.Samples.Trigger.Models;
using InvvardDev.Ifttt.Toolkit;
using InvvardDev.Ifttt.Toolkit.Attributes;

namespace InvvardDev.Ifttt.Samples.Trigger.Triggers;

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