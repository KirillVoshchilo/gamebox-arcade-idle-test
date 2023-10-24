using UnityEngine;
using VContainer;

[CreateAssetMenu]
public class ShopFactory : AScriptableFactory
{
    [SerializeField] private ShopEntity _shop;

    private UIController _uiController;
    private WorldCanvasStorage _worldCanvasStorage;
    private IAppInputSystem _appInputSystem;
    private readonly SEvent<ShopEntity> _onCreated = new();

    public SEvent<ShopEntity> OnCreated
        => _onCreated;

    [Inject]
    public void Construct(IAppInputSystem appInputSystem,
        UIController uiController,
        WorldCanvasStorage worldCanvasStorage)
    {
        _uiController = uiController;
        _worldCanvasStorage = worldCanvasStorage;
        _appInputSystem = appInputSystem;
    }
    public override void Create()
    {
        ShopEntity instance = Instantiate(_shop, Parent.position, Parent.rotation);
        instance.gameObject.name = _shop.name;
        instance.Construct(_uiController, _worldCanvasStorage, _appInputSystem);
        _onCreated.Invoke(instance);
    }
    public override void Remove(GameObject obj)
    {
        ShopEntity shopEntity = obj.GetComponent<ShopEntity>();
        shopEntity.Destruct();
        Destroy(obj);
    }
}