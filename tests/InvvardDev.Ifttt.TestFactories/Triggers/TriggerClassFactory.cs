﻿using InvvardDev.Ifttt.TestFactories.Common;
using InvvardDev.Ifttt.TestFactories.Extensions;
using InvvardDev.Ifttt.Trigger.Attributes;
using InvvardDev.Ifttt.Trigger.Contracts;

namespace InvvardDev.Ifttt.TestFactories.Triggers;

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
}