using InvvardDev.Ifttt.Models.Trigger;

namespace InvvardDev.Ifttt.Contracts;

public interface IProcessorService
{
    Task AddOrUpdateProcessor(ProcessorTree processorTree);
    
    Task AddDataField(string processorSlug, string dataFieldSlug, Type dataFieldType);
    
    Task<bool> Exists(string processorSlug);
    
    Task<ProcessorTree?> GetProcessor(string processorSlug);

    Task<Type?> GetDataFieldType(string processorSlug, string dataFieldSlug);

    Task<TInterface?> GetProcessorInstance<TInterface>(string processorSlug);
}
