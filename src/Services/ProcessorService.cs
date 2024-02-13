using InvvardDev.Ifttt.Contracts;
using InvvardDev.Ifttt.Models.Core;
using InvvardDev.Ifttt.Models.Trigger;

namespace InvvardDev.Ifttt.Services;

internal abstract class ProcessorService(IProcessorRepository processorRepository) : IProcessorService
{
    protected abstract ProcessorKind Kind { get; }

    public async Task AddOrUpdateProcessor(ProcessorTree processorTree)
    {
        await (await GetProcessor(processorTree.Slug) switch
               {
                   null => processorRepository.AddProcessor(processorTree),
                   { } existingProcessorTree when existingProcessorTree.Type == processorTree.Type
                       => processorRepository.UpdateProcessor(processorTree),
                   { } pt when pt.Type != processorTree.Type
                       => throw new InvalidOperationException($"Conflict: '{pt.Kind}' processor with slug '{pt.Slug}' already exists (Type is '{pt.Type}')."),
                   _ => throw new ArgumentOutOfRangeException(nameof(processorTree))
               });
    }

    public async Task AddDataField(string processorSlug, string dataFieldSlug, Type dataFieldType)
    {
        switch (await GetProcessor(processorSlug))
        {
            case { } processorTree when processorTree.DataFields.ContainsKey(dataFieldSlug) && processorTree.DataFields[dataFieldSlug] != dataFieldType:
                throw new
                    InvalidOperationException($"Conflict: '{processorTree.Kind}' processor with slug '{processorTree.Slug}' already has a data field with slug '{dataFieldSlug}' with a different type '{processorTree.DataFields[dataFieldSlug]}'.");
            case { } processorTree when processorTree.DataFields.ContainsKey(dataFieldSlug) && processorTree.DataFields[dataFieldSlug] == dataFieldType:
                // Nothing to do
                break;
            case { } processorTree when !processorTree.DataFields.ContainsKey(dataFieldSlug):
                processorTree.DataFields.Add(dataFieldSlug, dataFieldType);
                await processorRepository.UpdateProcessor(processorTree);
                break;
            default:
                throw new InvalidOperationException($"Processor with slug '{processorSlug}' does not exist.");
        }
    }

    public Task<bool> Exists(string processorSlug)
        => processorRepository.Exists(Kind.GetProcessorKey(processorSlug));

    public async Task<Type?> GetDataFieldType(string processorSlug, string dataFieldSlug)
    {
        if (await GetProcessor(processorSlug) is { } processorTree
            && processorTree.DataFields.TryGetValue(dataFieldSlug, out var dataFieldType))
        {
            return dataFieldType;
        }

        return default;
    }

    public async Task<ProcessorTree?> GetProcessor(string processorSlug)
        => await processorRepository.GetProcessor(Kind.GetProcessorKey(processorSlug));

    public async Task<TInterface?> GetProcessorInstance<TInterface>(string processorSlug)
    {
        if (await GetProcessor(processorSlug) is { } processorTree
            && Activator.CreateInstance(processorTree.Type) is TInterface processor)
        {
            return processor;
        }

        return default;
    }
}