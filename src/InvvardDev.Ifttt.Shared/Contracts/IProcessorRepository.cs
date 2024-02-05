namespace InvvardDev.Ifttt.Shared.Contracts;

public interface IProcessorRepository<T>
{
    void UpsertProcessor(string processorSlug, T processorType);
    
    T? GetProcessor(string processorSlug);

    Type? GetDataFieldType(string processorSlug, string dataFieldSlug);

    TInterface? GetProcessorInstance<TInterface>(string processorSlug);
}