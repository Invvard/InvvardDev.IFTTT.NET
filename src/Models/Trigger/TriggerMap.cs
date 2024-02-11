namespace InvvardDev.Ifttt.Models.Trigger;

public class TriggerMap(string triggerSlug, Type triggerType)
{
    public string TriggerSlug { get; } = triggerSlug;
    
    public Type TriggerType { get; } = triggerType;
    
    public List<TriggerField> TriggerFields { get; } = [];
}