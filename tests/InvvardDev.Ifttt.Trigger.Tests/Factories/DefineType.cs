using System.Reflection;

namespace InvvardDev.Ifttt.Trigger.Tests.Factories;

internal class DefineType
{
    private readonly string typeName;
    private readonly List<Type> interfaces = new();
    private readonly List<Property> properties = new();
    private readonly List<Method> methods = new();
    private readonly Dictionary<string, CustomAttribute> attributes = new();

    private DefineType(string typeName)
    {
        this.typeName = typeName;
    }

    public static DefineType Called(string typeName) => new(typeName);

    public DefineType ImplementInterface<TInterface>()
    {
        interfaces.Add(typeof(TInterface));
        return this;
    }

    public DefineType WithProperty<TType>(string propertyName, bool writeable = true, bool readable = true)
    {
        properties.Add(new Property(propertyName, typeof(TType), readable, writeable));
        return this;
    }

    public DefineType WithMethod(string methodName, Type returnType, bool isPublic, params Type[] parameterTypes)
    {
        methods.Add(new Method(methodName, returnType, isPublic, parameterTypes));
        return this;
    }

    public DefineType WithCustomAttribute<TAttribute>(string targetName, params object[] parameters)
        where TAttribute : Attribute
    {
        attributes.Add(targetName, new CustomAttribute(typeof(TAttribute), parameters));
        return this;
    }

    public Type Build()
    {
        var typeFactory = new TypeFactory(typeName, GetAttribute(typeName));

        foreach (var itf in interfaces)
        {
            typeFactory.ImplementInterface(itf);
        }

        foreach (var property in properties)
        {
            typeFactory.AddProperty(property.Name,
                                    property.Type,
                                    property.Readable,
                                    property.Writeable,
                                    GetAttribute(property.Name));
        }
        
        foreach (var method in methods)
        {
            typeFactory.AddMethod(method.Name,
                                  method.ReturnType,
                                  method.IsPublic ? MethodAttributes.Public : MethodAttributes.Private,
                                  GetAttribute(method.Name),
                                  method.ParameterTypes);
        }
        
        return typeFactory.Compile();
    }

    private CustomAttribute? GetAttribute(string targetName)
        => attributes.TryGetValue(targetName, out var attribute) ? attribute : null;
}

internal record Property(string Name, Type Type, bool Readable, bool Writeable);

internal record Method(string Name, Type ReturnType, bool IsPublic, Type[] ParameterTypes);

internal record CustomAttribute(Type Type, object[] Parameters);