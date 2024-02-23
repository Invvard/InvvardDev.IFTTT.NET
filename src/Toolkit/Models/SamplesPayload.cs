using System.Text.Json.Serialization;

namespace InvvardDev.Ifttt.Toolkit.Models;

/// <summary>
/// Class to hold samples payload for IFTTT test setup.
/// </summary>
/// <param name="processorPayload">The processors list with data fields.</param>
public class SamplesPayload(ProcessorPayload processorPayload)
{
    public SamplesPayload() : this(new ProcessorPayload())
    {
        
    }
    
    public ProcessorPayload Samples { get; } = processorPayload;

    /// <summary>
    /// Removes empty processors from the payload.
    /// </summary>
    internal void SkimEmptyProcessors()
    {
        Samples.Triggers?.RemoveEmptyProcessors();
        if (Samples.Triggers?.Count == 0)
        {
            Samples.Triggers = default;
        }
    }
}

/// <summary>
/// Record to hold the processors and related data fields for the IFTTT test setup.
/// </summary>
public record ProcessorPayload
{
    /// <summary>
    /// Gets or sets the triggers and related trigger fields for the IFTTT test setup.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Processors? Triggers { get; set; }
}
