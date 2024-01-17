using InvvardDev.Ifttt.Trigger.Attributes;
using InvvardDev.Ifttt.Trigger.Contracts;

namespace InvvardDev.Ifttt.Trigger.Tests.Factories;

internal static class TriggerClassFactory
{
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
}