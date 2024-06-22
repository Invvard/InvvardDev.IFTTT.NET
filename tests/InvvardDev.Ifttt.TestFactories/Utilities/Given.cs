using System.Collections.Concurrent;
using System.Reflection;

namespace InvvardDev.Ifttt.TestFactories.Utilities;

public class Given
{
    private static readonly ConcurrentDictionary<string, Assembly> SourceAssemblies = new();
    private static readonly ConcurrentDictionary<Type, Func<int, object>> Factories = new();

    protected Given()
    {
        SourceAssemblies.TryAdd(typeof(Given).Assembly.GetName().Name!, typeof(Given).Assembly);
    }

    private static IEnumerable<T> Generate<T>(int count)
        where T : class
    {
        if (!Factories.ContainsKey(typeof(T)) || Factories[typeof(T)] == null)
        {
            throw new ArgumentException($"Type {typeof(T)} not registered in the factory. Implement a new EntityFactoryBase<T>.");
        }

        return (Factories[typeof(T)](count) as IEnumerable<T>)!;
    }

    /// <summary>
    /// Will build a series of object with the registered factory.
    /// </summary>
    /// <typeparam name="T">Is the type of the object to create.</typeparam>
    /// <param name="count">Is the count requested.</param>
    /// <returns>The list of newly created object. Empty list if count is below 0.</returns>
    public static IEnumerable<T> ALotOf<T>(int count)
        where T : class
        => count < 1 ? new List<T>() : GenerateSome<T>(Assembly.GetCallingAssembly(), count);

    public static T A<T>()
        where T : class
        => GenerateSome<T>(Assembly.GetCallingAssembly()).Single();

    /// <summary>
    /// This method only exists when static things execute before those fixtures which, apparently, can happen.
    /// 
    /// I found out that the dotnet test command will not exactly execute things in the same order as the VS test explorer.
    /// So this will force the given instance to use a certain assembly event when not in context of an instance that's supposed to be there.
    /// </summary>
    /// <typeparam name="T">Is the type of the thing we want to build.</typeparam>
    /// <returns>The built object.</returns>
    public static T A<T>(params Type[] factories)
        where T : class
    {
        foreach (var type in factories)
        {
            Using(type);
        }

        return A<T>();
    }

    /// <summary>
    /// See <see cref="A{T}()"/>.
    /// </summary>
    public static T An<T>(params Type[] factories)
        where T : class
        => A<T>();

    public static T An<T>()
        where T : class
        => GenerateSome<T>(Assembly.GetCallingAssembly()).First();

    private static IEnumerable<T> GenerateSome<T>(Assembly assembly, int count = 1)
        where T : class
    {
        SourceAssemblies.TryAdd(assembly.GetName().Name!, assembly);
        Factories.GetOrAdd(typeof(T), _ => ScanForFactory<T>().Generate);

        var instance = new Given();
        return Generate<T>(count);
    }

    /// <summary>
    /// Could be used to configure a test fixture to add assemblies from other places.
    /// </summary>
    /// <param name="types"></param>
    /// <returns></returns>
    public static void Using(params Type[] types)
    {
        foreach (var assembly in types.Select(t => t.Assembly))
        {
            SourceAssemblies.TryAdd(assembly.GetName().Name!, assembly);
        }
    }

    public static void UsingThisAssembly()
    {
        var assembly = Assembly.GetCallingAssembly();
        SourceAssemblies.TryAdd(assembly.GetName().Name!, assembly);
    }

    /// <summary>
    /// Scans the current assembly for any type inheriting from <see cref="EntityFactoryBase{T}"/>.
    /// <para>
    /// Although this reduces configuration and boilerplate code by an order of magniture it doesn't
    /// solve using this as a package as it's scanning the same assembly as the <see cref="Given"/>
    /// class.
    /// </para>
    /// <para>
    /// Also it's using reflection which is slow. Might be unadvisable when dealing with unit tests. To be benchmarked.
    /// </para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    private static EntityFactoryBase<T> ScanForFactory<T>()
        where T : class
    {
        var types = GetTypes()
                    .Where(t => t.BaseType is not null
                                && typeof(EntityFactoryBase<T>).IsAssignableFrom(t)
                                && t.BaseType.IsGenericType
                                && Array.Exists(t.BaseType.GetGenericArguments(), g => g == typeof(T)))
                    .ToList();
        return types switch
               {
                   { Count: 0 } => throw new ArgumentException($"Type {typeof(T)} has no related factories."),
                   { Count: > 1 } => throw new ArgumentException($"Type {typeof(T)} has more than one factory."),
                   _ => (EntityFactoryBase<T>)Activator.CreateInstance(types.First())!
               };
    }

    private static IEnumerable<Type> GetTypes()
        => SourceAssemblies.SelectMany(a => a.Value.GetTypes());
}
