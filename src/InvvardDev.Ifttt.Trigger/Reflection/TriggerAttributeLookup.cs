using InvvardDev.Ifttt.Shared.Contracts;
using InvvardDev.Ifttt.Trigger.Contracts;
using InvvardDev.Ifttt.Trigger.Models.Attributes;
using InvvardDev.Ifttt.Trigger.Models.Contracts;

namespace InvvardDev.Ifttt.Trigger.Reflection;

internal class TriggerAttributeLookup(IAssemblyAccessor assemblyAccessor)
    : AttributeLookup(assemblyAccessor)
{
    protected override bool IsMatching(Type type)
        => type is { IsClass: true, IsAbstract: false }
           && typeof(object) != type
           && typeof(ITrigger).IsAssignableFrom(type)
           && type.GetCustomAttributes(typeof(TriggerAttribute), true).Length > 0;
}