using InvvardDev.Ifttt.Models.Core;

namespace InvvardDev.Ifttt.Models.Trigger;

public class ProcessorTree(string processorSlug, Type processorType, ProcessorKind kind)
{
    internal string Key => Kind.GetProcessorKey(Slug);
    
    public string Slug { get; } = processorSlug;
    
    public Type Type { get; } = processorType;

    public ProcessorKind Kind { get; } = kind;
    
    public Dictionary<string, Type> DataFields { get; } = [];
}