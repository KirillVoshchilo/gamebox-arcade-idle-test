using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

public sealed class FieldEntity : MonoBehaviour, IEntity, IDestructable
{
    [SerializeField] private InteractableComp _interactableComp;
    [SerializeField] private Key _key;
    [SerializeField] private int _itemsCount;
    [SerializeField] private GameObject _crystal;
    [SerializeField] private InteractionRequirementsComp _fieldData;

    private PlayerInventorySystem _playerInventory;
    private WorldCanvasStorage _worldCanvasStorage;
    private IAppInputSystem _appInputSystem;

    [Inject]
    public void Construct(PlayerInventorySystem playerInventorySystem,
        WorldCanvasStorage worldCanvasStorage,
        IAppInputSystem appInputSystem)
    {
        _appInputSystem = appInputSystem;
        _interactableComp.Construct(appInputSystem);
        _playerInventory = playerInventorySystem;
        _worldCanvasStorage = worldCanvasStorage;
        _interactableComp.OnRecovered.AddListener(OnRecovered);
        _interactableComp.OnEnable.AddListener(OnEnabled);
    }
    public T Get<T>() where T : class
    {
        if (typeof(T) == typeof(InteractableComp))
            return _interactableComp as T;
        if (typeof(T) == typeof(InteractionRequirementsComp))
            return _fieldData as T;
        return null;
    }
    public void Destruct()
    {
        _interactableComp.OnRecovered.ClearListeners();
        _interactableComp.OnEnable.ClearListeners();
        _interactableComp.OnStarted.ClearListeners();
        _interactableComp.OnCancel.ClearListeners();
        _interactableComp.OnPerformed.ClearListeners();
    }

    private void OnRecovered(bool obj)
        => _crystal.SetActive(obj);
    private void OnEnabled(bool value)
    {
        if (value)
        {
            _worldCanvasStorage.InteractIcon.SetPosition(_interactableComp.IconTransform.position);
            _worldCanvasStorage.InteractIcon.gameObject.SetActive(true);
            _worldCanvasStorage.InteractIcon.IsEnable = true;
            _worldCanvasStorage.InteractIcon.OpenTip();
            _interactableComp.OnStarted.AddListener(OnStartedInteracrtion);
            _interactableComp.OnCancel.AddListener(OnCancelInteraction);
            _interactableComp.OnPerformed.AddListener(OnPerformedInteraction);
        }
        else
        {
            _worldCanvasStorage.InteractIcon.CloseProgress();
            _worldCanvasStorage.InteractIcon.CloseTip();
            _worldCanvasStorage.InteractIcon.IsEnable = false;
            _worldCanvasStorage.InteractIcon.gameObject.SetActive(false);
            _interactableComp.OnStarted.ClearListeners();
            _interactableComp.OnCancel.ClearListeners();
            _interactableComp.OnPerformed.ClearListeners();
        }
    }

    private void OnPerformedInteraction()
    {
        _crystal.SetActive(false);
        _playerInventory.AddItem(_key, _itemsCount);
    }
    private void OnCancelInteraction()
    {
        _worldCanvasStorage.InteractIcon.CloseProgress();
        _worldCanvasStorage.InteractIcon.OpenTip();
    }
    private void OnStartedInteracrtion()
    {
        _worldCanvasStorage.InteractIcon.CloseTip();
        _worldCanvasStorage.InteractIcon.OpenProgress();
        ProgressVisualize()
            .Forget();
    }
    private async UniTask ProgressVisualize()
    {
        while (_interactableComp.IsInteracting)
        {
            _worldCanvasStorage.InteractIcon.SetProgress(_appInputSystem.InteractionPercentage);
            await UniTask.Delay(50);
        }
    }
}