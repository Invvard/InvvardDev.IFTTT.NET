using InvvardDev.Ifttt.Models.Trigger;

namespace InvvardDev.Ifttt.Contracts;

public interface IProcessorRepository
{
    Task AddProcessor(ProcessorTree processorTree);
    
    Task UpdateProcessor(ProcessorTree processorTree);
    
    Task<bool> Exists(string key);
    
    Task<ProcessorTree?> GetProcessor(string key);
}