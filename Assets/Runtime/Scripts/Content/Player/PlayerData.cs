using App.Architecture;
using App.Architecture.AppInput;
using App.Components;
using App.Content.Entities;
using System;
using UnityEngine;

namespace App.Content.Player
{
    [Serializable]
    public sealed class PlayerData
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Transform _transform;
        [SerializeField] private float _defaultMovingSpeed;
        [SerializeField] private TriggerComponent _triggerComponent;

        private PlayerInventorySystem _playerInventorySystem;
        private bool _isMoving;
        private float _movingSpeed;
        private Transform _mainCameraTransform;
        private InteractionComp _interactionEntity;
        private IAppInputSystem _appInputSystem;
        private bool _isEnable;

        public bool IsMoving { get => _isMoving; set => _isMoving = value; }
        public float MovingSpeed { get => _movingSpeed; set => _movingSpeed = value; }
        public Transform MainCameraTransform { get => _mainCameraTransform; set => _mainCameraTransform = value; }
        public InteractionComp InteractionEntity { get => _interactionEntity; set => _interactionEntity = value; }
        public float DefaultMovingSpeed => _defaultMovingSpeed;
        public Rigidbody Rigidbody => _rigidbody;
        public Transform Transform => _transform;
        public TriggerComponent TriggerComponent => _triggerComponent;
        public IAppInputSystem AppInputSystem { get => _appInputSystem; set => _appInputSystem = value; }
        public PlayerInventorySystem PlayerInventorySystem { get => _playerInventorySystem; set => _playerInventorySystem = value; }
        public bool IsEnable { get => _isEnable; set => _isEnable = value; }
    }
}