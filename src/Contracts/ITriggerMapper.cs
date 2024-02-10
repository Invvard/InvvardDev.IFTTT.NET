namespace InvvardDev.Ifttt.Contracts;

public interface ITriggerMapper
{
    ITriggerMapper MapTriggerProcessors();

    ITriggerMapper MapTriggerFields();
}