using UnityEngine;
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
    [SerializeField] private ShopFactory _shopFactory;

    protected override void Configure(IContainerBuilder builder)
    {
        _iconsConfiguration.Construct();
        builder.RegisterComponent(_iconsConfiguration);
        builder.RegisterComponent(_worldCanvasStorage);
        builder.RegisterComponent(_playerEntity);
        builder.RegisterComponent(_uiController);
        builder.RegisterComponent(_camerasStorage);
        Debug.Log("Сконфигурировал SceneScope");
        builder.RegisterBuildCallback((container) =>
        {
            PlayerInventorySystem playerInventorySystem = container.Resolve<PlayerInventorySystem>();
            int count = _configuration.StartInventoryConfiguration.Items.Length;
            for (int i = 0; i < count; i++)
            {
                string name = _configuration.StartInventoryConfiguration.Items[i].Key.Value;
                int quantity = _configuration.StartInventoryConfiguration.Items[i].Count;
                playerInventorySystem.AddItem(name, quantity);
            }
            IAppInputSystem appInputSystem = container.Resolve<IAppInputSystem>();
            _shopFactory.Construct(appInputSystem, _uiController, _worldCanvasStorage);
        });
    }
}
