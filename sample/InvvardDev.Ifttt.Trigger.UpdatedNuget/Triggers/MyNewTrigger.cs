using InvvardDev.Ifttt.Service.Api.Trigger.Attributes;
using InvvardDev.Ifttt.Service.Api.Trigger.Triggers;

namespace InvvardDev.Ifttt.Trigger.UpdatedNuget.Triggers;

[Trigger("my_new_trigger")]
public class MyNewTrigger : ITrigger
{
    public Task ProcessAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}