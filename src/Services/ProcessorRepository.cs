using InvvardDev.Ifttt.Contracts;

namespace InvvardDev.Ifttt.Services;

internal abstract class ProcessorRepository<T> : IProcessorRepository<T>
{
    protected readonly Dictionary<string, T> Processors = new();
    
    public void UpsertProcessor(string processorSlug, T processorType)
    {
        Processors[processorSlug] = processorType;
    }

    public T? GetProcessor(string processorSlug) 
        => Processors.GetValueOrDefault(processorSlug);

    public abstract void UpsertDataField(string processorSlug, string dataFieldSlug, Type dataFieldType);

    public abstract Type? GetDataFieldType(string processorSlug, string dataFieldSlug);

    public abstract TInterface? GetProcessorInstance<TInterface>(string processorSlug);

    public IEnumerable<string> GetProcessorSlugs()
        => Processors.Keys.ToArray();
}