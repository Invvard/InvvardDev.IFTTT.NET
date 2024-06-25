using Bogus;

namespace InvvardDev.Ifttt.TestFactories.Utilities;

[ExcludeFromCodeCoverage]
public abstract class EntityFactoryBase<T>
    where T : class
{
    private readonly Faker<T> faker;

    protected EntityFactoryBase()
    {
        faker = Configure();
    }

    public IEnumerable<T> Generate(int count = 1)
        => faker!.Generate(count);

    protected abstract Faker<T> Configure();
}
