using InvvardDev.Ifttt.Contracts;
using InvvardDev.Ifttt.Models.Trigger;

namespace InvvardDev.Ifttt.Services;

public class ProcessorRepository : IProcessorRepository
{
    private readonly Dictionary<string, ProcessorTree> processors = new();

    public Task AddProcessor(ProcessorTree processorTree)
    {
        processors.Add(processorTree.Key, processorTree);
        
        return Task.CompletedTask;
    }

    public Task UpdateProcessor(ProcessorTree processorTree)
    {
        processors[processorTree.Key] = processorTree;

        return Task.CompletedTask;
    }

    public Task<bool> Exists(string key)
        => Task.FromResult(processors.ContainsKey(key));

    public Task<ProcessorTree?> GetProcessorByKey(string key)
        => Task.FromResult(processors.GetValueOrDefault(key));

    public Task<IEnumerable<ProcessorTree>> FilterProcessors(Func<ProcessorTree, bool> predicate)
        => Task.FromResult(GetProcessorsTrees().Where(predicate));

    public Task<IEnumerable<ProcessorTree>> GetAllProcessors() 
        => Task.FromResult(GetProcessorsTrees());
    
    private IEnumerable<ProcessorTree> GetProcessorsTrees() 
        => processors.Select(p => p.Value);
}