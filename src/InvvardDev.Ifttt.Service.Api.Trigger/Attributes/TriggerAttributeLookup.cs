using System.Reflection;
using InvvardDev.Ifttt.Service.Api.Trigger.Triggers;
using Microsoft.Extensions.DependencyModel;

namespace InvvardDev.Ifttt.Service.Api.Trigger.Attributes;

internal static class TriggerAttributeLookup
{
    public static IEnumerable<Type> GetTriggers()
    {
        var triggerTypes = new List<Type>();
        var assemblies = GetApplicationAssemblies();

        // Iterate through each assembly and find types with TriggerAttribute
        foreach (var assembly in assemblies)
        {
            triggerTypes.AddRange(assembly.GetTypes()
                                          .Where(IsTrigger)
                                          .ToList());
        }

        return triggerTypes;
    }

    private static Assembly[] GetApplicationAssemblies()
    {
        // Get the entry assembly
        var entryAssembly = Assembly.GetEntryAssembly();

        // Get the list of assemblies in the application
        if (DependencyContext.Default != null)
        {
            var applicationAssemblies = DependencyContext.Default
                                                         .GetDefaultAssemblyNames()
                                                         .Where(assembly => !IsFrameworkAssembly(assembly))
                                                         .Select(Assembly.Load);

            // Optionally, include the entry assembly
            if (entryAssembly != null && !IsFrameworkAssembly(entryAssembly.GetName()))
            {
                applicationAssemblies = applicationAssemblies.Append(entryAssembly);
            }

            return applicationAssemblies.ToArray();
        }

        return Array.Empty<Assembly>();
    }

    private static bool IsFrameworkAssembly(AssemblyName assemblyName)
        => assemblyName.FullName.StartsWith("System.") || assemblyName.FullName.StartsWith("Microsoft.");

    private static bool IsTrigger(Type type)
        => type is { IsClass: true, IsAbstract: false }
           && typeof(object) != type
           && typeof(ITrigger).IsAssignableFrom(type)
           && type.GetCustomAttributes(typeof(TriggerAttribute), true).Length > 0;
}