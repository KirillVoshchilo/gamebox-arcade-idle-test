using System;
using UnityEngine;
using VContainer;

public sealed class ShopEntity : MonoBehaviour, IEntity, IDestructable
{
    [SerializeField] private InteractableComp _interactableComp;
    [SerializeField] private ItemCost[] _priceList;

    private WorldCanvasStorage _worldCanvasStorage;
    private UIController _uiController;

    [Inject]
    public void Construct(UIController uiController,
        WorldCanvasStorage worldCanvasStorage,
        IAppInputSystem appInputSystem)
    {
        _uiController = uiController;
        Debug.Log("Сконструировал ShopEntity");
        _interactableComp.Construct(appInputSystem);
        _worldCanvasStorage = worldCanvasStorage;
        _interactableComp.OnEnable.AddListener(OnEnabled);
        _interactableComp.OnPerformed.AddListener(OnPerformedInteraction);
    }
    public T Get<T>() where T : class
    {
        if (typeof(T) == typeof(InteractableComp))
            return _interactableComp as T;
        return null;
    }

    public void Destruct()
    {
        _interactableComp.OnEnable.ClearListeners();
        _interactableComp.OnPerformed.ClearListeners();
    }

    private void OnEnabled(bool obj)
    {
        if (obj)
        {
            _interactableComp.IsValid = true;
            _worldCanvasStorage.InteractIcon.SetPosition(_interactableComp.IconTransform.position);
            _worldCanvasStorage.InteractIcon.gameObject.SetActive(true);
            _worldCanvasStorage.InteractIcon.IsEnable = true;
            _worldCanvasStorage.InteractIcon.OpenTip();
        }
        else
        {
            _worldCanvasStorage.InteractIcon.CloseProgress();
            _worldCanvasStorage.InteractIcon.CloseTip();
            _worldCanvasStorage.InteractIcon.IsEnable = false;
            _worldCanvasStorage.InteractIcon.gameObject.SetActive(false);
        }
    }
    private void OnPerformedInteraction()
        => _uiController.OpenShop(_priceList);
}
