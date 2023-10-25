using UnityEngine;
using VContainer;

public sealed class PlayerEntity : MonoBehaviour, IEntity
{
    [SerializeField] private PlayerMoveHandler _moveHandler;
    [SerializeField] private TriggerComponent _triggerComponent;

    private InteractableComp _interactEntity;
    private PlayerInventorySystem _playerInventorySystem;

    [Inject]
    public void Construct(IAppInputSystem appInputSystem,
        CamerasStorage camerasStorage,
        PlayerInventorySystem playerInventorySystem)
    {
        _playerInventorySystem = playerInventorySystem;
        _moveHandler.Construct(appInputSystem, camerasStorage);
        _moveHandler.IsEnable = true;
        _triggerComponent.OnExit.AddListener(OnExitEntity);
        _triggerComponent.OnEnter.AddListener(OnEnterEntity);
    }
    public T Get<T>() where T : class
        => null;

    private void OnExitEntity(Collider collider)
    {
        if (!collider.TryGetComponent(out IEntity entity))
            return;
        InteractableComp interactableComp = entity.Get<InteractableComp>();
        if (interactableComp != null && interactableComp == _interactEntity)
        {
            _interactEntity.IsEnable = false;
            _interactEntity = null;
        }
    }
    private void OnEnterEntity(Collider collider)
    {
        if (!collider.TryGetComponent(out IEntity entity))
            return;
        InteractableIntityEnter(entity);
    }
    private void InteractableIntityEnter(IEntity entity)
    {
        InteractableComp interactableComp = entity.Get<InteractableComp>();
        if (interactableComp != null)
        {
            InteractionRequirementsComp interactionRequirements = entity.Get<InteractionRequirementsComp>();
            if (interactionRequirements != null)
            {
                int count = interactionRequirements.Requirements.Length;
                for (int i = 0; i < count; i++)
                {
                    Key key = interactionRequirements.Requirements[i].Key;
                    int quantity = _playerInventorySystem.GetCount(key);
                    if (quantity < interactionRequirements.Requirements[i].Count)
                        return;
                }
            }
            if (_interactEntity != null)
                _interactEntity.IsEnable = false;
            interactableComp.IsValid = true;
            interactableComp.IsEnable = true;
            _interactEntity = interactableComp;
        }
    }
}
