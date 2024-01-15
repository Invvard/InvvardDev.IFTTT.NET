using System.Reflection;
using System.Reflection.Emit;
using Castle.DynamicProxy;
using InvvardDev.Ifttt.Trigger.Attributes;

namespace InvvardDev.Ifttt.Trigger.Tests.Factories;

public class TestClassFactory
{
    public TestClassFactory()
    {
        
    }

    public TestClassFactory WithAttribute<TType, TAttribute>(this TType type)
        where TType : Type
        where TAttribute : Attribute
    {
        
    }

    public static void SetCustomAttribute(this Type type, CustomAttributeBuilder attributeBuilder)
        {
            var typeBuilder = type.Module.DefineType(
                                                     type.FullName + "_WithAttribute",
                                                     TypeAttributes.Public | TypeAttributes.Class
                                                                           | TypeAttributes.BeforeFieldInit,
                                                     type);

            typeBuilder.SetCustomAttribute(attributeBuilder);

            var newType = typeBuilder.CreateType();
        }

        return this;
    }

    public static void CreateClass()
    {
        var generator = new ProxyGenerator();
        var proxy = generator.CreateClassProxy<DynamicClassBase>(new MyInterceptor());

        // Accessing the dynamically generated type
        var dynamicType = proxy.GetType();

        // Adding a custom attribute to the dynamically generated type
        dynamicType.SetCustomAttribute(new CustomAttributeBuilder(typeof(MyCustomAttribute).GetConstructor(Type.EmptyTypes),
                                                                  new object[] { }));
    }

    public static string GivenATrigger(string slug = "trigger_slug")
    {
        AssemblyName assemblyName = new AssemblyName("DynamicAssembly");
        AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
        ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("DynamicModule");

        TypeBuilder typeBuilder = moduleBuilder.DefineType("DynamicType", TypeAttributes.Public | TypeAttributes.Class);

        ConstructorInfo? attributeCtor = typeof(TriggerAttribute).GetConstructor(new[] { typeof(string) });
        CustomAttributeBuilder attributeBuilder = new CustomAttributeBuilder(attributeCtor!, new object[] { slug });
        typeBuilder.SetCustomAttribute(attributeBuilder);

        // Create the dynamic type
        Type dynamicType = typeBuilder.CreateType();

        // Create an instance of the dynamic type
        var instance = Activator.CreateInstance(dynamicType);

        // Example: check if the new attribute is present
        var triggerAttribute = (TriggerAttribute)Attribute.GetCustomAttribute(instance!.GetType(), typeof(TriggerAttribute))!;

        Console.WriteLine($"Attribute Value: {triggerAttribute!.Slug}");

        return triggerAttribute.Slug;
    }

    public class DynamicClassBase
    {
    }
}

public class MyInterceptor : IInterceptor
{
    public void Intercept(IInvocation invocation)
    {
        invocation.Proceed();
    }
}