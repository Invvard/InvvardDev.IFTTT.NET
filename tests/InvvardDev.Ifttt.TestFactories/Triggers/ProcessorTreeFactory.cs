﻿using Bogus;
using InvvardDev.Ifttt.Models.Core;
using InvvardDev.Ifttt.Models.Trigger;

namespace InvvardDev.Ifttt.TestFactories.Triggers;

public class ProcessorTreeFactory : EntityFactoryBase<ProcessorTree>
{
    protected override Faker<ProcessorTree> Configure()
        => new Faker<ProcessorTree>()
            .CustomInstantiator(f => new ProcessorTree(f.Random.Words(2), default!, f.PickRandom<ProcessorKind>()));
}
