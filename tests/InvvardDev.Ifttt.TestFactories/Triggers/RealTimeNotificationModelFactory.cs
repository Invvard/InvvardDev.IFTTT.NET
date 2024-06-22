using Bogus;
using InvvardDev.Ifttt.Toolkit;

namespace InvvardDev.Ifttt.TestFactories.Triggers;

public class RealTimeNotificationModelFactory : Faker<RealTimeNotificationModel>
{
    // Create a static instance of the factory
    public static RealTimeNotificationModelFactory Instance { get; } = new();

    private const string TriggerIdentityRuleSet = "triggerIdentity";

    private const string UserIdRuleSet = "userId";

    private RealTimeNotificationModelFactory()
    {
        base.RuleSet(TriggerIdentityRuleSet,
                     rule => rule.CustomInstantiator(f => RealTimeNotificationModel.CreateTriggerIdentity(f.Random.AlphaNumeric(10))))
            .RuleSet(UserIdRuleSet, rule => rule.RuleFor(x => x.TriggerIdentity, _ => null)
                                                .RuleFor(x => x.UserId, f => f.Random.AlphaNumeric(10)));
    }

    public RealTimeNotificationModel CreateTriggerIdentity() => base.Generate(TriggerIdentityRuleSet);

    public RealTimeNotificationModel CreateUserId() => base.Generate(UserIdRuleSet);

    public List<RealTimeNotificationModel> CreateTriggerIdentities(int count)
        => base.Generate(count, TriggerIdentityRuleSet);

    public List<RealTimeNotificationModel> CreateUserIds(int count) => base.Generate(count, UserIdRuleSet);
}
