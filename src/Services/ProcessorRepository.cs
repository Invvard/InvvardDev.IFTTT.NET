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

    public Task<ProcessorTree?> GetProcessor(string key)
        => Task.FromResult(processors.GetValueOrDefault(key));
}