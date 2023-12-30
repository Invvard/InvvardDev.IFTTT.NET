using System.Reflection;
using InvvardDev.Ifttt.Service.Api.Trigger.Triggers;

namespace InvvardDev.Ifttt.Service.Api.Trigger.Attributes;

public static class TriggerAttributeLookup
{
    public static IEnumerable<Type> GetTriggers(Assembly assembly)
        => assembly.GetTypes()
                   .Where(IsTrigger);

    private static bool IsTrigger(Type type)
        => type is { IsClass: true, IsAbstract: false }
           && typeof(object) != type
           && typeof(ITrigger).IsAssignableFrom(type)
           && type.GetCustomAttributes(typeof(TriggerAttribute), true).Length > 0;
}