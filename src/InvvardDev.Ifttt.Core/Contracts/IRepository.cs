namespace InvvardDev.Ifttt.Core.Contracts;

public interface IRepository
{
    void UpsertType(string slug, Type type);
    
    TInterface? GetInstance<TInterface>(string slug);

    Type? GetDataType(string slug);
}