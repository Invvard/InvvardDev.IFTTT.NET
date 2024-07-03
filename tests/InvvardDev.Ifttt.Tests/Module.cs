using System.Runtime.CompilerServices;
using InvvardDev.Ifttt.TestFactories;

namespace InvvardDev.Ifttt.Tests;

public static class Module
{
    [ModuleInitializer]
    public static void Initialize()
    {
        IftttNetDataFactories.Register();
    }
}
