using System.Reflection;
using System.Reflection.Emit;

namespace InvvardDev.Ifttt.Trigger.Tests.Factories;

internal static class CreateType
{
    private const string DynamicAssemblyName = "DynamicAssembly";
    private const string DynamicModuleName = "DynamicModule";

    public static TypeBuilder Called(string typeName)
        => AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(DynamicAssemblyName), AssemblyBuilderAccess.Run)
                          .DefineDynamicModule(DynamicModuleName)
                          .DefineType(typeName, TypeAttributes.Public);

    public static TypeBuilder WithAttribute<TAttribute>(this TypeBuilder typeBuilder, params object[] attributeParams)
        where TAttribute : Attribute
    {
        var attributeParamTypes = attributeParams.Select(param => param.GetType()).ToArray();
        var constructorInfo = typeof(TAttribute).GetConstructor(attributeParamTypes)!;
        var con = new CustomAttributeBuilder(constructorInfo, attributeParams);
        typeBuilder.SetCustomAttribute(con);

        return typeBuilder;
    }

    public static TypeBuilder ThatImplements<TInterface>(this TypeBuilder typeBuilder)
    {
        typeBuilder.AddInterfaceImplementation(typeof(TInterface));
        foreach (var method in typeof(TInterface).GetMethods().ToList())
        {
            typeBuilder.WithMethod(method.Name,
                                   method.ReturnType,
                                   MethodAttributes.Public | MethodAttributes.Virtual,
                                   method.GetParameters().Select(p => p.ParameterType).ToArray());
        }

        return typeBuilder;
    }

    public static TypeBuilder WithProperty<TProperty>(this TypeBuilder typeBuilder,
                                                      string propertyName,
                                                      bool writeable = true,
                                                      bool readable = true)
    {
        typeBuilder.CreateProperty<TProperty>(propertyName, writeable, readable);

        return typeBuilder;
    }

    public static TypeBuilder WithPropertyAttribute<TProperty, TAttribute>(this TypeBuilder typeBuilder,
                                                                           string propertyName,
                                                                           params object[] attributeParams)
        where TAttribute : Attribute
    {
        var propertyBuilder = typeBuilder.CreateProperty<TProperty>(propertyName, true, true);
        var attributeParamTypes = attributeParams.Select(param => param.GetType()).ToArray();
        propertyBuilder.SetCustomAttribute(new CustomAttributeBuilder(typeof(TAttribute).GetConstructor(attributeParamTypes)!,
                                                                      attributeParams));

        return typeBuilder;
    }

    private static PropertyBuilder CreateProperty<TProperty>(this TypeBuilder typeBuilder,
                                                             string propertyName,
                                                             bool writeable,
                                                             bool readable)
    {
        if (writeable && !readable)
        {
            throw new ArgumentException("A property cannot be writeable and not readable");
        }

        var propertyBuilder = typeBuilder.DefineProperty(propertyName, PropertyAttributes.None, typeof(TProperty), null);
        var getMethodBuilder = typeBuilder.DefineMethod($"get_{propertyName}",
                                                        writeable ? MethodAttributes.Public : MethodAttributes.Private,
                                                        typeof(TProperty),
                                                        [typeof(TProperty)]);
        var getMethodIlGenerator = getMethodBuilder.GetILGenerator();
        getMethodIlGenerator.Emit(OpCodes.Ldarg_0);
        getMethodIlGenerator.Emit(OpCodes.Ret);
        propertyBuilder.SetGetMethod(getMethodBuilder);

        var setMethodBuilder = typeBuilder.DefineMethod($"set_{propertyName}",
                                                        readable ? MethodAttributes.Public : MethodAttributes.Private,
                                                        typeof(TProperty),
                                                        [typeof(TProperty)]);
        var setMethodIlGenerator = setMethodBuilder.GetILGenerator();
        setMethodIlGenerator.Emit(OpCodes.Ldarg_0);
        setMethodIlGenerator.Emit(OpCodes.Ret);
        propertyBuilder.SetSetMethod(setMethodBuilder);

        return propertyBuilder;
    }

    public static TypeBuilder WithMethod(this TypeBuilder typeBuilder,
                                         string methodName,
                                         Type returnType,
                                         MethodAttributes? methodAttributes,
                                         params Type[] parameterTypes)
    {
        methodAttributes ??= MethodAttributes.Public | MethodAttributes.Virtual;
        var methodBuilder = typeBuilder.DefineMethod(methodName,
                                                     methodAttributes.Value,
                                                     returnType,
                                                     parameterTypes);
        var methodIlGenerator = methodBuilder.GetILGenerator();
        methodIlGenerator.Emit(OpCodes.Ret);

        return typeBuilder;
    }

    public static Type Build(this TypeBuilder typeBuilder)
    {
        return typeBuilder.CreateType() ?? throw new InvalidOperationException("Unable to create dynamic type");
    }
}