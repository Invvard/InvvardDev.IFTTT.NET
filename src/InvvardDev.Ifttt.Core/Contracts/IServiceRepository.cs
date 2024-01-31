namespace InvvardDev.Ifttt.Core.Contracts;

public interface IServiceRepository
{
    void UpsertProcessorType(string slug, Type processorType);
    
    void UpsertDataFieldsType(string slug, Type dataFieldsType);

    TInterface? GetProcessorInstance<TInterface>(string slug);

    Type? GetDataFieldsType(string slug);
}