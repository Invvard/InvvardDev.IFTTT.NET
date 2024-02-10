using System.Reflection;
using InvvardDev.Ifttt.Shared.Contracts;
using Microsoft.Extensions.DependencyModel;

namespace InvvardDev.Ifttt.Shared.Services;

public class AssemblyAccessor : IAssemblyAccessor
{
    private List<Assembly>? applicationAssemblies;

    private readonly List<string> assemblyNamesToFilterOut =
    [
        "Microsoft.",
        "mscorlib",
        "netstandard",
        "Newtonsoft.Json",
        "System",
        "System.",
        "WindowsBase"
    ];

    public IEnumerable<Assembly> GetApplicationAssemblies()
    {
        if (applicationAssemblies is { } list) return list.ToArray();

        if (DependencyContext.Default is not { } defaultDependency) return Array.Empty<Assembly>();

        applicationAssemblies = new List<Assembly>(defaultDependency
                                                   .GetDefaultAssemblyNames()
                                                   .Where(assembly => !IsFrameworkAssembly(assembly.Name))
                                                   .Select(Assembly.Load));

        if (Assembly.GetEntryAssembly() is not { } entryAssembly) return applicationAssemblies.ToArray();

        if (!IsFrameworkAssembly(entryAssembly.GetName().Name)
            && applicationAssemblies.Any(assembly => assembly.GetName() == entryAssembly.GetName()))
        {
            applicationAssemblies.Add(entryAssembly);
        }

        return applicationAssemblies.ToArray();
    }

    public TAttribute? GetAttribute<TAttribute>(Type classType)
        where TAttribute : Attribute
        => classType.GetCustomAttribute<TAttribute>();

    public void FilterOutAssemblies(params string[] assemblyNames)
    {
        assemblyNamesToFilterOut.AddRange(assemblyNames);
    }

    private bool IsFrameworkAssembly(string? assemblyName)
        => !string.IsNullOrWhiteSpace(assemblyName) && assemblyNamesToFilterOut.Any(a => a.StartsWith(assemblyName));
}