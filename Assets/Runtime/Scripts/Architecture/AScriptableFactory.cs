using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public abstract class AScriptableFactory : ScriptableObject
{
    private Transform _parent;

    public Transform Parent
    {
        get => _parent;
        set => _parent = value;
    }

    public abstract void Create();
    public abstract void Remove(GameObject obj);
}
[CreateAssetMenu]
public class ShopFactory : AScriptableFactory
{
    [SerializeField] private ShopEntity _shop;

    private UIController _uiController;
    private WorldCanvasStorage _worldCanvasStorage;
    private IAppInputSystem _appInputSystem;

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
        ShopEntity entity = Instantiate(_shop, Parent.position, Parent.rotation);
        entity.Construct(_uiController, _worldCanvasStorage, _appInputSystem);
    }
    public override void Remove(GameObject obj)
    {
        ShopEntity shopEntity = obj.GetComponent<ShopEntity>();
        shopEntity.Destruct();
        Destroy(obj);
    }
}