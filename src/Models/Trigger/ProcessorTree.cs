using InvvardDev.Ifttt.Models.Core;

namespace InvvardDev.Ifttt.Models.Trigger;

public record ProcessorTree(string ProcessorSlug, Type ProcessorType, ProcessorKind Kind)
{
    internal string Key => Kind.GetProcessorKey(ProcessorSlug);
    
    public Dictionary<string, Type> DataFields { get; } = [];
}
