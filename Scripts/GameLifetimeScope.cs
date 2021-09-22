using Scripts.Player;
using Scripts.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Scripts
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private ViewLife _viewLife;
        [SerializeField] private PlayerVital _playerVital;
    
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<ViewLife>(Lifetime.Singleton);
            builder.Register<VContainerLifePresenter>(Lifetime.Singleton);
//            builder.RegisterComponentInHierarchy<ViewLife>(uiViewLife);
//            builder.RegisterComponentInHierarchy<PlayerVital>(_playerVital);
            builder.RegisterEntryPoint<VContainerLifePresenter>();
//            builder.Register<VCoLifePresenter>(Lifetime.Singleton);
//            builder.Register<HelloWorldService>(Lifetime.Singleton);
//            builder.RegisterEntryPoint<GamePresenter>();
        }
    }
}
