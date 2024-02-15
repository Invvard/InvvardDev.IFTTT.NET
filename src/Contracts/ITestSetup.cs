using InvvardDev.Ifttt.Toolkit.Models;

namespace InvvardDev.Ifttt.Contracts;

public interface ITestSetup
{
    Task<Samples> PrepareSetupListing();
}