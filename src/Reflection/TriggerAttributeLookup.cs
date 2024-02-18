using InvvardDev.Ifttt.Contracts;
using InvvardDev.Ifttt.Toolkit.Attributes;
using InvvardDev.Ifttt.Toolkit.Contracts;

namespace InvvardDev.Ifttt.Reflection;

internal class TriggerAttributeLookup(IAssemblyAccessor assemblyAccessor)
    : AttributeLookup(assemblyAccessor)
{
    protected override bool IsMatching(Type type)
        => type is { IsClass: true, IsAbstract: false }
           && typeof(object) != type
           && typeof(ITrigger).IsAssignableFrom(type)
           && type.GetCustomAttributes(typeof(TriggerAttribute), true).Length > 0;
}