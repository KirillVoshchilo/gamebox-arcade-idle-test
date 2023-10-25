using UnityEngine;
using VContainer;

public sealed class BuildingSiteEntity : MonoBehaviour, IEntity, IDestructable
{
    [SerializeField] private InteractionRequirementsComp _buildRequirements;
    [SerializeField] private InteractableComp _interactableComp;
    [SerializeField] private Transform _requirementsPanelTarget;
    [SerializeField] private AScriptableFactory _buildingFactory;
    [SerializeField] private Transform _builderTransform;

    private PlayerInventorySystem _playerInventory;
    private WorldCanvasStorage _worldCanvasStorage;

    [Inject]
    public void Construct(PlayerInventorySystem playerInventorySystem,
    WorldCanvasStorage worldCanvasStorage,
    IAppInputSystem appInputSystem)
    {
        _interactableComp.Construct(appInputSystem);
        _playerInventory = playerInventorySystem;
        _worldCanvasStorage = worldCanvasStorage;
        _interactableComp.OnEnable.AddListener(OnEnabled);
        _interactableComp.OnPerformed.AddListener(OnPerformedInteraction);
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
            _worldCanvasStorage.RequirementsPanel.SetPosition(_requirementsPanelTarget.position);
            _worldCanvasStorage.RequirementsPanel.gameObject.SetActive(true);
            _worldCanvasStorage.RequirementsPanel.FillWithItems(_buildRequirements.Requirements);
            _worldCanvasStorage.RequirementsPanel.IsEnable = true;
            if (CheckRequirement())
            {
                _interactableComp.IsValid = true;
                _worldCanvasStorage.InteractIcon.SetPosition(_interactableComp.IconTransform.position);
                _worldCanvasStorage.InteractIcon.gameObject.SetActive(true);
                _worldCanvasStorage.InteractIcon.IsEnable = true;
                _worldCanvasStorage.InteractIcon.OpenTip();
            }
            else _interactableComp.IsValid = false;
        }
        else
        {
            _worldCanvasStorage.RequirementsPanel.IsEnable = false;
            _worldCanvasStorage.RequirementsPanel.Clear();
            _worldCanvasStorage.RequirementsPanel.gameObject.SetActive(false);
            _worldCanvasStorage.InteractIcon.CloseProgress();
            _worldCanvasStorage.InteractIcon.CloseTip();
            _worldCanvasStorage.InteractIcon.IsEnable = false;
            _worldCanvasStorage.InteractIcon.gameObject.SetActive(false);
        }
    }
    private void OnPerformedInteraction()
    {
        if (!CheckRequirement())
            return;
        foreach (ItemCount item in _buildRequirements.Requirements)
            _playerInventory.RemoveItem(item.Key, item.Count);
        _buildingFactory.Parent = _builderTransform;
        _buildingFactory.Create();
        Destroy(gameObject);
    }
    private bool CheckRequirement()
    {
        foreach (ItemCount item in _buildRequirements.Requirements)
        {
            if (_playerInventory.GetCount(item.Key) < item.Count)
                return false;
        }
        return true;
    }
    public T Get<T>() where T : class
    {
        if (typeof(T) == typeof(InteractableComp))
            return _interactableComp as T;
        return null;
    }
}
