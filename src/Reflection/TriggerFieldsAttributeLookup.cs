using InvvardDev.Ifttt.Contracts;
using InvvardDev.Ifttt.Toolkit.Attributes;

namespace InvvardDev.Ifttt.Reflection;

internal class TriggerFieldsAttributeLookup(IAssemblyAccessor assemblyAccessor)
    : AttributeLookup(assemblyAccessor)
{
    protected override bool IsMatching(Type type)
        => type is { IsClass: true, IsAbstract: false }
           && typeof(object) != type
           && type.GetCustomAttributes(typeof(TriggerFieldsAttribute), true).Length > 0
           && type.GetProperties().Any(p => p.GetCustomAttributes(typeof(TriggerFieldAttribute), true).Length > 0);
}