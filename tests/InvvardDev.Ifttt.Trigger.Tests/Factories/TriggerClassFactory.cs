using Bogus;
using InvvardDev.Ifttt.Trigger.Attributes;
using InvvardDev.Ifttt.Trigger.Contracts;

namespace InvvardDev.Ifttt.Trigger.Tests.Factories;

public static class TriggerClassFactory
{
    private static readonly Faker Faker = new();
    private static string NewName(this string? name, int count = 2)
        => name ?? string.Join('_', Faker.Random.WordsArray(count));
    
    public static Type MissingITriggerInterface(string? typeName = null, string? expectedSlug = null)
        => CreateType.Called(typeName.NewName())
                     .WithAttribute<TriggerAttribute>(expectedSlug.NewName())
                     .Build();

    public static Type MissingTriggerAttribute(string? typeName = null)
        => CreateType.Called(typeName.NewName())
                     .ThatImplements<ITrigger>()
                     .Build();

    public static Type MatchingClass(string? typeName = null, string? expectedSlug = null)
        => CreateType.Called(typeName.NewName())
                     .WithAttribute<TriggerAttribute>(expectedSlug.NewName())
                     .ThatImplements<ITrigger>()
                     .Build();

    public static Type MissingTriggerFieldsAttribute(string? typeName = null, string? propertyName = null)
    {
        propertyName = propertyName.NewName();
        return CreateType.Called(typeName.NewName())
                         .WithProperty<string>(propertyName)
                         .WithPropertyAttribute<TriggerFieldAttribute>(propertyName, $"{propertyName}_slug");
    }
}