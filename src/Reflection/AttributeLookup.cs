using InvvardDev.Ifttt.Contracts;

namespace InvvardDev.Ifttt.Reflection;

internal abstract class AttributeLookup(IAssemblyAccessor assemblyAccessor) : IAttributeLookup
{
    public IEnumerable<Type> GetAnnotatedTypes()
    {
        var annotatedTypes = new List<Type>();
        var assemblies = assemblyAccessor.GetApplicationAssemblies();

        foreach (var assembly in assemblies)
        {
            annotatedTypes.AddRange(assembly.GetTypes()
                                            .Where(IsMatching)
                                            .ToList());
        }

        return annotatedTypes;
    }

    protected abstract bool IsMatching(Type type);
}