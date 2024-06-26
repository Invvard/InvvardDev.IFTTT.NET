using System.Linq.Expressions;
using Moq;

namespace InvvardDev.Ifttt.TestFactories.Utilities;

/// <summary>
/// This class exposes the necessary building blocks to implement a fluent mock factory.
/// </summary>
/// <typeparam name="TService">Is the type of the service to be mocked.</typeparam>
/// <typeparam name="TMockBuilder">Is the type of the builder.</typeparam>
[ExcludeFromCodeCoverage]
public abstract class MockServiceBuilderBase<TService, TMockBuilder>
    where TService : class
    where TMockBuilder : MockServiceBuilderBase<TService, TMockBuilder>, new()
{
    protected readonly Mock<TService> MockFactory = new();
    public static TMockBuilder That => new();

    public TService Provision()
        => MockFactory.Object;

    public Mock<TService> GetMock()
        => MockFactory;

    public MockServiceComponentProvisioner<TServiceComponent> HasA<TServiceComponent>(Expression<Func<TService, TServiceComponent>> func)
        where TServiceComponent : class
        => new(this, func);

    public MockMethodResultProvisioner<TObject> Returns<TObject>(TObject returned)
        => new(this, returned);

    public MockVoidMethodProvisioner Does<T1>(Action<T1> callback)
        => new(this, callback);

    public MockVoidMethodProvisioner Does<T1, T2>(Action<T1, T2> callback)
        => new(this, callback);

    public MockVoidMethodProvisioner Does<T1, T2, T3>(Action<T1, T2, T3> callback)
        => new(this, callback);

    public static TService ThatDoesNothing
        => new Mock<TService>().Object;

    public class MockMethodResultProvisioner<TObject>(MockServiceBuilderBase<TService, TMockBuilder> context, TObject returned)
    {
        private Delegate? callback;

        public MockMethodResultProvisioner<TObject> AndDoes<T1>(Action<T1> callbackMethod)
        {
            this.callback = callbackMethod;
            return this;
        }

        public MockMethodResultProvisioner<TObject> AndDoes<T1, T2>(Action<T1, T2> callbackMethod)
        {
            this.callback = callbackMethod;
            return this;
        }

        public MockMethodResultProvisioner<TObject> AndDoes<T1, T2, T3, T4>(Action<T1, T2, T3, T4> callbackMethod)
        {
            this.callback = callbackMethod;
            return this;
        }

        public MockMethodResultProvisioner<TObject> AndDoes<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> callbackMethod)
        {
            this.callback = callbackMethod;
            return this;
        }

        public TMockBuilder When(Expression<Func<TService, TObject>> fn)
        {
            var setup = context.MockFactory.Setup(fn).Returns(returned);
            if (callback != null)
            {
                setup.Callback(callback);
            }
            return (context as TMockBuilder)!;
        }

        public TMockBuilder When(Expression<Func<TService, Task<TObject>>> fn)
        {
            var setup = context.MockFactory.Setup(fn).ReturnsAsync(returned);
            if (callback != null)
            {
                setup.Callback(callback);
            }
            return (context as TMockBuilder)!;
        }
    }

    public class MockVoidMethodProvisioner(MockServiceBuilderBase<TService, TMockBuilder> context, Delegate callback)
    {
        public TMockBuilder When(Expression<Action<TService>> fn)
        {
            context.GetMock().Setup(fn).Callback(callback);
            return (context as TMockBuilder)!;
        }
    }

    public class MockServiceComponentProvisioner<TServiceComponent>
        where TServiceComponent : class
    {
        internal MockServiceBuilderBase<TService, TMockBuilder> BaseBuilderContext { get; }

        public Mock<TServiceComponent> Factory { get; }

        public MockServiceComponentProvisioner(MockServiceBuilderBase<TService, TMockBuilder> context, Expression<Func<TService, TServiceComponent>> func)
        {
            BaseBuilderContext = context;
            Factory = new Mock<TServiceComponent>();
            BaseBuilderContext.GetMock().Setup(func).Returns(Factory.Object);
        }

        public ProvisionerComposer<TServiceComponent> ThatProvides(Expression<Func<TServiceComponent, object>> func)
        {
            return new ProvisionerComposer<TServiceComponent>(this, func, Factory);
        }
    }

    public class ProvisionerComposer<TServiceComponent>(MockServiceComponentProvisioner<TServiceComponent> componentProvisionnerContext,
                                                        Expression<Func<TServiceComponent, object>> func,
                                                        Mock<TServiceComponent> component)
        where TServiceComponent : class
    {
        public ProvisionerComposer<TServiceComponent> As(object instance)
        {
            component.Setup(func).Returns(instance);
            return this;
        }

        public TService AndNothingElse()
            => componentProvisionnerContext.BaseBuilderContext.Provision();

        public MockServiceComponentProvisioner<TServiceComponent> Also()
            => componentProvisionnerContext;

        public MockServiceBuilderBase<TService, TMockBuilder> And()
            => componentProvisionnerContext.BaseBuilderContext;
    }
}
