using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

[Serializable]
public sealed class PlayerMoveHandler
{
    [SerializeField] private float _speed;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform _transform;

    private IAppInputSystem _appInputSystem;
    private Transform _cameraTransform;
    private bool _isEnable;
    private bool _isMoving;

    public bool IsEnable
    {
        get => _isEnable;
        set
        {
            _isEnable = value;
            if (value)
            {
                _appInputSystem.OnMovingStarted.AddListener(StartMove);
                _appInputSystem.OnMovingStoped.AddListener(StopMove);
            }
            else
            {
                _appInputSystem.OnMovingStarted.RemoveListener(StartMove);
                _appInputSystem.OnMovingStoped.RemoveListener(StopMove);
                StopMove();
            }
        }
    }

    public void Construct(IAppInputSystem appInputSystem,
        CamerasStorage camerasStorage)
    {
        _cameraTransform = camerasStorage.MainCamera.transform;
        _appInputSystem = appInputSystem;
    }

    private void StartMove()
    {
        _isMoving = true;
        MoveProcess()
            .Forget();
    }
    private void StopMove()
        => _isMoving = false;
    private async UniTask MoveProcess()
    {
        while (_isMoving && _isEnable)
        {
            Move();
            await UniTask.DelayFrame(1);
        }
        _isMoving = false;
    }
    private void Move()
    {
        Vector3 target = _transform.position;
        Vector3 forwarDirection = _cameraTransform.forward - (Vector3.up * Vector3.Dot(_cameraTransform.forward, Vector3.up));
        target += _appInputSystem.MoveDirection.x * _speed * _cameraTransform.right;
        target += _appInputSystem.MoveDirection.y * _speed * forwarDirection.normalized;
        Debug.Log($"_speed: {_speed} _appInputSystem.MoveDirection.x: {_appInputSystem.MoveDirection.x} _appInputSystem.MoveDirection.y: {_appInputSystem.MoveDirection.y}" +
            $"Time.deltaTime: {Time.deltaTime}");
        Debug.Log($"");
        _rigidbody.MovePosition(target);
    }
}
