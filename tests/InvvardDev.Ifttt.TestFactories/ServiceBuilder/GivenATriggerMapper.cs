using InvvardDev.Ifttt.Contracts;
using Moq;

namespace InvvardDev.Ifttt.TestFactories.ServiceBuilder;

public class GivenATriggerMapper : MockServiceBuilderBase<ITriggerMapper, GivenATriggerMapper>
{
    public GivenATriggerMapper Throws(string exceptionMessage)
    {
        MockFactory.Setup(m => m.MapTriggerProcessors(It.IsAny<CancellationToken>()))
                   .ThrowsAsync(new Exception(exceptionMessage));

        return this;
    }

    public GivenATriggerMapper TakesTooLong(CancellationToken token)
    {
        MockFactory.Setup(m => m.MapTriggerProcessors(It.IsAny<CancellationToken>()))
                   .Returns(Task.Delay(Timeout.Infinite, token));

        return this;
    }
}
