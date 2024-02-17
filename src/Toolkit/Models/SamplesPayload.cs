using System.Text.Json.Serialization;

namespace InvvardDev.Ifttt.Toolkit.Models;

public class SamplesPayload(ProcessorPayload processorPayload)
{
    public SamplesPayload() : this(new ProcessorPayload())
    {
        
    }
    
    private ProcessorPayload Samples { get; } = processorPayload;

    internal void SkimEmptyProcessors()
    {
        Samples.Triggers?.RemoveEmptyProcessors();
        if (Samples.Triggers?.Count == 0)
        {
            Samples.Triggers = default;
        }
    }
}

public record ProcessorPayload
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Processors? Triggers { get; set; }
}
