using System.ComponentModel;
using System.Reflection;
using InvvardDev.Ifttt.Toolkit.Attributes;
using InvvardDev.Ifttt.Toolkit.Models;

namespace InvvardDev.Ifttt.Toolkit;

public static class TriggerFieldsMapper
{
    /// <summary>
    /// Extension method to map a dictionary to a trigger fields class.
    /// </summary>
    /// <remarks>Any unrecognized trigger field slug is stored in the <see cref="TriggerFieldsBase.Metadata"/> dictionary.</remarks>
    /// <param name="dictionary">The dictionary of data field slugs and its related data.</param>
    /// <typeparam name="T">The <see cref="TriggerFieldsBase"/> derived type to map to.</typeparam>
    /// <returns>A new <typeparamref name="T"/> instance.</returns>
    public static T To<T>(this Dictionary<string, string> dictionary)
        where T : TriggerFieldsBase, new()
    {
        var triggerFields = new T();

        foreach (var (key, value) in dictionary)
        {
            if (triggerFields.GetType()
                             .GetProperties()
                             .SingleOrDefault(p => p.GetCustomAttribute<DataFieldAttribute>()?.Slug == key)
                    is { CanWrite: true } property 
                && TypeDescriptor.GetConverter(property.PropertyType).ConvertFrom(dictionary[key]) is { } result)
            {
                property.SetValue(triggerFields, result);
            }
            else
            {
                triggerFields.Metadata.Add(key, value);
            }
        }

        return triggerFields;
    }
}