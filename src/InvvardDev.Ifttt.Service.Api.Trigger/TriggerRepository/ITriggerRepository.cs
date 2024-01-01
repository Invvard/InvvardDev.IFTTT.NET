namespace InvvardDev.Ifttt.Service.Api.Trigger.TriggerRepository;

internal interface ITriggerRepository
{
        IReadOnlyCollection<Type> GetTriggerTypes();
        void AddTriggerTypes(IList<Type> triggerTypes);
}