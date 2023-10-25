using UnityEngine;
using VContainer;
using VContainer.Unity;

public sealed class RootScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<AppInputSystem>(Lifetime.Singleton)
            .As<IAppInputSystem>();
        builder.Register<PlayerInventorySystem>(Lifetime.Singleton)
            .AsSelf();
    }
}