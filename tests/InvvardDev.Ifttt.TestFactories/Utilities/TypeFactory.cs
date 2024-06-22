using System.Reflection;
using System.Reflection.Emit;

namespace InvvardDev.Ifttt.TestFactories.Utilities;

internal class TypeFactory
{
    private const string DynamicAssemblyName = "DynamicAssembly";
    private const string DynamicModuleName = "DynamicModule";
    private readonly TypeBuilder typeBuilder;

    private readonly ModuleBuilder moduleBuilder = AssemblyBuilder
                                                   .DefineDynamicAssembly(new AssemblyName(DynamicAssemblyName),
                                                                          AssemblyBuilderAccess.Run)
                                                   .DefineDynamicModule(DynamicModuleName);


    public TypeFactory(string typeName)
    {
        typeBuilder = moduleBuilder.DefineType(typeName, TypeAttributes.Public);
    }

    public TypeFactory(string typeName, CustomAttribute? attribute) : this(typeName)
    {
        if (attribute is null) return;

        AddAttribute(attribute.Type, attribute.Parameters);
    }

    public TypeFactory ImplementInterface(Type interfaceType)
    {
        typeBuilder.AddInterfaceImplementation(interfaceType);
        foreach (var method in interfaceType.GetMethods().ToList())
        {
            AddMethod(method.Name,
                      method.ReturnType,
                      MethodAttributes.Public | MethodAttributes.Virtual,
                      null,
                      method.GetParameters().Select(p => p.ParameterType).ToArray());
        }

        return this;
    }

    public TypeFactory AddMethod(string methodName,
                                 Type returnType,
                                 MethodAttributes? methodAttributes,
                                 CustomAttribute? attribute,
                                 params Type[] parameterTypes)
    {
        methodAttributes ??= MethodAttributes.Public;
        var methodBuilder = typeBuilder.DefineMethod(methodName,
                                                     methodAttributes.Value,
                                                     returnType,
                                                     parameterTypes);

        if (attribute is not null)
        {
            AddAttribute(methodBuilder, attribute.Type, attribute.Parameters);
        }

        var methodIlGenerator = methodBuilder.GetILGenerator();
        methodIlGenerator.Emit(OpCodes.Ret);

        return this;
    }

    public TypeFactory AddProperty(string propertyName,
                                   Type propertyType,
                                   bool writeable,
                                   bool readable,
                                   CustomAttribute? attribute = null)
    {
        if (!writeable && !readable)
        {
            throw new ArgumentException("A property cannot be writeable and not readable");
        }

        var propertyBuilder = typeBuilder.DefineProperty(propertyName, PropertyAttributes.None, propertyType, null);
        var getMethodBuilder = typeBuilder.DefineMethod($"get_{propertyName}",
                                                        writeable ? MethodAttributes.Public : MethodAttributes.Private,
                                                        propertyType,
                                                        [propertyType]);
        var getMethodIlGenerator = getMethodBuilder.GetILGenerator();
        getMethodIlGenerator.Emit(OpCodes.Ldarg_0);
        getMethodIlGenerator.Emit(OpCodes.Ret);
        propertyBuilder.SetGetMethod(getMethodBuilder);

        var setMethodBuilder = typeBuilder.DefineMethod($"set_{propertyName}",
                                                        readable ? MethodAttributes.Public : MethodAttributes.Private,
                                                        propertyType,
                                                        [propertyType]);
        var setMethodIlGenerator = setMethodBuilder.GetILGenerator();
        setMethodIlGenerator.Emit(OpCodes.Ldarg_0);
        setMethodIlGenerator.Emit(OpCodes.Ret);
        propertyBuilder.SetSetMethod(setMethodBuilder);

        if (attribute is not null)
        {
            AddAttribute(propertyBuilder, attribute.Type, attribute.Parameters);
        }

        return this;
    }

    private static CustomAttributeBuilder BuildAttribute(Type attributeType, object[] attributeParams)
    {
        var attributeParamTypes = attributeParams.Select(param => param.GetType()).ToArray();
        var constructorInfo = attributeType.GetConstructor(attributeParamTypes)!;
        var attr = new CustomAttributeBuilder(constructorInfo, attributeParams);

        return attr;
    }

    private void AddAttribute(Type attributeType, object[] attributeParams)
    {
        var attr = BuildAttribute(attributeType, attributeParams);
        typeBuilder.SetCustomAttribute(attr);
    }

    private void AddAttribute(MethodBuilder methodBuilder, Type attributeType, object[] attributeParams)
    {
        var attr = BuildAttribute(attributeType, attributeParams);
        methodBuilder.SetCustomAttribute(attr);
    }

    private void AddAttribute(PropertyBuilder propertyBuilder, Type attributeParams, object[] attributeParameters)
    {
        var attr = BuildAttribute(attributeParams, attributeParameters);
        propertyBuilder.SetCustomAttribute(attr);
    }

    public Type Compile()
    {
        return typeBuilder.CreateType() ?? throw new InvalidOperationException("Unable to create dynamic type");
    }
}