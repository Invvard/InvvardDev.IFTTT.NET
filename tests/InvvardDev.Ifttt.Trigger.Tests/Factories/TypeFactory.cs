using System.Reflection;
using System.Reflection.Emit;

namespace InvvardDev.Ifttt.Trigger.Tests.Factories;

internal static class CreateType
{
    private const string DynamicAssemblyName = "DynamicAssembly";
    private const string DynamicModuleName = "DynamicModule";

    private static readonly ModuleBuilder ModuleBuilder = AssemblyBuilder
                                                          .DefineDynamicAssembly(new AssemblyName(DynamicAssemblyName),
                                                                                 AssemblyBuilderAccess.Run)
                                                          .DefineDynamicModule(DynamicModuleName);


    public static TypeBuilder Called(string typeName)
        => ModuleBuilder.DefineType(typeName, TypeAttributes.Public);

    public static TypeBuilder WithAttribute<TAttribute>(this TypeBuilder typeBuilder, params object[] attributeParams)
        where TAttribute : Attribute
    {
        var attributeParamTypes = attributeParams.Select(param => param.GetType()).ToArray();
        typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(typeof(TAttribute).GetConstructor(attributeParamTypes)!,
                                                                  attributeParams));

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

        typeBuilder.GetProperties().Single()

        return typeBuilder;
    }

    public static TypeBuilder WithPropertyAttribute<TAttribute>(this TypeBuilder typeBuilder,
                                                                string propertyName,
                                                                params object[] attributeParams)
        where TAttribute : Attribute
    {
        var attributeParamTypes = attributeParams.Select(param => param.GetType()).ToArray();
        var propertyInfo = typeBuilder.GetProperties().Single(p => p.Name == propertyName);
        
        
        typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(typeof(TAttribute).GetConstructor(attributeParamTypes)!,
                                                                  new object[] { propertyName, slug }));

        return typeBuilder;
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