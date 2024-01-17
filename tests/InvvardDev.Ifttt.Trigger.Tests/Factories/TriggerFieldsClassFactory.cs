using InvvardDev.Ifttt.Trigger.Attributes;

namespace InvvardDev.Ifttt.Trigger.Tests.Factories;

internal static class TriggerFieldsClassFactory
{
    public static Type MissingTriggerFieldsAttribute(string? typeName = null, string? propertyName = null)
    {
        propertyName = propertyName.NewName();
        return CreateType.Called(typeName.NewName())
                         .WithPropertyAttribute<string, TriggerFieldAttribute>(propertyName, $"{propertyName}_slug")
                         .Build();
    }

    public static Type MissingTriggerFieldProperty(string? typeName = null, string? expectedSlug = null, string? propertyName = null)
        => CreateType.Called(typeName.NewName())
                     .WithAttribute<TriggerFieldsAttribute>(expectedSlug.NewName())
                     .WithProperty<string>(propertyName.NewName())
                     .Build();

    public static Type MatchingTriggerFieldsClass(string? typeName = null, string? expectedSlug = null, string? propertyName = null)
    {
        propertyName = propertyName.NewName();
        return CreateType.Called(typeName.NewName())
                         .WithAttribute<TriggerFieldsAttribute>(expectedSlug.NewName())
                         .WithPropertyAttribute<string, TriggerFieldAttribute>(propertyName, $"{propertyName}_slug")
                         .Build();
    }
}