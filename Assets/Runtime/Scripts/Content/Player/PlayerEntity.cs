﻿using App.Architecture;
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


        public bool IsEnable
        {
            get => _playerData.IsEnable;
            set
            {
                _playerData.IsEnable = value;
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
            _playerData.PlayerInventorySystem = playerInventorySystem;
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
            InteractionComp interactableComp = entity.Get<InteractionComp>();
            if (interactableComp != null && interactableComp == _playerData.InteractionEntity)
            {
                _playerData.InteractionEntity.IsInFocus = false;
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
            InteractionComp interactableComp = entity.Get<InteractionComp>();
            if (interactableComp != null)
            {
                if (_playerData.InteractionEntity != null)
                    _playerData.InteractionEntity.IsInFocus = false;
                interactableComp.IsInFocus = true;
                _playerData.InteractionEntity = interactableComp;
            }
        }
    }
}