using InvvardDev.Ifttt.Service.Api.Trigger.Attributes;
using InvvardDev.Ifttt.Service.Api.Trigger.Contracts;
using InvvardDev.Ifttt.Service.Api.Trigger.Models;

namespace InvvardDev.Ifttt.Trigger.UpdatedNuget.Triggers;

[Trigger(TriggerSlug)]
public class NugetPackageUpdatedTrigger : ITrigger
{
    internal const string TriggerSlug = "nuget_package_updated";

    public Task ExecuteAsync(TriggerRequestBase triggerRequest, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}