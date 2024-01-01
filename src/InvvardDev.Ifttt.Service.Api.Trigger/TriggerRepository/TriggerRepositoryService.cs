namespace InvvardDev.Ifttt.Service.Api.Trigger.TriggerRepository;

internal class TriggerRepositoryService : ITriggerRepository
{
    private readonly List<Type> triggerTypes = new();
    
    public IReadOnlyCollection<Type> GetTriggerTypes()
        => triggerTypes;

    public void AddTriggerTypes(IList<Type> types)
        => triggerTypes.AddRange(types);
}