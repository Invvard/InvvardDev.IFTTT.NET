using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using InvvardDev.Ifttt.Contracts;
using Microsoft.Extensions.DependencyModel;

namespace InvvardDev.Ifttt.Reflection;

[ExcludeFromCodeCoverage(Justification = "It'd be equivalent to testing Assembly reflection")]
internal class AssemblyAccessor : IAssemblyAccessor
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
        if (applicationAssemblies is { } list) return list;

        if (DependencyContext.Default is not { } defaultDependency) return new List<Assembly>();

        applicationAssemblies =
        [
            .. defaultDependency
               .GetDefaultAssemblyNames()
               .Where(assembly => !IsFrameworkAssembly(assembly.Name))
               .Select(Assembly.Load)
        ];

        if (Assembly.GetEntryAssembly() is not { } entryAssembly) return applicationAssemblies;

        if (!IsFrameworkAssembly(entryAssembly.GetName().Name)
            && applicationAssemblies.Exists(assembly => assembly.GetName() == entryAssembly.GetName()))
        {
            applicationAssemblies.Add(entryAssembly);
        }

        return applicationAssemblies;
    }

    public TAttribute? GetAttribute<TAttribute>(Type classType)
        where TAttribute : Attribute
        => classType.GetCustomAttribute<TAttribute>();

    public void FilterOutAssemblies(params string[] assemblyNames)
        => assemblyNamesToFilterOut.AddRange(assemblyNames);

    private bool IsFrameworkAssembly(string? assemblyName)
        => !string.IsNullOrWhiteSpace(assemblyName) && assemblyNamesToFilterOut.Exists(a => a.StartsWith(assemblyName));
}
