using InvvardDev.Ifttt.TestFactories.Extensions;
using InvvardDev.Ifttt.TestFactories.Shared;
using InvvardDev.Ifttt.Trigger.Models.Attributes;

namespace InvvardDev.Ifttt.TestFactories.Triggers;

public static class TriggerFieldsClassFactory
{
    public static Type MissingTriggerFieldsAttribute(string? typeName = null, string? propertyName = null)
    {
        propertyName = propertyName.NewName();
        return DefineType.Called(typeName.NewName())
                         .WithProperty<string>(propertyName.NewName())
                         .WithCustomAttribute<TriggerFieldAttribute>(propertyName, $"{propertyName}_slug")
                         .Build();
    }

    public static Type MissingTriggerFieldProperty(string? typeName = null, string? propertyName = null, string? triggerSlug = null)
    {
        typeName = typeName.NewName();
        return DefineType.Called(typeName)
                         .WithCustomAttribute<TriggerFieldsAttribute>(typeName, triggerSlug.NewName())
                         .WithProperty<string>(propertyName.NewName())
                         .Build();
    }

    public static Type MatchingTriggerFieldsClass(string? typeName = null, string? propertyName = null, string? triggerSlug = null, string? triggerFieldSlug = null)
    {
        typeName = typeName.NewName();
        propertyName = propertyName.NewName();
        return DefineType.Called(typeName)
                         .WithCustomAttribute<TriggerFieldsAttribute>(typeName, triggerSlug.NewName())
                         .WithProperty<string>(propertyName)
                         .WithCustomAttribute<TriggerFieldAttribute>(propertyName, $"{propertyName}_slug")
                         .Build();
    }
}