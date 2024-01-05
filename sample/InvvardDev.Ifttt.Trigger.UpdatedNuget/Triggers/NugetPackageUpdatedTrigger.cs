using InvvardDev.Ifttt.Service.Api.Trigger.Attributes;
using InvvardDev.Ifttt.Service.Api.Trigger.Contracts;
using InvvardDev.Ifttt.Service.Api.Trigger.Models;

namespace InvvardDev.Ifttt.Trigger.UpdatedNuget.Triggers;

[Trigger("nuget_package_updated")]
public class NugetPackageUpdatedTrigger : ITrigger
{
    public Task ExecuteAsync(TriggerRequestBase triggerRequest, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}