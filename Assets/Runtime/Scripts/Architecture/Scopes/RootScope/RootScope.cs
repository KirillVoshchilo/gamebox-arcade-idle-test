using App.Architecture.AppInput;
using VContainer;
using VContainer.Unity;

namespace App.Architecture.Scopes
{
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
}