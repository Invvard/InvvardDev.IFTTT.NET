namespace InvvardDev.Ifttt.Trigger.Attributes;

internal class TriggerFieldsAttributeLookup : AttributeLookup
{
    protected override bool IsMatching(Type type)
        => type is { IsClass: true, IsAbstract: false }
           && typeof(object) != type
           && type.GetCustomAttributes(typeof(TriggerFieldsAttribute), true).Length > 0
           && type.GetProperties()
                  .Any(p => p.GetCustomAttributes(typeof(TriggerFieldAttribute), true).Length > 0);
}