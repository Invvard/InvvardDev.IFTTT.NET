using InvvardDev.Ifttt.Toolkit;
using InvvardDev.Ifttt.Toolkit.Attributes;

namespace InvvardDev.Ifttt.TestFactories.Triggers;

[ExcludeFromCodeCoverage]
public static class TriggerClassFactory
{
    public static Type MissingITriggerInterface(string? typeName = null, string? triggerSlug = null)
    {
        typeName = typeName.NewName();
        return DefineType.Called(typeName)
                         .WithCustomAttribute<TriggerAttribute>(typeName, triggerSlug.NewName())
                         .Build();
    }

    public static Type MissingTriggerAttribute(string? typeName = null)
        => DefineType.Called(typeName.NewName())
                     .ImplementInterface<ITrigger>()
                     .Build();

    public static Type MatchingClass(string? typeName = null, string? triggerSlug = null)
    {
        typeName = typeName.NewName();
        return DefineType.Called(typeName)
                         .WithCustomAttribute<TriggerAttribute>(typeName, triggerSlug.NewName())
                         .ImplementInterface<ITrigger>()
                         .Build();
    }

    public static (Type type, List<string> dataFieldSlugs) MatchingClassWithDataFields(string? typeName = null, string? triggerSlug = null, int dataFieldCount = 1)
    {
        typeName = typeName.NewName();
        var type = DefineType.Called(typeName)
                             .WithCustomAttribute<TriggerAttribute>(typeName, triggerSlug.NewName())
                             .ImplementInterface<ITrigger>();

        var dataFieldSlugs = new List<string>();
        for (var i = 0; i < dataFieldCount; i++)
        {
            var propertySlug = $"property_{i}";
            type.WithProperty<string>($"Property{i}")
                .WithCustomAttribute<DataFieldAttribute>($"Property{i}", propertySlug);
            dataFieldSlugs.Add(propertySlug);
        }

        return (type.Build(), dataFieldSlugs);
    }
}
