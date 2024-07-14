using System.ComponentModel;
using System.Reflection;
using InvvardDev.Ifttt.Toolkit.Attributes;

namespace InvvardDev.Ifttt.Toolkit;

public static class TriggerFieldsMapper
{
    /// <summary>
    /// Extension method to map a dictionary to a trigger fields class.
    /// </summary>
    /// <remarks>Any unmatched trigger field slug is ignored.</remarks>
    /// <param name="dictionary">The dictionary of data field slugs and its related data.</param>
    /// <typeparam name="T">The targeted <typeparamref name="T"/> type to map to.</typeparam>
    /// <returns>A new <typeparamref name="T"/> instance.</returns>
    public static T To<T>(this Dictionary<string, string> dictionary)
        where T : class, new()
    {
        var triggerFields = new T();

        foreach (var (key, value) in dictionary)
        {
            if (triggerFields.GetType().GetProperties().SingleOrDefault(p => p.GetCustomAttribute<DataFieldAttribute>()?.Slug == key) is { CanWrite: true } property
                && TypeDescriptor.GetConverter(property.PropertyType).ConvertFrom(value) is { } result)
            {
                property.SetValue(triggerFields, result);
            }
        }

        return triggerFields;
    }
}
