using InvvardDev.Ifttt.Toolkit.Models;

namespace InvvardDev.Ifttt.Toolkit.Contracts;

/// <summary>
/// Interface for setting up the test listing.
/// </summary>
public interface ITestSetup
{
    /// <summary>
    /// Prepares the setup listing for IFTTT to use for testing.
    /// </summary>
    /// <returns>The list of processors to be tested.</returns>
    Task<ProcessorPayload> PrepareSetupListing();
}
