using System.Reflection;

namespace InvvardDev.Ifttt.Trigger.Contracts;

public interface IAssemblyAccessor
{
    IEnumerable<Assembly> GetApplicationAssemblies();
    
    TAttribute? GetAttribute<TAttribute>(Type classType) where TAttribute : Attribute;
    
    void FilterOutAssemblies(params string [] assemblyNames);
}