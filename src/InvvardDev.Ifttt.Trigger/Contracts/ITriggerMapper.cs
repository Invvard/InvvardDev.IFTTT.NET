namespace InvvardDev.Ifttt.Trigger.Contracts;

public interface ITriggerMapper
{
    ITriggerMapper MapTriggerProcessors();
    
    ITriggerMapper MapTriggerFields();
}