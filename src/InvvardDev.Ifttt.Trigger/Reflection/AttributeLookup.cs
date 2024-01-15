using System.Reflection;
using InvvardDev.Ifttt.Trigger.Contracts;
using Microsoft.Extensions.DependencyModel;

namespace InvvardDev.Ifttt.Trigger.Reflection;

internal abstract class AttributeLookup : IAttributeLookup
{
    public IEnumerable<Type> GetAnnotatedTypes()
    {
        var annotatedTypes = new List<Type>();
        var assemblies = GetApplicationAssemblies();

        foreach (var assembly in assemblies)
        {
            annotatedTypes.AddRange(assembly.GetTypes()
                                            .Where(IsMatching)
                                            .ToList());
        }

        return annotatedTypes;
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

    protected abstract bool IsMatching(Type type);
}