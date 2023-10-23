using System;
using Unity.VisualScripting;
using UnityEngine;
using VContainer;

public sealed class BuildingSiteEntity : MonoBehaviour, IEntity
{
    [SerializeField] private InteractionRequirements _buildRequirements;
    [SerializeField] private InteractableComp _interactableComp;
    [SerializeField] private Transform _requirementsPanelTarget;
    [SerializeField] private GameObject _buildingPrefab;
    [SerializeField] private Transform _builderTransform;

    private IAppInputSystem _appInputSystem;
    private PlayerInventorySystem _playerInventory;
    private WorldCanvasStorage _worldCanvasStorage;

    [Inject]
    public void Construct(PlayerInventorySystem playerInventorySystem,
    WorldCanvasStorage worldCanvasStorage,
    IAppInputSystem appInputSystem)
    {
        Debug.Log("Сконструировал FieldEntity");
        _appInputSystem = appInputSystem;
        _interactableComp.Construct(appInputSystem);
        _playerInventory = playerInventorySystem;
        _worldCanvasStorage = worldCanvasStorage;
        _interactableComp.OnEnable.AddListener(OnEnabled);
        _interactableComp.OnPerformed.AddListener(OnPerformedInteraction);
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
        GameObject instance = Instantiate(_buildingPrefab, _builderTransform.position, _builderTransform.rotation);
        Destroy(gameObject);
    }

    private bool CheckRequirement()
    {
        foreach (ItemCount item in _buildRequirements.Requirements)
        {
            if (_playerInventory.GetCount(item.Name.Value) < item.Count)
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
