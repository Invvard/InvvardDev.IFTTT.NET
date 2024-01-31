using InvvardDev.Ifttt.Core.Contracts;

namespace InvvardDev.Ifttt.Core.Services;

public abstract class DataRepository : IRepository
{
    protected readonly Dictionary<string, Type> DataTypes = new();
    
    public virtual void UpsertType(string slug, Type type)
    {
        DataTypes[slug] = type;
    }

    public virtual TInterface? GetInstance<TInterface>(string slug)
    {
        if (!DataTypes.TryGetValue(slug, out var dataType)
            || Activator.CreateInstance(dataType) is not TInterface instance)
        {
            return default;
        }

        return instance;
    }

    public virtual Type? GetDataType(string slug)
    {
        DataTypes.TryGetValue(slug, out var dataFieldsType);

        return dataFieldsType;
    }
}