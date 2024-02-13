using InvvardDev.Ifttt.TestFactories.Extensions;
using InvvardDev.Ifttt.TestFactories.Shared;
using InvvardDev.Ifttt.Toolkit.Attributes;

namespace InvvardDev.Ifttt.TestFactories.Triggers;

public static class TriggerFieldsClassFactory
{
    public static Type MissingTriggerFieldsAttribute(string? typeName = null, string? propertyName = null)
    {
        propertyName = propertyName.NewName();
        return DefineType.Called(typeName.NewName())
                         .WithProperty<string>(propertyName.NewName())
                         .WithCustomAttribute<DataFieldAttribute>(propertyName, $"{propertyName}_slug")
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

    public static Type MatchingTriggerFieldsModel(string? typeName = null, string? propertyName = null, string? triggerSlug = null, string? triggerFieldSlug = null)
    {
        typeName = typeName.NewName();
        propertyName = propertyName.NewName();
        triggerFieldSlug ??= $"{propertyName}_slug";
        return DefineType.Called(typeName)
                         .WithCustomAttribute<TriggerFieldsAttribute>(typeName, triggerSlug.NewName())
                         .WithProperty<string>(propertyName)
                         .WithCustomAttribute<DataFieldAttribute>(propertyName, triggerFieldSlug)
                         .Build();
    }
}