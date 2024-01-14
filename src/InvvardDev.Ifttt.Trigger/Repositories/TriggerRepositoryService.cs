using System.Reflection;
using InvvardDev.Ifttt.Trigger.Attributes;
using InvvardDev.Ifttt.Trigger.Contracts;
using InvvardDev.Ifttt.Trigger.Models;

namespace InvvardDev.Ifttt.Trigger.Repositories;

internal class TriggerRepositoryService(
    [FromKeyedServices(nameof(TriggerAttributeLookup))] IAttributeLookup triggerAttributeLookup,
    [FromKeyedServices(nameof(TriggerFieldsAttributeLookup))] IAttributeLookup triggerFieldsAttributeLookup
) : ITriggerRepository
{
    private const string ValidatorSuffix = "Validator";
    private readonly Dictionary<string, TriggerDataType> triggers = new();

    public ITriggerRepository MapTriggerTypes()
    {
        var types = triggerAttributeLookup.GetAnnotatedTypes();
        foreach (var triggerType in types)
        {
            if (triggerType.GetCustomAttribute<TriggerAttribute>() is { } triggerAttribute
                && !triggers.ContainsKey(triggerAttribute.Slug))
            {
                triggers.Add(triggerAttribute.Slug, new TriggerDataType(triggerAttribute.Slug, triggerType));
            }
        }

        return this;
    }

    public ITriggerRepository MapTriggerFields()
    {
        var types = triggerFieldsAttributeLookup.GetAnnotatedTypes();
        foreach (var triggerFieldsType in types)
        {
            if (triggerFieldsType.GetCustomAttribute<TriggerFieldsAttribute>() is { } triggerFieldsAttribute
                && triggers.TryGetValue(triggerFieldsAttribute.Slug, out var triggerDataType))
            {
                foreach (var (propertyName, method) in MapValidators(triggerFieldsType))
                {
                    triggerDataType.TriggerFieldValidator.Add(propertyName, method);
                }

                triggers[triggerFieldsAttribute.Slug] = triggerDataType with
                                                        {
                                                            TriggerFieldsType = triggerFieldsType
                                                        };
            }
        }

        return this;
    }

    private IEnumerable<(string propertySlug, MethodInfo method)> MapValidators(Type triggerFieldsType)
    {
        foreach (var method in triggerFieldsType.GetMethods(BindingFlags.Public
                                                            | BindingFlags.Instance
                                                            | BindingFlags.DeclaredOnly))
        {
            if (IsValidator(method, ValidatorSuffix)
                && GetPropertySlug(triggerFieldsType, method.Name[..^ValidatorSuffix.Length]) is { } propertySlug)
            {
                yield return (propertySlug, method);
            }
        }
    }

    private static string? GetPropertySlug(Type? classInfo, string propertyName)
        => classInfo?.GetProperty(propertyName) is { } propertyInfo
           && propertyInfo.GetCustomAttribute<TriggerFieldAttribute>() is { } triggerFieldAttribute
            ? triggerFieldAttribute.Slug
            : throw new ArgumentNullException(nameof(triggerFieldAttribute));

    private static bool IsValidator(MethodInfo method, string validatorSuffix)
        => method.Name.EndsWith(validatorSuffix)
           && method is { IsPublic: true }
           && method.ReturnType == typeof(bool)
           && method.GetParameters().Length == 1
           && method.GetParameters()[0].ParameterType == typeof(string)
           && method.DeclaringType?.GetProperty(method.Name[..^validatorSuffix.Length]) is not null;

    public ITrigger GetTriggerProcessorInstance(string triggerSlug)
    {
        if (!triggers.TryGetValue(triggerSlug, out var triggerDataType)
            || Activator.CreateInstance(triggerDataType.TriggerType) is not ITrigger triggerInstance)
        {
            throw new InvalidOperationException();
        }

        return triggerInstance;
    }

    public Type? GetTriggerFieldsType(string triggerSlug)
    {
        triggers.TryGetValue(triggerSlug, out var triggerDataType);

        return triggerDataType?.TriggerFieldsType;
    }
}