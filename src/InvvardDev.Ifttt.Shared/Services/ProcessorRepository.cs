﻿using InvvardDev.Ifttt.Shared.Contracts;

namespace InvvardDev.Ifttt.Shared.Services;

public abstract class ProcessorRepository<T> : IProcessorRepository<T>
{
    protected readonly Dictionary<string, T> Processors = new();
    
    public void UpsertProcessor(string processorSlug, T processorType)
    {
        Processors[processorSlug] = processorType;
    }

    public abstract void UpsertDataField(string processorSlug, string dataFieldSlug, Type dataFieldType);

    public T? GetProcessor(string processorSlug) 
        => Processors.GetValueOrDefault(processorSlug);

    public abstract Type? GetDataFieldType(string processorSlug, string dataFieldSlug);

    public abstract TInterface? GetProcessorInstance<TInterface>(string processorSlug);

    public IEnumerable<string> GetProcessorSlugs()
        => Processors.Keys.ToArray();
}