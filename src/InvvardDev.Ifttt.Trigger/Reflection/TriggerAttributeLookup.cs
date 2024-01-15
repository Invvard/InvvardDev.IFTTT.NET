using InvvardDev.Ifttt.Trigger.Attributes;
using InvvardDev.Ifttt.Trigger.Contracts;

namespace InvvardDev.Ifttt.Trigger.Reflection;

internal class TriggerAttributeLookup : AttributeLookup
{
    protected override bool IsMatching(Type type)
        => type is { IsClass: true, IsAbstract: false }
           && typeof(object) != type
           && typeof(ITrigger).IsAssignableFrom(type)
           && type.GetCustomAttributes(typeof(TriggerAttribute), true).Length > 0;
}