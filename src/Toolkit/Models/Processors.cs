namespace InvvardDev.Ifttt.Toolkit;

/// <summary>
/// Class to hold the processors and related data fields for the IFTTT test setup.
/// </summary>
public class Processors : Dictionary<string, Dictionary<string, string>>
{
    /// <summary>
    /// Adds a processor to the processors collection.
    /// </summary>
    /// <param name="slug">The processor slug.</param>
    /// <returns>The current processor collection.</returns>
    public Processors AddProcessor(string slug)
    {
        Add(slug, new Dictionary<string, string>());

        return this;
    }

    /// <summary>
    /// Adds a data field to the related processor.
    /// </summary>
    /// <param name="slug">The processor slug to attach the data field to.</param>
    /// <param name="dataFieldSlug">The data field slug.</param>
    /// <param name="data">The data to be used in the test.</param>
    /// <returns>The current processor collection.</returns>
    public Processors AddDataField(string slug, string dataFieldSlug, string data)
    {
        if (TryGetValue(slug, out var dataFields))
        {
            dataFields.Add(dataFieldSlug, data);
        }

        return this;
    }
    
    /// <summary>
    /// A processor without any data fields won't be used in the test setup, so it's safe to remove it.
    /// </summary>
    internal void RemoveEmptyProcessors()
    {
        var emptyProcessors = this.Where(p => p.Value.Count == 0).ToList();
        foreach (var emptyProcessor in emptyProcessors)
        {
            Remove(emptyProcessor.Key);
        }
    }
}