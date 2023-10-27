using App.Architecture;
using App.Architecture.AppData;
using App.Architecture.AppInput;
using App.Content.Entities;
using App.Logic;
using UnityEngine;
using VContainer;

namespace App.Content.Player
{
    public sealed class PlayerEntity : MonoBehaviour, IEntity
    {
        [SerializeField] private PlayerData _playerData;

        private PlayerMoveHandler _moveHandler;
        private PlayerInventorySystem _playerInventorySystem;
        private bool _isEnable;

        public bool IsEnable
        {
            get => _isEnable;
            set
            {
                _isEnable = value;
                if (value)
                {
                    _playerData.TriggerComponent.OnExit.AddListener(OnExitEntity);
                    _playerData.TriggerComponent.OnEnter.AddListener(OnEnterEntity);
                    _moveHandler.IsEnable = true;
                }
                else
                {
                    _playerData.TriggerComponent.OnExit.ClearListeners();
                    _playerData.TriggerComponent.OnEnter.ClearListeners();
                    _moveHandler.IsEnable = true;
                }
            }
        }

        [Inject]
        public void Construct(IAppInputSystem appInputSystem,
            CamerasStorage camerasStorage,
            PlayerInventorySystem playerInventorySystem)
        {
            _playerData.AppInputSystem = appInputSystem;
            _playerData.MainCameraTransform = camerasStorage.MainCamera.transform;
            _playerInventorySystem = playerInventorySystem;
            _moveHandler = new PlayerMoveHandler(_playerData)
            {
                IsEnable = true
            };
            _playerData.TriggerComponent.OnExit.AddListener(OnExitEntity);
            _playerData.TriggerComponent.OnEnter.AddListener(OnEnterEntity);
        }
        public T Get<T>() where T : class
            => null;

        private void OnExitEntity(Collider collider)
        {
            if (!collider.TryGetComponent(out IEntity entity))
                return;
            InteractableComp interactableComp = entity.Get<InteractableComp>();
            if (interactableComp != null && interactableComp == _playerData.InteractionEntity)
            {
                _playerData.InteractionEntity.IsEnable = false;
                _playerData.InteractionEntity = null;
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
                        {
                            interactableComp.IsValid = false;
                            return;
                        }
                    }
                }
                if (_playerData.InteractionEntity != null)
                    _playerData.InteractionEntity.IsEnable = false;
                interactableComp.IsValid = true;
                interactableComp.IsEnable = true;
                _playerData.InteractionEntity = interactableComp;
            }
        }
    }
}