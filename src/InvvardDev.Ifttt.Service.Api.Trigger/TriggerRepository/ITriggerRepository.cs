using InvvardDev.Ifttt.Service.Api.Trigger.Triggers;

namespace InvvardDev.Ifttt.Service.Api.Trigger.TriggerRepository;

public interface ITriggerRepository
{
        void AddTriggerTypes(IEnumerable<Type> triggerTypes);
        ITrigger GetTriggerProcessorInstance(string triggerSlug);
}