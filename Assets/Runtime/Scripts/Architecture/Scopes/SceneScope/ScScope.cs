using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

public class ScScope : LifetimeScope
{
    [SerializeField] private PlayerEntity _playerEntity;
    [SerializeField] private CamerasStorage _camerasStorage;
    [SerializeField] private WorldCanvasStorage _worldCanvasStorage;
    [SerializeField] private IconsConfiguration _iconsConfiguration;
    [SerializeField] private UIController _uiController;
    [SerializeField] private Configuration _configuration;
    [SerializeField] private ShopFactory[] _shopFactories;

    protected override void Configure(IContainerBuilder builder)
    {
        Debug.Log("��������������� SceneScope");
        DontDestroyOnLoad(this.gameObject);
        _iconsConfiguration.Construct();
        builder.RegisterComponent(_iconsConfiguration);
        builder.RegisterComponent(_configuration);
        builder.RegisterComponent(_worldCanvasStorage);
        builder.RegisterComponent(_playerEntity);
        builder.RegisterComponent(_uiController);
        builder.RegisterComponent(_camerasStorage);
        builder.Register<LevelLoader>(Lifetime.Singleton)
            .AsSelf();

        builder.RegisterBuildCallback((container) =>
        {
            IAppInputSystem appInputSystem = container.Resolve<IAppInputSystem>();
            foreach (ShopFactory shopFactory in _shopFactories)
                shopFactory.Construct(appInputSystem, _uiController, _worldCanvasStorage);
        });
    }
}
