using InvvardDev.Ifttt.Samples.Trigger.Data;
using InvvardDev.Ifttt.Samples.Trigger.Data.Models;
using InvvardDev.Ifttt.Samples.Trigger.Models;
using InvvardDev.Ifttt.Toolkit;
using InvvardDev.Ifttt.Toolkit.Attributes;

namespace InvvardDev.Ifttt.Samples.Trigger.Triggers;

[Trigger(TriggerSlug)]
public class NugetPackageUpdatedTrigger(IDataRepository<NugetPackageVersion> repository) : ITrigger
{
    internal const string TriggerSlug = "nuget_package_updated";

    public async Task<TriggerResponse> ExecuteAsync(TriggerRequest triggerRequest, CancellationToken cancellationToken = default)
    {
        var watchedNuget = triggerRequest.TriggerFields.To<WatchedNugetTriggerFields>();

        if (await repository.GetByName(watchedNuget.Name, cancellationToken) is not { } nugetPackageVersion)
        {
            return default!;
        }

        return (WatchedNugetTriggerResponse)nugetPackageVersion;
    }
}
