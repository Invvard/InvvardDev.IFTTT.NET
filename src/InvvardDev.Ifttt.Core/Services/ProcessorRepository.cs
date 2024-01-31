using InvvardDev.Ifttt.Core.Contracts;
using InvvardDev.Ifttt.Core.Models;

namespace InvvardDev.Ifttt.Core.Services;

public class ProcessorRepository : IServiceRepository
{
    private readonly Dictionary<string, DataType> dataTypes = new();
    
    public void UpsertProcessorType(string slug, Type processorType)
    {
        if (!dataTypes.TryGetValue(slug, out var dataType))
        {
            dataTypes.Add(slug, new DataType(slug, processorType));
        }
        else
        {
            dataTypes[slug] = dataType with
                              {
                                  ProcessorType = processorType
                              };
        }
    }

    public void UpsertDataFieldsType(string slug, Type dataFieldsType)
    {
        if (!dataTypes.TryGetValue(slug, out var dataType))
        {
            throw new InvalidOperationException($"Data type '{slug}' was not found.");
        }

        dataTypes[slug] = dataType with
                          {
                              DataFieldsType = dataFieldsType
                          };
    }

    public TInterface? GetProcessorInstance<TInterface>(string slug)
    {
        if (!dataTypes.TryGetValue(slug, out var dataType)
            || Activator.CreateInstance(dataType.ProcessorType) is not TInterface processorInstance)
        {
            return default;
        }

        return processorInstance;
    }

    public Type? GetDataFieldsType(string slug)
    {
        dataTypes.TryGetValue(slug, out var dataType);

        return dataType?.DataFieldsType;
    }
}