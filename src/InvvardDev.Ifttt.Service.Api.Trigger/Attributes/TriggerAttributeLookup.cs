namespace InvvardDev.Ifttt.Service.Api.Trigger.Attributes;

internal class TriggerAttributeLookup : AttributeLookup
{
    protected override bool IsMatching(Type type)
        => type is { IsClass: true, IsAbstract: false }
           && typeof(object) != type
           && typeof(ITrigger).IsAssignableFrom(type)
           && type.GetCustomAttributes(typeof(TriggerAttribute), true).Length > 0;
}