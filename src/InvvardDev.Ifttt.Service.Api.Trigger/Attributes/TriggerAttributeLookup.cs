using System.Reflection;
using InvvardDev.Ifttt.Service.Api.Trigger.Triggers;
using Microsoft.Extensions.DependencyModel;

namespace InvvardDev.Ifttt.Service.Api.Trigger.Attributes;

internal static class TriggerAttributeLookup
{
    public static IEnumerable<Type> GetTriggerTypes()
    {
        var triggerTypes = new List<Type>();
        var assemblies = GetApplicationAssemblies();

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
        var entryAssembly = Assembly.GetEntryAssembly();
        
        if (DependencyContext.Default != null)
        {
            var applicationAssemblies = DependencyContext.Default
                                                         .GetDefaultAssemblyNames()
                                                         .Where(assembly => !IsFrameworkAssembly(assembly))
                                                         .Select(Assembly.Load).ToList();

            if (entryAssembly != null
                && !IsFrameworkAssembly(entryAssembly.GetName())
                && applicationAssemblies.Any(assembly => assembly.GetName() == entryAssembly.GetName()))
            {
                applicationAssemblies.Add(entryAssembly);
            }

            return applicationAssemblies.ToArray();
        }

        return Array.Empty<Assembly>();
    }

    private static bool IsFrameworkAssembly(AssemblyName assemblyName)
        => assemblyName.FullName.StartsWith("System.")
           || assemblyName.FullName.Equals("System")
           || assemblyName.FullName.Equals("mscorlib")
           || assemblyName.FullName.Equals("netstandard")
           || assemblyName.FullName.Equals("WindowsBase")
           || assemblyName.FullName.StartsWith("Microsoft.");

    private static bool IsTrigger(Type type)
        => type is { IsClass: true, IsAbstract: false }
           && typeof(object) != type
           && typeof(ITrigger).IsAssignableFrom(type)
           && type.GetCustomAttributes(typeof(TriggerAttribute), true).Length > 0;
}